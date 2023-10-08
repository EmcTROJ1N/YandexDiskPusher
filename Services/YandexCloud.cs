using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using YaDiskObserver.Interfaces;
using YaDiskObserver.Models;

namespace YaDiskObserver.Services;

public class YandexCloud: IPushService
{
    private HttpClient _client = new();
    private string _baseUri;

    public YandexCloud(string token)
    {
        _client.DefaultRequestHeaders.Add("Authorization", token);
        _baseUri = "https://cloud-api.yandex.net/v1";
    }

    private async Task<bool> Contains(string path)
    {
        string? directoryName = Path.GetDirectoryName(path)?.Replace("\\", "/");
        string? fileName = Path.GetFileName(path).Replace("\\", "/");
        if (directoryName == null || fileName == null)
            throw new ArgumentException();
        
        return (from file in await GetDataAsync(directoryName)
            select file.name).Contains(fileName);
    }

    private async Task CreateFolderAsync(string path)
    {
        string request = $"{_baseUri}/disk/resources";
        request += $"?path={Uri.EscapeDataString(path)}";
        
        HttpResponseMessage response = await _client.PutAsync(request, null);
        string json = response.Content.ReadAsStringAsync().Result;

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException();
    }

    private async Task<List<CloudFileInfoModel>> GetDataAsync(string path)
    {
        string request = $"https://cloud-api.yandex.net/v1/disk/resources";
        request += $"?path={Uri.EscapeDataString(path)}";
        request += $"&limit=500";

        HttpResponseMessage response = await _client.GetAsync(request);
        string json = response.Content.ReadAsStringAsync().Result;
        JToken? obj = JObject.Parse(json)?["_embedded"]?["items"];
        if (obj == null)
            throw new NullReferenceException();
        List<CloudFileInfoModel>? result = obj.ToObject<JArray>()?.ToObject<List<CloudFileInfoModel>>();
        if (result == null)
            throw new SerializationException();
        
        return result;
    }

    private async Task CreateTreeAsync()
    {
        string folderName = SysFuncs.GetDateTime();

        if (!await Contains($"/{folderName}"))
            await CreateFolderAsync($"/{folderName}");
        if (!await Contains($"/{folderName}/Photo"))
            await CreateFolderAsync($"/{folderName}/Photo");
        if (!await Contains($"/{folderName}/Audio"))
            await CreateFolderAsync($"/{folderName}/Audio");
        if (!await Contains($"/{folderName}/Video"))
            await CreateFolderAsync($"/{folderName}/Video");
    }

    public async Task PushAsync(string sourcePath, string destPath)
    {
        await CreateTreeAsync();
        
        bool filenameUsed = await Contains(destPath);
        for (int i = 1; filenameUsed; i++)
        {
            string newPath = $"{i}_{destPath}";
            filenameUsed = await Contains(newPath);
            destPath = newPath;
        }

        string request = $"{_baseUri}/disk/resources/upload";
        request += $"?path={Uri.EscapeDataString(destPath)}";

        HttpResponseMessage message = await _client.GetAsync(request);
        if (!message.IsSuccessStatusCode)
            throw new HttpRequestException();
        string json = await message.Content.ReadAsStringAsync();
        string? link = JObject.Parse(json)["href"]?.Value<string>();
        if (link == null)
            throw new SerializationException();
        
        
        MultipartFormDataContent content = new MultipartFormDataContent();
        byte[] fileBytes = File.ReadAllBytes(sourcePath);

        HttpResponseMessage newMessage = await _client.PutAsync(link, new ByteArrayContent(fileBytes));
        if (!message.IsSuccessStatusCode)
        {
            string newJson = await newMessage.Content.ReadAsStringAsync();
            throw new HttpRequestException();
        }
        
    }
}
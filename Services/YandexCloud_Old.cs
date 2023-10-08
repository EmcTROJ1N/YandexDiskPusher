using YaDiskObserver.Interfaces;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using System.Collections.Generic;
using System.Threading.Channels;

namespace YaDiskObserver.Services;

public class YandexCloud_Old: IPushService
{
    public IDiskApi Api;
    
    public YandexCloud_Old(string token) =>
        Api = new DiskHttpApi(token);

    private async Task<Resource> GetData(string path)
    {
        return await Api.MetaInfo.GetInfoAsync(new ResourceRequest()
        {
            Path = path,
        }, CancellationToken.None);
    }
    
    private async Task<bool> Contains(string path, string data)
    {
        Resource response = await GetData(path);
        return !response.Embedded.Items.TrueForAll(res =>
            res.Path != $"disk:{Path.Combine("/", path, data)}");
    }

    private async Task CreateFolderAsync(string path) =>
        await Api.Commands.CreateDictionaryAsync(path,
            CancellationToken.None);

    private async Task CreateTreeAsync()
    {
        string folderName = DateTime.Today.ToShortDateString();
        folderName = folderName.Replace("/", "_");
        Resource response = await GetData("/");

        if (!await Contains("/", folderName))
            await CreateFolderAsync($"/{folderName}");
        if (!await Contains(folderName, "Photo"))
            await CreateFolderAsync($"/{folderName}/Photo");
        if (!await Contains(folderName, "Audio"))
            await CreateFolderAsync($"/{folderName}/Audio");
        if (!await Contains(folderName, "Video"))
            await CreateFolderAsync($"/{folderName}/Video");
    }

    public async Task PushAsync(string sourcePath, string destPath)
    {
        await CreateTreeAsync();

        bool filenameUsed = await Contains(Path.GetDirectoryName(destPath), Path.GetFileName(destPath));
        for (int i = 1; filenameUsed; i++)
        {
            string newPath = $"{Path.GetFullPath(destPath)}/{Path.GetFileNameWithoutExtension(destPath)}_{i}{Path.GetExtension(destPath)}";
            filenameUsed = await Contains(Path.GetDirectoryName(newPath), Path.GetFileName(newPath));
            destPath = newPath;
        }

        Console.WriteLine($"Pushing {sourcePath}");
        await Api.Files.UploadFileAsync(path: destPath,
            overwrite: false,
            localFile: sourcePath,
            cancellationToken: CancellationToken.None);
        Console.WriteLine($"Pushed {sourcePath}");
    }
}
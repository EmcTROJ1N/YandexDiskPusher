using Microsoft.Extensions.DependencyInjection;
using YaDiskObserver.Interfaces;

namespace YaDiskObserver;

public class App
{
    public INotifyService NotifyService { get; set; }
    public IObserverService ObserverService { get; set; }
    public IPushService PushService { get; set; }
    public string FolderPath { get; }
    
    public App(INotifyService notifyService,
        IObserverService observerService,
        IPushService pushService)
    {
        NotifyService = notifyService;
        ObserverService = observerService;
        PushService = pushService;
        FolderPath = Configuration.ObservablePath;
    }

    private void CreateTree(string currentPath)
    {
        if (!Directory.Exists(currentPath))
            Directory.CreateDirectory(currentPath);
        if (!Directory.Exists($"{currentPath}/Photo"))
            Directory.CreateDirectory($"{currentPath}/Photo");
        if (!Directory.Exists($"{currentPath}/Audio"))
            Directory.CreateDirectory($"{currentPath}/Audio");
        if (!Directory.Exists($"{currentPath}/Video"))
            Directory.CreateDirectory($"{currentPath}/Video");
    }

    private async Task ObserverOnCreatingAsync(string path)
    {
        await NotifyService.SendNotifyAsync($"Detected new batch: {path}");

        List<string> yaDiskUploadFiles = new List<string>();
        
        void SortFile(string currentPath, string file)
        {
            string? sourceFolder = null;
            if (new [] { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".tiff", ".arw" }.Contains(Path.GetExtension(file).ToLower()))
                sourceFolder = "Photo";
            if (new [] { ".mp4", ".mkv", ".avi", ".mov", ".wmw", ".m4a" }.Contains(Path.GetExtension(file).ToLower()))
                sourceFolder = "Video";
            if (new [] { ".mp3", ".wav", ".flac", ".aac", ".wma" }.Contains(Path.GetExtension(file).ToLower()))
                sourceFolder = "Audio";

            try
            {
                if (sourceFolder == null)
                {
                    File.Delete(file);
                    return;
                }
                
                string combinedPath = Path.Combine(currentPath, sourceFolder, Path.GetFileName(file));
                File.Move(file, combinedPath);
                yaDiskUploadFiles.Add(combinedPath);
            }
            catch (UnauthorizedAccessException ex)
            {
                NotifyService.SendErrorAsync(ex.Message);
            }
            catch (IOException)
            {
                int i = 1;
                bool filenameUsed = false;
                do
                {
                    try
                    {
                        File.Move(file, Path.Combine(currentPath, sourceFolder, $"{i}_{Path.GetFileName(file)}"));
                    }
                    catch (IOException)
                    {
                        filenameUsed = true;
                    }

                    i++;
                } while (!filenameUsed);
            }
        }

        string shortToday = SysFuncs.GetDateTime();
        string currentPath = $"{FolderPath}/{shortToday}";
        string yaDiskPath = $"/{shortToday}";
        
        CreateTree(currentPath);

        if (File.Exists(path))
            SortFile(currentPath, path);
        else if (Directory.Exists(path))
        {
            List<string> allFiles = new List<string>();
            try
            {
                allFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();
            }
            catch (UnauthorizedAccessException ex)
            {
                await NotifyService.SendErrorAsync(ex.Message);
            }


            foreach (string file in allFiles)
                SortFile(currentPath, file);
            
            Directory.Delete(path, true);
        }
        else
            await NotifyService.SendErrorAsync($"Could not find path: {path}");

        if (yaDiskUploadFiles.Count > 0)
        {
            await UploadFiles(yaDiskUploadFiles);
            NotifyService.SendNotifyAsync("A new batch of files has been uploaded to the cloud").Wait();
        }

    }

    private async Task UploadFiles(IEnumerable<string> files)
    {
        foreach (string file in files)
        {
            string destPath = "/" + string.Join("/", file.Split(new char[] { '/', '\\' }).TakeLast(3));
            //Thread.Sleep(1000);

            await NotifyService.SendNotifyAsync($"File {file} uploading");
            await PushService.PushAsync(file, destPath);
            Console.WriteLine(destPath);
            await NotifyService.SendNotifyAsync($"File {file} was uploaded");

        }
    }

    public async void Run() =>
        ObserverService.OnCreating += ObserverOnCreatingAsync;
}
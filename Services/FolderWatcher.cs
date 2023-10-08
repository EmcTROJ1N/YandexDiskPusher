using YaDiskObserver.Interfaces;

namespace YaDiskObserver.Services;

public class FolderWatcher: IObserverService
{
    public event ObserverNotify? OnCreating;
    private FileSystemWatcher Observer { get; set; }
    private string _path;
    public string Path
    {
        get => _path;
        private set =>
            _path = Directory.Exists(value) ? value :
                throw new DirectoryNotFoundException();
    }
    private List<string> _lastPaths = new();

    public FolderWatcher(string path)
    {
        Path = path;
        Observer = new FileSystemWatcher(Path);
        Observer.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;

        Observer.Created += ObserverOnCreated;
        Observer.IncludeSubdirectories = true;
        Observer.EnableRaisingEvents = true;
    }

    private void ObserverOnCreated(object sender, FileSystemEventArgs e)
    {
        string fullPath = e.FullPath.Replace("\\", "/");
        if (Directory.Exists(fullPath))
        {
            foreach (string file in Directory.GetFiles(fullPath))
            {
                FileSystemEventArgs args = new FileSystemEventArgs(
                    WatcherChangeTypes.Created,
                    System.IO.Path.GetDirectoryName(file)!,
                    System.IO.Path.GetFileName(file));
                ObserverOnCreated(sender, args);
            }
        }
        
        string[] fullPathSplitted = fullPath.Split( '/');
        
        if (File.Exists(fullPath) &&
            !fullPathSplitted.Contains(SysFuncs.GetDateTime()) &&
            !_lastPaths.Contains(fullPath))
        {
            _lastPaths.Add(fullPath);
            if (_lastPaths.Count > 100)
                _lastPaths.RemoveAt(0);
            
            OnCreating?.Invoke(fullPath);
        }
    }
}
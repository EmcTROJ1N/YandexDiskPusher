
namespace YaDiskObserver.Interfaces;

public interface IPushService
{
    public Task PushAsync(string sourcePath, string destPath);
}
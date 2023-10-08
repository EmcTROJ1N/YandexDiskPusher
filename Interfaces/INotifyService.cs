namespace YaDiskObserver.Interfaces;

public interface INotifyService
{
    public Task SendNotifyAsync(string message);
    public Task SendErrorAsync(string message);
}
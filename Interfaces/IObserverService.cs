namespace YaDiskObserver.Interfaces;

public delegate Task ObserverNotify(string creating);
public interface IObserverService
{
    public event ObserverNotify OnCreating;
}
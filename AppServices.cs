using Microsoft.Extensions.DependencyInjection;
using YaDiskObserver.Interfaces;
using YaDiskObserver.Services;

namespace YaDiskObserver;

public class AppServices: ServiceCollection
{
    private readonly ServiceProvider _serviceProvider;
    public App? App => _serviceProvider.GetService<App>();

    public AppServices()
    {
        this.AddTransient<App>()
            .AddTransient<INotifyService, TelegramNotify>(notify => new TelegramNotify(Configuration.TelegramBotToken))
            .AddTransient<IPushService, YandexCloud>(cloud => new YandexCloud(Configuration.YandexToken))
            .AddTransient<IObserverService, FolderWatcher>(watcher => new FolderWatcher(Configuration.ObservablePath))
            .AddTransient<App>();
        _serviceProvider = this.BuildServiceProvider();
    }
}
using YaDiskObserver;
using YaDiskObserver.Services;

class Program
{
    public static void Main(string[] args)
    {
        AppServices services = new AppServices();
        App? app = services.App;
        app?.Run();

        Console.WriteLine("To end the program press Ctrl + D");
        HotkeyManager.WaitForHotKeys(ConsoleModifiers.Control, ConsoleKey.D);
    }
}
namespace YaDiskObserver;

public static class HotkeyManager
{

    public static bool WaitForHotKeys(ConsoleModifiers modifier, ConsoleKey key)
    {
        do
        {
            ConsoleKeyInfo keypress = Console.ReadKey(true); 
            if ((ConsoleModifiers.Control & keypress.Modifiers) != 0)
                if (keypress.Key == ConsoleKey.D)
                    return true;
            
        } while (true);
    }
}
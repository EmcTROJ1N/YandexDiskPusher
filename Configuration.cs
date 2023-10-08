using System.Text.Json;

namespace YaDiskObserver;

public static class Configuration
{
    public static string ObservablePath { get; set; }
    public static string YandexToken { get; set; }
    public static string TelegramBotToken { get; set; }
    public static long TelegramUserId { get; set; }
    
    static Configuration()
    {
        string json = File.ReadAllText("Configuration.json");
        Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        if (data == null)
            throw new Exception("Serialization error");
        
        ObservablePath = data["ObservablePath"];
        YandexToken = data["YandexToken"];
        TelegramBotToken = data["TelegramBotToken"];
        TelegramUserId = long.Parse(data["TelegramUserId"]);
    }
}
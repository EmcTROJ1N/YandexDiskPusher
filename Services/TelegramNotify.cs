using YaDiskObserver.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace YaDiskObserver.Services;

public class TelegramNotify: INotifyService
{
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationToken = new();
    private long _ownerId = Configuration.TelegramUserId;

    public TelegramNotify(string token) =>
        _botClient = new TelegramBotClient(token);

    public async Task SendNotifyAsync(string message)
    {
        Message sentMessage = await _botClient.SendTextMessageAsync(
            chatId: _ownerId,
            text: message,
            cancellationToken: _cancellationToken.Token);
    }

    public async Task SendErrorAsync(string message)
    {
        Message sentMessage = await _botClient.SendTextMessageAsync(
            chatId: _ownerId,
            text: $"ERROR: {message}",
            cancellationToken: _cancellationToken.Token);
    }
}
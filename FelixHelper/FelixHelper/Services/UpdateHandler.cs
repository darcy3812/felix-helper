using FelixHelper.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FelixHelper.Services;

public class UpdateHandler(ITelegramBotClient bot, ILogger<UpdateHandler> logger, PromotionService promotionService) : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message } => OnMessage(message),
            _ => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(Message msg)
    {
        logger.LogInformation("Receive message type: {MessageType}", msg.Type);
        if (string.IsNullOrEmpty(msg.Text))
        {
            return;
        }
        await HandlePromotionCommand(msg);
    }

    async Task<Message> HandleUnknownCommand(Message msg)
    {
        return await bot.SendTextMessageAsync(msg.Chat, Consts.UnkownCommand, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> HandlePromotionCommand(Message msg)
    {
        logger.LogInformation("Received message: {UpdateType}", msg.Text);

        var parts = msg.Text.Split(' ', 2);
        if (parts.Length != 2)
        {
            return await bot.SendTextMessageAsync(msg.Chat, Consts.UnkownFormat, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
        }

        if (!Consts.Months.TryGetValue(parts[0], out var updatedMonth))
        {
            return await bot.SendTextMessageAsync(msg.Chat, Consts.UnkownMonth, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
        }

        var promotion = new PromotionModel(updatedMonth, parts[1]);
        promotionService.UpdatePromotion(promotion);
        return await bot.SendTextMessageAsync(msg.Chat, Consts.PromotionsUpdated, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

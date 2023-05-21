﻿using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KinopoiskBot
{
    internal abstract class Program
    {
        private static void Main(string[] args)
        {
            var client = new TelegramBotClient("5918832145:AAEJLhWvQt6ZNaoJNv_HNBUATaoK36v4W1Q");
            client.StartReceiving(Update, Error);
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }

        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                if (message.Text.ToLower().Contains("здарова"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Здоровее видали", cancellationToken: token);
                    return;
                }
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
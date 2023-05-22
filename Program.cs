using System;
using System.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace KinopoiskBot
{
    internal abstract class Program
    {
        private static void Main(string[] args)
        {
            var client = new TelegramBotClient(ConfigurationManager.AppSettings["TG_TOKEN"] ?? throw new InvalidOperationException());
            client.StartReceiving(Update, Error);
            Console.WriteLine("Bot launched successfully!");
            Console.ReadLine();
        }

        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var kinopoiskApi = new KinopoiskApi();
            var message = update.Message;
            Console.WriteLine($"{message?.Chat.Username} -> {message?.Text}");
            
            if (message?.Text != null)
            {
                switch (message.Text)
                {
                    case "/start":
                    case "/help":
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            "Привет! Назови фильм, о котором хочешь узнать, и я тебе расскажу о нём!", 
                            cancellationToken: token);
                        return;
                    
                    default:
                        var film = message.Text;
                        var (filmName, filmDescription) = kinopoiskApi.Search(film);

                        var answer = $"*{filmName}*\n\n{filmDescription}";
                        await botClient.SendTextMessageAsync(
                            message.Chat.Id,
                            answer,
                            cancellationToken: token,
                            parseMode: ParseMode.Markdown);
                        break;
                }
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
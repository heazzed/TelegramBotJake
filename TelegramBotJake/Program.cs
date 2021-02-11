using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
//using Telegram.Bot.Types.InlineQueryResults;

namespace TelegramBotJake
{
    class Program
    {
        static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1477584069:AAGrfgHioPB7FYEiLkhMVvA9sT_n6-JQvOE");

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;

            var me = Bot.GetMeAsync().Result; 
            Console.WriteLine(me.FirstName);

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnCallbackQueryReceived(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал кнопку {buttonText}");

            try
            {
                await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {buttonText}");
            }
            catch
            {}
            
        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text) //добавить вывод типа сообщения
                return;

            string name = $"{message.From.FirstName} {message.From.LastName}";

            Console.WriteLine($"{name} отправил сообщение: {message.Text}'");

            switch (message.Text)
            {
                case "/start":
                    string text =
@"Список команд:
/start - запуск бота
/inline - вывод меню
/keyboard - вывод клавиатуры";
                    await Bot.SendTextMessageAsync(message.From.Id, text);
                    break;
                case "/inline":
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Vk", "https://vk.com/milisov"),
                            InlineKeyboardButton.WithUrl("Telegram", "https://t.me/milisov")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Пункт 1"),
                            InlineKeyboardButton.WithCallbackData("Пункт 2")
                        }
                    });
                    await Bot.SendTextMessageAsync(message.From.Id, "Выберите пункт меню", replyMarkup: inlineKeyboard);
                    break;
                case "/keyboard":
                    var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    {
                        new[]
                        {
                           new KeyboardButton("Привет!"),
                           new KeyboardButton("Как дела?")
                        },
                        new[]
                        {
                            new KeyboardButton("Контакт"){ RequestContact = true},
                            new KeyboardButton("Геолокация"){ RequestLocation = true}
                        }
                    });
                    await Bot.SendTextMessageAsync(message.Chat.Id, "Сообщение", replyMarkup: replyKeyboard);
                    break;
                default:
                    break;
            }

        }
    }
}

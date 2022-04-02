using Discord;
using Discord.WebSocket;
using UwUBot;

namespace UwUBot
{
    internal class main
    {
        public static Task Main(string[] args) => new main().MainAsync();

        public async Task MainAsync()
        {
            string botTokenFileName = "botToken.tkt";
            string botToken = File.ReadAllText(botTokenFileName);

            //Create Bot and Setup and start
            DiscordSocketClient uwuBot = new DiscordSocketClient();
            await uwuBot.LoginAsync(TokenType.Bot, botToken);
            await uwuBot.StartAsync();
            
            while(uwuBot.ConnectionState != ConnectionState.Connected)
            {
                Thread.Sleep(100);
            }

            commandHanlder botCommandHanlder = new commandHanlder(uwuBot);
            await botCommandHanlder.installCommandsAsync();

            await Task.Delay(-1);
        }
    }
} 
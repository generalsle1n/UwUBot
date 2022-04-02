using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UwUBot
{
    internal class commandHanlder
    {
        private DiscordSocketClient client;
        private CommandService commands = new CommandService();
        private static char commandPrefix = '!';

        public commandHanlder(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task installCommandsAsync()
        {
            //Setup EventHandling and insert all Modules
            client.MessageReceived += handleTextCommandAsync;
            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: null);
        }

        private async Task handleTextCommandAsync(SocketMessage message)
        {
            SocketUserMessage userMessage = (SocketUserMessage)message;
            if(userMessage == null)
            {
                return;
            }

            //Where Prefix ends
            int argPos = 0;

            //Check if prefix is valid and is not an bot message
            if (!userMessage.Author.IsBot)
            {
                if (userMessage.HasCharPrefix(commandPrefix, ref argPos))
                {
                    //Create context from message
                    SocketCommandContext commandContext = new SocketCommandContext(client, userMessage);

                    //Execute Command
                    IResult commandResult = await commands.ExecuteAsync(commandContext, argPos, null);

                    if(commandResult.IsSuccess != true)
                    {
                        await userMessage.Channel.SendMessageAsync(userMessage.Author.Mention + $"An Error occurred: {commandResult.Error}");
                    }
                }
            }
        }
    }
}

using Discord.Commands;

namespace UwUBot.commands
{
    public class textCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task pingPong(string text)
        {
            Context.Channel.SendMessageAsync("pong");
        }
    }
}

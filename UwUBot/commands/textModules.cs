using Discord.Commands;

namespace UwUBot.commands
{
    public class textCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task pingPong()
        {
            Context.Channel.SendMessageAsync("pong");
        }
    }
}

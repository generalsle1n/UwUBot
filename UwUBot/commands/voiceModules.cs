using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

namespace UwUBot.commands
{
    public class voiceModules : ModuleBase<SocketCommandContext>
    {
        commandHelper helper = new commandHelper();

        [Command("hello", RunMode = RunMode.Async)]
        public async Task sayHello()
        {
            SocketUserMessage message = Context.Message;
            SocketGuildUser user = (SocketGuildUser)Context.User;
            
            SocketVoiceChannel channel = user.VoiceChannel;

            IAudioClient audioClient = await channel.ConnectAsync();

            AudioOutStream stream = audioClient.CreateDirectPCMStream(AudioApplication.Music);

            var lol = helper.getAudioTestFileStream("sound.wav");

        }
    }
}

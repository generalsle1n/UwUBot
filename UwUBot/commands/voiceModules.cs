using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System.Diagnostics;
using System.Net;
using YoutubeExplode;

namespace UwUBot.commands
{
    public class voiceModules : ModuleBase<SocketCommandContext>
    {
        private HttpClient httpClient = new HttpClient();
        private YoutubeClient ytClient = new YoutubeClient();

        [Command("play", RunMode = RunMode.Async)]
        public async Task playMp3(string url)
        {
            //GetAllData
            SocketGuildUser user = (SocketGuildUser)Context.User;
            SocketVoiceChannel channel = user.VoiceChannel;

            IAudioClient audioClient = await channel.ConnectAsync();

            //Create Process for Conversion
            Process ffmpeg = createFfmegProcess();

            //FileToStream
            Task<Stream> audioStreamFromWeb = httpClient.GetStreamAsync(url);
            
            //Start Conversion
            ffmpeg.Start();
            Stream output = ffmpeg.StandardOutput.BaseStream;
            Stream input = ffmpeg.StandardInput.BaseStream;
            audioStreamFromWeb.Result.CopyToAsync(input);
           
            //Create and start stream            
            AudioOutStream discord = audioClient.CreatePCMStream(AudioApplication.Voice);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }

        private Process createFfmegProcess()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "ffmpeg.exe",
                Arguments = $@"-i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            Process audioProcess = new Process();
            audioProcess.StartInfo = startInfo;

            return audioProcess;
        }
    }
}

using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System.Diagnostics;
using System.Net;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace UwUBot.commands
{
    public class voiceModules : ModuleBase<SocketCommandContext>
    {
        private HttpClient httpClient = new HttpClient();
        private YoutubeClient ytClient = new YoutubeClient();
        
        [Command("play", RunMode = RunMode.Async)]
        public async Task playMp3(string url)
        {
            await streamToPCMStream(url, (SocketGuildUser)Context.User);        
        }

        [Command("yt", RunMode = RunMode.Async)]
        public async Task playYoutube(string url)
        {
            string streamUrl = await getYoutubeStreamUrlAsync(url);
            await streamToPCMStream(streamUrl, (SocketGuildUser)Context.User);
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task stopAllStreams()
        {
            SocketGuildUser user = (SocketGuildUser)Context.User;
            SocketVoiceChannel currentVoiceChannel = user.VoiceChannel;
            DiscordSocketClient client = Context.Client;

            if(currentVoiceChannel != null)
            {
                int userCount = 0;
                while(currentVoiceChannel.Users.Count > userCount)
                {
                    SocketGuildUser currentJoinedUser = currentVoiceChannel.Users.ElementAt(userCount);
                    if(currentJoinedUser.IsBot == true && currentJoinedUser.Id == client.CurrentUser.Id)
                    {
                        currentVoiceChannel.DisconnectAsync();
                        break;
                    }
                    userCount++;
                }
            }
            Console.WriteLine("");
        }

        private async Task streamToPCMStream(string url, SocketGuildUser user)
        {
            //GetAllData
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

        private async Task<string> getYoutubeStreamUrlAsync(String url)
        {
            StreamManifest allAvailabeStreams = await ytClient.Videos.Streams.GetManifestAsync(url);
            IEnumerable<AudioOnlyStreamInfo> allAvailableAudioStreams = allAvailabeStreams.GetAudioOnlyStreams();

            int counter = 0;
            double smallestStreamSize = 9999999999999;
            string streamUrl = ""; 

            while(allAvailableAudioStreams.Count() > counter)
            {
                double size = allAvailableAudioStreams.ElementAt(counter).Size.KiloBytes;
                if(size < smallestStreamSize)
                {
                    smallestStreamSize = size;
                    streamUrl = allAvailableAudioStreams.ElementAt(counter).Url;
                }
                counter++;
            }

            return streamUrl;
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

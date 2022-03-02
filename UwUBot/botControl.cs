using Discord;
using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwUBot
{
    internal class botControl
    {
        private string botToken;
        private DiscordSocketClient currentBot = new DiscordSocketClient();
        private SocketGuild mainServer;
        private ulong serverID;

        public botControl(string botToken, ulong serverID)
        {
            this.botToken = botToken;
            this.serverID = serverID;
        }

        public void initBot()
        {

            currentBot.LoginAsync(TokenType.Bot, botToken);
            currentBot.StartAsync();

            while (currentBot.ConnectionState != ConnectionState.Connected)
            {
                Thread.Sleep(100);
            }

            mainServer = currentBot.GetGuild(serverID);

            Console.WriteLine("Bot was started");
        }

        public void stopBot()
        {
            currentBot.LogoutAsync();
            currentBot.StopAsync();
            Console.WriteLine("Bot was stopped");
        }

        private ulong getChannelUlongByString(string channelName)
        {
            int counter = 0;

            List<SocketGuildChannel> allServerChannel = mainServer.Channels.ToList();
            ulong channelID = 0;

            while (allServerChannel.Count > counter)
            {
                SocketGuildChannel currentChannel = allServerChannel[counter];
                SocketTextChannel currentTextChannel = mainServer.GetTextChannel(currentChannel.Id);

                if (currentChannel.Name.Equals(channelName))
                {
                    channelID = currentChannel.Id;
                    break;
                }

                counter++;
            }

            return channelID;

        }

        private SocketTextChannel getTextChannelSocketByUlong(ulong channelID)
        {
            return mainServer.GetTextChannel(channelID);
        }

        private SocketVoiceChannel getVoiceChannelSocketByUlong(ulong channelID)
        {
            return mainServer.GetVoiceChannel(channelID);
        }

        public void sendMessageToChannelByName(string channelName, string message)
        {
            ulong channelID = getChannelUlongByString(channelName);
            SocketTextChannel textChannel = getTextChannelSocketByUlong(channelID);

            textChannel.SendMessageAsync(message);
        }

        private SocketVoiceChannel getVoiceChannelSocketByName(string channelName)
        {
            ulong channelID = getChannelUlongByString(channelName);
            SocketVoiceChannel voiceChannel = getVoiceChannelSocketByUlong(channelID);

            return voiceChannel;
        }

        private Stream createStreamFromAudioFile(string audioFile)
        {
            Process ffmmpegConversion = new Process();
            ffmmpegConversion.StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                ArgumentList =
                {
                    "-hide_banner",
                    "-loglevel",
                    "quiet",
                    "-i",
                    audioFile,
                    "-ac",
                    "2",
                    "-f",
                    "s16le",
                    "-ar",
                    "48000",
                    "pipe:1"
                },
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            ffmmpegConversion.Start();
            return ffmmpegConversion.StandardOutput.BaseStream;
        }

        public async Task playAudioFileInVoiceChannel(string audioFile, string channelName)
        {
            SocketVoiceChannel audioChannel = getVoiceChannelSocketByName(channelName);
            IAudioClient audioInterface = await audioChannel.ConnectAsync();

            while (audioInterface.ConnectionState != ConnectionState.Connected)
            {
                Thread.Sleep(100);
            }

            AudioOutStream audioStream = audioInterface.CreatePCMStream(AudioApplication.Music);
            createStreamFromAudioFile(audioFile).CopyToAsync(audioStream).Wait();

            audioStream.FlushAsync().Wait();
        }
    }
}

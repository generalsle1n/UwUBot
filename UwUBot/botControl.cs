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
        private string commandPrefix = "!";
        private audioStream audioHandler = new audioStream();
        private static string youtubeDomain = "youtube.com";

        public botControl(string botToken, ulong serverID)
        {
            this.botToken = botToken;
            this.serverID = serverID;
        }

        public async Task initBotAsync()
        {
            registerInteralEvents();
            await currentBot.LoginAsync(TokenType.Bot, botToken);
            await currentBot.StartAsync();
            
            while (currentBot.ConnectionState != ConnectionState.Connected)
            {
                Thread.Sleep(100);
            }

            mainServer = currentBot.GetGuild(serverID);
            
            Console.WriteLine("Bot was started");

            await Task.Delay(Timeout.Infinite);
        }

        public async Task stopBotAsync()
        {
            await currentBot.LogoutAsync();
            await currentBot.StopAsync();
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
                
        public async Task playAudioFileInVoiceChannel(string audioFile, ulong channelGuild)
        {
            SocketVoiceChannel audioChannel = getVoiceChannelSocketByUlong(channelGuild);
            IAudioClient audioInterface = await audioChannel.ConnectAsync();
            while (audioInterface.ConnectionState != ConnectionState.Connected)
            {
                Thread.Sleep(100);
            }

            AudioOutStream audioStream = audioInterface.CreatePCMStream(AudioApplication.Music);
            audioHandler.createStreamFromAudioFile(audioFile).CopyToAsync(audioStream).Wait();
            
            audioStream.FlushAsync().Wait();
            audioChannel.DisconnectAsync().Wait();
        }

        private void registerInteralEvents()
        {
            currentBot.MessageReceived += interalCommandHandler;
        }

        private ulong getMessageAuthorCurrentVoiceChannel(SocketMessage message)
        {
            SocketGuildUser messageAuthor = (SocketGuildUser)message.Author;
            return messageAuthor.VoiceChannel.Id;
        }

        private void playYoutubeMusicInChannel(Uri youtubeUrl, SocketMessage message)
        {
            if (youtubeUrl.Host.EndsWith(youtubeDomain))
            {
                string audioPath = audioHandler.downloadMusicFromUrl(youtubeUrl);
                playAudioFileInVoiceChannel(audioPath, getMessageAuthorCurrentVoiceChannel(message)).Wait();
                File.Delete(audioPath);
            }
        }

        private async Task interalCommandHandler(SocketMessage message)
        {
            if (message.Content.StartsWith(commandPrefix))
            {
                string commandText = message.Content.Split("!")[1];
                string[] commandArguments = commandText.Split(" ");
                switch (commandArguments[0])
                {
                    case "play":
                        Uri youtubeUrl = new Uri(commandArguments[1]);
                        playYoutubeMusicInChannel(youtubeUrl, message);
                        break;
                }
            }
        }
    }
}

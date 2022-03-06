using UwUBot;

namespace UwUBot // Note: actual namespace depends on the project name.
{
    internal class main
    {
        public static Task Main(string[] args) => new main().MainAsync();

        public async Task MainAsync()
        {
            string botTokenFileName = "botToken.tkt";
            ulong serverID = 799996272999792650;
            string botToken = System.IO.File.ReadAllText(botTokenFileName);

            botControl discordBot = new botControl(botToken, serverID);
            discordBot.initBotAsync().Wait();

            Path.GetFullPath("sound.wav");

            string voiceChannel = "Allgemein";

            //discordBot.playAudioFileInVoiceChannel("sound.wav", voiceChannel).Wait();
        }
    }
} 
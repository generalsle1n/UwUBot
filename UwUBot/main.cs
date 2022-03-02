using UwUBot;

string botTokenFileName = "botToken.tkt";
ulong serverID = 799996272999792650;
string botToken = System.IO.File.ReadAllText(botTokenFileName);

botControl discordBot = new botControl(botToken, serverID);
discordBot.initBot();

//discordBot.sendMessageToChannelByName("log", "Bot was started");

string voiceChannel = "Allgemein";
var voice = discordBot.connectToVoiceChannelByName(voiceChannel);
//
Thread.Sleep(10000);

discordBot.disconnectFromVoiceChannel(voice);

discordBot.stopBot();
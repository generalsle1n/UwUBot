
# UwUBot

This is an realy simple discord bot that is build with Discord.Net, the bot can play audio from the web and stream the audio to the voicechannel.
There are currently two sources that can be used to stream (An direct url to an audio file, or you can enter an youtube url)


## Roadmap

- Add an help command that respond with all available commands

- Add an user management module for usermanagement (Permission, Ban/Unban etc.)
- Add an module for temporary channels (which only you or your friend can seee) that are created


# Setup

### Prerequisite

You need to setup an discord application in the discord [developer portal](https://discord.com/developers/applications/)

Here you [read](https://discordpy.readthedocs.io/en/stable/discord.html) how to setup an bot in the developer portal


At the moment you just need the follwing permissions:
- "Send Message"
- "Manage Messaged"
- "Connect"
- "Speak"


### Installation
Download and extract the ZIPFile with the binary.
There is an File called "botToken.tkt", you need to open and insert youre discord bot client secret.
You get the secret from the developer portal

    
## Usage

```
cd C:\Folder
UwUBot.exe
```

### Discord Commands
- !play (Url to an Audiofile)
- !yt (Url to an Youtube video)
- !stop (This command stops the current Audio Stream and the bot disconntects from the channel)
## Bugs

If you find and problem/bug just feel free to create an isue on github

## Acknowledgements

 - [Discord.Net](https://github.com/discord-net/Discord.Nets)
 - [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode)
 - [FFMPEG](https://github.com/FFmpeg/FFmpeg)


# UwUBot

This is a really simple discord bot that is built with Discord.Net, the bot can play audio from the web and stream the audio to the voice channel. There are currently two sources that can be used to stream (An direct url to an audio file, or you can enter a youtube url)

## Roadmap

- Add a help command that responds with all available commands

- Add a user management module for user-management (Permission, Ban/Unban etc.)
- Add a module for temporary channels (which only you or your friend can see) that are created


# Setup

### Prerequisite

You need to set up a discord application in the discord [developer portal](https://discord.com/developers/applications/)

Here you [read](https://discordpy.readthedocs.io/en/stable/discord.html) how to set up a bot in the developer portal


At the moment you just need the following permissions:
- "Send Message"
- "Manage Messaged"
- "Connect"
- "Speak"


### Installation
Download and extract the zip file with the binary.
There is a File called "botToken.tkt", you need to open and insert your discord bot client secret.
You get the secret from the developer portal

    
## Usage

```
cd C:\Folder
UwUBot.exe
```

### Discord Commands
- !play (Url to an Audiofile)
- !yt (Url to an Youtube video)
- !stop (This command stops the current Audio Stream and the bot disconnects from the channel)
## Bugs

If you find and problem/bug just feel free to create an issue on Github

## Acknowledgements

 - [Discord.Net](https://github.com/discord-net/Discord.Nets)
 - [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode)
 - [FFMPEG](https://github.com/FFmpeg/FFmpeg)

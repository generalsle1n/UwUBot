using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwUBot.commands
{
    internal class commandHelper
    {
        public Stream getAudioTestFileStream(string path)
        {
            
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "ffmpeg",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
            };

            return Process.Start(startInfo).StandardOutput.BaseStream;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace UwUBot
{
    internal class audioStream
    {
        private YoutubeClient youtbeClient = new YoutubeClient();
        private WebClient httpClient = new WebClient();

        public async Task<Uri> getMediaURLFromYoutubeAsync(Uri youtubeUrl)
        {
            StreamManifest allAvailableStreams = youtbeClient.Videos.Streams.GetManifestAsync(youtubeUrl.ToString()).Result;
            List<AudioOnlyStreamInfo> allAudioStreams = allAvailableStreams.GetAudioOnlyStreams().ToList();

            Uri streamUrl = null;

            int streamCounter = 0;
            while(allAudioStreams.Count > streamCounter)
            {
                if (allAudioStreams[streamCounter].Container.Name.Equals("mp4"))
                {
                    streamUrl = new Uri(allAudioStreams[streamCounter].Url);
                    break;
                }
                streamCounter++;
            }

            return streamUrl;
        }

        public Stream createStreamFromAudioFile(string audioFile)
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

        public string downloadMusicFromUrl(Uri url)
        {
            string tempPath = Path.GetTempFileName();
            httpClient.DownloadFile(url, tempPath);

            return tempPath;
        }
    }
}

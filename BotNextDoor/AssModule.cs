using System;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using Discord;
using Discord.Audio;
using Discord.Commands;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace BotNextDoor
{
    public class AssModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("ass", RunMode = RunMode.Async)]
        [Summary("Join ass")]
        public async Task AssAsync(string ass)
        {
            
            var youtubeClient = new YoutubeClient();
            var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(VideoId.Parse(ass));
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            await using var youtubeStream = await youtubeClient.Videos.Streams.GetAsync(streamInfo);

            await using var pipingStream = new MemoryStream();
            await Cli.Wrap("ffmpeg")
                .WithArguments(" -hide_banner -loglevel panic -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1")
                .WithStandardInputPipe(PipeSource.FromStream(youtubeStream))
                .WithStandardOutputPipe(PipeTarget.ToStream(pipingStream))
                .ExecuteAsync();
                
            var channel = (Context.User as IVoiceState).VoiceChannel; 
            var audioClient = await channel.ConnectAsync();
            await using var audioStream = audioClient.CreatePCMStream(AudioApplication.Mixed);
            try
            {
                await audioStream.WriteAsync(new ReadOnlyMemory<byte>(pipingStream.ToArray()));
            }
            catch
            {
                await Console.Error.WriteLineAsync("im broken");
            }
            
        }
    }
}
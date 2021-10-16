using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace BotNextDoor
{
    public class CockModule : ModuleBase<SocketCommandContext>
    {
        private const string DictionaryFile = "cock_dict.dic";
        
        private const string CockTemplate = "Cock {0} бок";

        private readonly Random rdn = new Random();

        private static List<string> cocks;

        static CockModule()
        {
            cocks = File.ReadAllLines(DictionaryFile).ToList();
        }

        [Command("cock", RunMode = RunMode.Async)]
        [Summary("Cock next bock")]
        public Task Cock()
        {
            string msg = GetRandomCock();
            return ReplyAsync(msg);
        }
        
        [Command("cock", RunMode = RunMode.Async)]
        [Summary("Cock next bock")]
        public Task Cock(int cockNumber)
        {
            var localCocks = Enumerable.Range(0, cockNumber).Select(x => GetRandomCock());
            return ReplyAsync(string.Join(Environment.NewLine, localCocks));
        }

        private string GetRandomCock()
            => string.Format(CockTemplate, cocks[rdn.Next(0, cocks.Count - 1)]);
    }
}
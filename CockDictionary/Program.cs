using System;
using System.IO;
using System.Linq;
using Nestor;
using Nestor.Models;

namespace CockDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = { "index.dic", "ru_gen4.dic", "ru_names4.dic", "ru_phys4.dic",  };
            var words = files.SelectMany(File.ReadAllLines).ToList();
            var morpher = new NestorMorph();
            var cockWords = words
                .Distinct()
                .SelectMany(word => morpher.WordInfo(word).SelectMany(x => x.Forms))
                .Where(wordInfo => wordInfo.Tag is
                {
                    Pos: Pos.Adjective or Pos.Numeral or Pos.Participle,
                    Gender: Gender.Masculine,
                    Case: Case.Nominative,
                    Number: Number.Singular,
                })
                .Select(wordInfo => wordInfo.Word)
                .Distinct()
                .ToList();
            File.WriteAllLines("cock_dict.dic", cockWords);              
        }
    }
}
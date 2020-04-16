using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CsMigemo
{
    public class Migemo
    {
        private readonly CompactDictionary Dictionary;
        private readonly RegexOperator RegexOperator;
        public Migemo(byte[] bytes, RegexOperator regexOperator)
            :this(new MemoryStream(bytes), regexOperator)
        {
        }
        public Migemo(Stream stream, RegexOperator regexOperator)
        {
            Dictionary = new CompactDictionary(stream);
            RegexOperator = regexOperator;
        }

        private static IEnumerable<string> ParseQuery(string query)
        {
            var re = new Regex(@"[^A-Z\s]+|[A-Z]{2,}|([A-Z][^A-Z\s]+)|([A-Z]\s*$)");
            var match = re.Match(query);
            while (match.Success)
            {
                yield return match.Groups[0].Value;
                match = match.NextMatch();
            }
        }

        private string QueryAWord(string word)
        {
            var generator = new RegexGenerator();
            generator.Add(word);
            var lower = word.ToLower();
            foreach (var w in Dictionary.PredictiveSearch(lower))
            {
                generator.Add(w);
            }
            var zen = CharacterConverter.ConvertZen2Han(word);
            generator.Add(zen);
            var han = CharacterConverter.ConvertHan2Zen(word);
            generator.Add(han);

            var hiraganaResult = RomajiProcessor.RomajiToHiraganaPredictively(lower);
            foreach (var a in hiraganaResult.PredictiveSuffixes)
            {
                var hira = hiraganaResult.EstaglishedHiragana + a;
                generator.Add(hira);
                foreach (var b in Dictionary.PredictiveSearch(hira))
                {
                    generator.Add(b);
                }
                var kata = CharacterConverter.ConvertHira2Kata(hira);
                generator.Add(kata);
                generator.Add(CharacterConverter.ConvertZen2Han(kata));
            }
            return generator.Generate(RegexOperator);
        }

        public string Query(string word)
        {
            if (word == "")
            {
                return "";
            }
            var sb = new StringBuilder();
            foreach (var w in ParseQuery(word))
            {
                sb.Append(QueryAWord(w));
            }
            return sb.ToString();
        }
    }
}

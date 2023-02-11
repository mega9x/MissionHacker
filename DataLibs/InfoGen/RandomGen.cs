using System.Security.Cryptography;
using System.Text;
using ChanceNET;
using DataLibs.Data.Movies;
using DataLibs.Data.Movies.Genres;
using Models.Data.MoviesAndTV;
using Syllabore;
using Utils.Randomizer;

namespace Utils.InfoGen
{
    public class RandomGen
    {
        private StringBuilder _stringBuilder = new();
        private NameGenerator GenOne = new NameGenerator()
            .UsingProvider(p => p
                .WithVowels("aeiouy")
                .WithLeadingConsonants("vstlr")
                .WithTrailingConsonants("zrt")
                .WithVowelSequences("ey", "ay", "oy"))
            .UsingTransformer(m => m
                .Select(1).Chance(0.99)
                .WithTransform(x => x.ReplaceSyllable(0, "Gran"))
                .WithTransform(x => x.ReplaceSyllable(0, "Bri"))
                .WithTransform(x => x.InsertSyllable(0, "Deu").AppendSyllable("gard")).Weight(2)
                .WithTransform(x => x.When(-2, "[aeoyAEOY]$").ReplaceSyllable(-1, "opolis"))
                .WithTransform(x => x.When(-2, "[^aeoyAEOY]$").ReplaceSyllable(-1, "polis")))
            .UsingFilter(v => v
                .DoNotAllow("yv", "yt", "zs")
                .DoNotAllowPattern(
                    @".{12,}",
                    @"(\w)\1\1",              // Prevents any letter from occuring three times in a row
                    @".*([y|Y]).*([y|Y]).*",  // Prevents double y
                    @".*([z|Z]).*([z|Z]).*")) // Prevents double z
            .UsingSyllableCount(2, 4);
        private List<Action> actions = new();
        private List<Action> endingAction = new();
        private List<Action> profession = new();
        private List<Action> stringModify = new();
        public RandomGen()
        {
            actions = new List<Action>
            {
                () => {
                    var chance = new Chance();
                    var p = chance.Person();
                    Append(p.FirstName);
                },
                () => {
                    var chance = new Chance();
                    var p = chance.Person();
                    Append(p.LastName);
                },
                () => {
                    Append(GenOne.Next());
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Animal());
                },
            };
            endingAction = new List<Action>
            {
                () => {
                    Append("_");
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Age(AgeRanges.Adult).ToString());
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Age(AgeRanges.Adult).ToString());
                },
                () => {
                    var chance = new Chance();
                    Append(chance.Birthday(AgeRanges.Adult).Year);
                },
            };
            profession = new List<Action>
            {
                () =>
                {
                    var chance = new Chance();
                    Append(chance.Profession());
                },
                () =>
                {
                    var chance = new Chance();
                    Append(chance.Profession(true));
                }
            };

            stringModify = new List<Action>
            {
                () =>
                {
                    _stringBuilder = new(_stringBuilder.ToString().ToLower());
                },
                () =>
                {
                    
                }
            };
        }
        public string GetRandomProfession()
        {
            _stringBuilder.Clear();
            profession[RandomNumberGenerator.GetInt32(0, profession.Count)]();
            stringModify[RandomNumberGenerator.GetInt32(0, stringModify.Count)]();
            return _stringBuilder.ToString();
        }
        public string GetRandomName()
        {
            _stringBuilder.Clear();
            var index = RandomNumberGenerator.GetInt32(1, 2);
            for (var i = 0; i <= index; i++)
            {
                actions[RandomNumberGenerator.GetInt32(0, actions.Count)]();
            }
            endingAction[RandomNumberGenerator.GetInt32(0, endingAction.Count)]();
            endingAction[RandomNumberGenerator.GetInt32(0, endingAction.Count)]();
            var result = _stringBuilder.ToString();
            return result.Replace(" ", "");
        }
        public string GetRandomNameWithoutEnding()
        {
            _stringBuilder.Clear();
            var index = RandomNumberGenerator.GetInt32(1, 2);
            for (var i = 0; i <= index; i++)
            {
                actions[RandomNumberGenerator.GetInt32(0, actions.Count)]();
            }
            var result = _stringBuilder.ToString();
            return result.Replace(" ", "");
        }
        public string GetPassword()
        {
            var chance = new Chance();
            return chance.String(10);
        }
        public string GetFirstName()
        {
            var chance = new Chance();
            return chance.FirstName();
        }
        public string GetLastName()
        {
            var chance = new Chance();
            return chance.LastName();
        }
        private RandomGen Append<T>(T str)
        {
            _stringBuilder.Append(str);
            return this;
        }

    }
}

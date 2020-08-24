using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EmojiCSharp
{
    public class EmojiManager
    {
        public static EmojiTrie EmojiTrie;
        public static List<Emoji> Emojis;

        private static Dictionary<string, Emoji> EmojisByAlias = new Dictionary<string, Emoji>();
        private static Dictionary<string, List<Emoji>> EmojisByTag = new Dictionary<string, List<Emoji>>();

        public static void Init()
        {
            var json = "";
            using (var reader = new StreamReader(typeof(EmojiManager).Assembly.GetManifestResourceStream("EmojiCSharp.emojis.json")))
                json = reader.ReadToEnd();
            Emojis = EmojiLoader.LoadEmojis(json);
            foreach(var emoji in Emojis)
            {
                foreach(var tag in emoji.Tags)
                {
                    if (EmojisByTag.ContainsKey(tag))
                        EmojisByTag[tag].Add(emoji);
                    else EmojisByTag.Add(tag, new List<Emoji>(new[] { emoji }));
                }
                foreach(var alias in emoji.Aliases)
                {
                    if(!EmojisByAlias.ContainsKey(alias))
                        EmojisByAlias.Add(alias, emoji);
                }
            }
            EmojiTrie = new EmojiTrie(Emojis);
            Emojis.Sort((x, y) => y.Unicode.Length - x.Unicode.Length);

        }

        public static Emoji GetForAlias(string alias)
        {
            if (string.IsNullOrEmpty(alias))
                return null;
            Emoji emoji = null;
            EmojisByAlias.TryGetValue(TrimAlias(alias), out emoji);
            return emoji;
        }

        public static Emoji GetByUnicode(string unicode)
        {
            if (string.IsNullOrEmpty(unicode))
                return null;
            var res = EmojiParser.GetEmojiInPosition(unicode.ToCharArray(), 0);
            if (res == null)
                return null;
            return res.Emoji;
        }

        public static bool IsEmoji(string input)
        {
            if (input == null)
                return false;
            var chars = input.ToCharArray();
            var result = EmojiParser.GetEmojiInPosition(chars, 0);
            return result != null && result.StartIndex == 0 && result.LastIndex == chars.Length;
        }

        public static bool ContainsEmoji(string input) =>
            input == null ? false : EmojiParser.GetNextEmoji(input.ToCharArray(), 0) != null;

        public static bool IsOnlyEmojis(string input) =>
            input != null && EmojiParser.RemoveAllEmojis(input) == "";

        public static Matches IsEmoji(char[] sequence) =>
            EmojiTrie.IsEmoji(sequence);

        public static IEnumerable<string> GetAllTags() =>
            EmojisByTag.Keys;

        private static string TrimAlias(string alias) =>
            alias.Substring(alias[0] == ':' ? 1 : 0,
                alias[alias.Length - 1] == ':' ? alias.Length - 2 : alias.Length - 1);

    }
}

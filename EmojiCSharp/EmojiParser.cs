using EmojiCSharp.EmojiTransformers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace EmojiCSharp
{
    public class EmojiParser
    {
        public static string ParseToUnicode(string input)
        {
            StringBuilder builder = new StringBuilder();

            for(int last = 0; last < input.Length; last++)
            {
                var alias = GetAliasAt(input, last);
                if (alias == null)
                    alias = GetHtmlEncodedEmojiAt(input, last);
                if (alias != null)
                {
                    builder.Append(alias.Emoji.Unicode);
                    last = alias.LastIndex;
                    if (alias.Fitzpatrick != null)
                        builder.Append(alias.Fitzpatrick);
                }
                else builder.Append(input[last]);
            }
            return builder.ToString();
        }

        protected static AliasCandidate GetAliasAt(string input, int startIndex)
        {
            if (input.Length < startIndex + 2 || input[startIndex] != ':')
                return null;
            var aliasLastIndex = input.IndexOf(':', startIndex + 2);
            if (aliasLastIndex == -1)
                return null;
            var fitzpatrickStart = input.IndexOf('|', startIndex + 2);
            Emoji emoji = null;
            if(fitzpatrickStart != -1 && fitzpatrickStart < aliasLastIndex)
            {
                emoji = EmojiManager.GetForAlias(input.Substring(startIndex, fitzpatrickStart - startIndex));
                if (emoji == null)
                    return null;
                if (!emoji.SupporrtsFitzpartick)
                    return null;
                var fitzpatrick = Fitzpatrick.FitzpatrickFromType(input.Substring(fitzpatrickStart + 1, aliasLastIndex - fitzpatrickStart - 1));
                return new AliasCandidate(emoji, fitzpatrick, startIndex, aliasLastIndex);
            }
            emoji = EmojiManager.GetForAlias(input.Substring(startIndex, aliasLastIndex));
            if (emoji == null)
                return null;
            return new AliasCandidate(emoji, null, startIndex, aliasLastIndex);
        }

        protected static AliasCandidate GetHtmlEncodedEmojiAt(string input, int startIndex)
        {
            if (input.Length < startIndex + 4 || input[startIndex] != '&' || input[startIndex + 1] != '#')
                return null;
            Emoji longestEmoji = null;
            var longestCodePointEnd = -1;
            char[] chars = new char[EmojiManager.EmojiTrie.MaxDepth];
            var charsIndex = 0;
            var codePointStart = startIndex;

            do
            {
                var codePointEnd = input.IndexOf(';', codePointStart + 3);
                if (codePointEnd == -1)
                    break;

                try
                {
                    var radix = input[codePointStart + 2] == 'x' ? 16 : 10;
                    int codePoint = int.Parse(input.Substring(codePointStart + 2 + (radix / 16), codePointEnd), radix == 10 ? NumberStyles.Integer : NumberStyles.HexNumber);
                    var code = char.ConvertFromUtf32(codePoint);
                    if (code.Length == 1)
                        chars[charsIndex] = code[0];
                    else if (code.Length == 2)
                    {
                        chars[charsIndex] = code[0];
                        chars[charsIndex + 1] = code[1];
                    }
                }
                catch (ArgumentException)
                {
                    break;
                }

                var foundEmoji = EmojiManager.EmojiTrie.GetEmoji(chars, 0, charsIndex);
                if(foundEmoji != null)
                {
                    longestEmoji = foundEmoji;
                    longestCodePointEnd = codePointEnd;
                }
                codePointStart = codePointEnd + 1;

            } while (input.Length > codePointStart + 4 && input[codePointStart] == '&' &&
                    input[codePointStart + 1] == '#' && charsIndex < chars.Length &&
                    !(EmojiManager.EmojiTrie.IsEmoji(chars, 0, charsIndex) == Matches.Imposible));
            if (longestEmoji == null)
                return null;
            return new AliasCandidate(longestEmoji, null, startIndex, longestCodePointEnd);
        }

        public static string ParseToAliases(string input, FitzpatrickAction action = FitzpatrickAction.Parse) =>
            ParseFromUnicode(input, new AliasesEmojiTransformer() { Action = action });

        public static string ReplaceAllEmojis(string input, string replacement) =>
            ParseFromUnicode(input, new ReplaceEmojiTransform() { Replace = replacement });

        public static string ParseToHtmlDecimal(string input, FitzpatrickAction action = FitzpatrickAction.Parse) =>
            ParseFromUnicode(input, new HTMLDecimalEmojiTransformer() { Action = action });

        public static string ParseToHtmlHexadecimal(string input, FitzpatrickAction action = FitzpatrickAction.Parse) =>
            ParseFromUnicode(input, new HTMLHexEmojiTransformer() { Action = action });

        public static string RemoveAllEmojis(string input) =>
            ParseFromUnicode(input, new RemoveAllEmojiTransformer());

        public static string RemoveEmojis(string input, IEnumerable<Emoji> emojisToRemove) =>
            ParseFromUnicode(input, new RemoveEmojiTransformer() { EmojisToRemove = emojisToRemove });

        public static string RemoveAllEmojisExcept(string input, IEnumerable<Emoji> emojisToKeep) =>
            ParseFromUnicode(input, new RemoveAllExceptEmojiTransformer() { EmojisToKeep = emojisToKeep });

        public static string ParseFromUnicode(string input, IEmojiTransformer transformer)
        {
            var prev = 0;
            StringBuilder builder = new StringBuilder();
            List<EmojiResult> replacements = GetEmojis(input);
            foreach(var candidate in replacements)
            {
                builder.Append(input, prev, candidate.StartIndex);
                builder.Append(transformer.Transform(candidate));
                prev = candidate.LastIndex;
            }
            return builder.Append(input.Substring(prev)).ToString();
        }

        public static List<string> ExtractEmojiStrings(string input, int limit = 0) =>
             ExtractEmojis(input, limit).Select(x => x.ToString()).ToList();


        public static List<EmojiResult> ExtractEmojis(string input, int limit = 0)
        {
            return GetEmojis(input, limit);
        }

        public static List<EmojiResult> GetEmojis(string input, int limit = 0)
        {
            char[] inputCharArray = input.ToCharArray();
            var candidates = new List<EmojiResult>();
            EmojiResult next;
            for(int i =  0; (next = GetNextEmoji(inputCharArray, i)) != null; i = next.LastIndex)
            {
                candidates.Add(next);
                if(limit != 0)
                {
                    limit--;
                    if (limit <= 0)
                        break;
                }
            }
            return candidates;
        }

        public static EmojiResult GetNextEmoji(char[] chars, int startIndex)
        {
            for(int i = startIndex; i < chars.Length; i++)
            {
                var emoji = GetEmojiInPosition(chars, i);
                if (emoji != null)
                    return emoji;
            }
            return null;
        }

        public static EmojiResult GetEmojiInPosition(char[] chars, int startIndex)
        {
            var emoji = GetBestBaseEmoji(chars, startIndex);
            if (emoji == null)
                return null;
            string fitzpatrick = null;
            GenderType gender = GenderType.None;
            var lastIndex = startIndex + emoji.Unicode.Length;
            var sequenceType = emoji.Sequence;
            if (sequenceType == Sequence.BaseSkinGender)
            {
                fitzpatrick = Fitzpatrick.Find(chars, lastIndex);
                if (fitzpatrick != null)
                    lastIndex += 2;
                var genderMatch = FindGender(chars, lastIndex);
                if (genderMatch != null)
                {
                    lastIndex = genderMatch.LastIndex;
                    gender = genderMatch.Gender;
                }
            }
            else if (sequenceType == Sequence.GenderSkinBase)
            {
                var genderType = FindGender(emoji.Unicode);
                if (genderType != GenderType.None)
                    gender = genderType;
                fitzpatrick = Fitzpatrick.Find(chars, lastIndex);
                if (fitzpatrick != null)
                    lastIndex += 2;
                var baseEmoji = TryGetBestBaseEmoji(chars, lastIndex);
                if (baseEmoji != null)
                {
                    lastIndex += baseEmoji.Unicode.Length + 1;
                    emoji = baseEmoji;
                }
            }
            if (chars.Length > lastIndex)
            {
                var ch = chars[lastIndex];
                if (ch == '\uFE0F')
                    lastIndex++;
            }
            return new EmojiResult(emoji, fitzpatrick, gender, chars, startIndex, lastIndex);
        }

        public static Emoji GetBestBaseEmoji(char[] text, int startIndex) =>
            EmojiManager.EmojiTrie.GetBestEmoji(text, startIndex);

        private static Emoji TryGetBestBaseEmoji(char[] chars, int startIndex)
        {
            var length = chars.Length;
            if (startIndex >= length)
                return null;
            if (chars[startIndex] != '\u200D')
                return null;
            startIndex++;
            return GetBestBaseEmoji(chars, startIndex);
        }

        private static GenderMatch FindGender(char[] chars, int startIndex)
        {
            var length = chars.Length;
            if (length <= startIndex)
                return null;
            var pos = startIndex;
            var ch = chars[pos];
            if (ch != '\u200D')
                return null;
            pos++;
            var gender = Gender.Find(chars, pos);
            if (gender == GenderType.None)
                return null;
            return new GenderMatch(gender, pos + 1);
        }

        private static GenderType FindGender(string emoji)
        {
            return Gender.Find(emoji);
        }

    }
}

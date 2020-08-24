using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace EmojiCSharp
{
    public enum Sequence
    {
        None,
        BaseSkinGender,
        GenderSkinBase
    }

    public class Emoji
    {
        public string Description { get; }
        public bool SupporrtsFitzpartick { get; }
        public Sequence Sequence { get; }
        public List<string> Aliases { get; }
        public List<string> Tags { get; }
        public string Unicode { get; }
        public string HtmlDecimal { get; }
        public string HtmlHexadecimal { get; }

        public Emoji(string desctription, Sequence sequence, List<string> aliases, List<string> tags, params byte[] bytes)
        {
            Description = desctription;
            Sequence = sequence;
            SupporrtsFitzpartick = sequence != Sequence.None;
            Aliases = aliases;
            Tags = tags;
            Unicode = Encoding.UTF8.GetString(bytes);
            for(int i = 0; i < Unicode.Length;)
            {
                var codePoint = char.ConvertToUtf32(Unicode, i);
                
                HtmlDecimal += $"&#{codePoint};";
                HtmlHexadecimal += $"&#x{codePoint.ToString("x")};";

                i += char.ConvertFromUtf32(codePoint).Length;
            }
        }

        public string GetUnicode(string fitzpatrick)
        {
            if (Fitzpatrick.FitzpatrickFromUnicode(fitzpatrick) == null)
                throw new ArgumentException("Invilid fitzpartick", "fitzpatrick");
            if (!SupporrtsFitzpartick)
                throw new InvalidOperationException("Emoji doesn't support fitzpatrick");
            return Unicode + fitzpatrick;
        }

        public override string ToString()
        {
            return "Emoji {\n" +
                $"description='{Description}',\n" +
                $"supportsFitzpatrick={SupporrtsFitzpartick},\n" +
                $"aliases=[{(Aliases.Count > 0 ? Aliases.Aggregate((x, y) => $"{x}, {y}") : "null")}],\n" +
                $"tags=[{(Tags.Count > 0 ? Tags.Aggregate((x, y) => $"{x}, {y}") : "null")}],\n" +
                $"unicode='{Unicode}',\n" +
                $"htmlDec='{HtmlDecimal}',\n" +
                $"htmlHex='{HtmlHexadecimal}'\n" +
                "}";
        }

    }
}

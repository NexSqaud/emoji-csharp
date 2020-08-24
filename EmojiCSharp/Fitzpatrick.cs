using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmojiCSharp
{

    public enum FitzpatrickAction 
    {
        Parse,
        Remove,
        Ignore
    }

    public class Fitzpatrick
    {
        // Костыль. Ибо в C# нельзя создавать enum с типом string. ¯\_(ツ)_/¯
        // Crutch. Because in c # you cannot create enum with string type. ¯\_(ツ)_/¯
        public const string Type12 = "\uD83C\uDFFB";
        public const string Type3 = "\uD83C\uDFFC";
        public const string Type4 = "\uD83C\uDFFD";
        public const string Type5 = "\uD83C\uDFFE";
        public const string Type6 = "\uD83C\uDFFF";

        public string Unicode;

        public Fitzpatrick(string unicode) => Unicode = unicode;
            

        public static string FitzpatrickFromUnicode(string unicode)
        {
            switch (unicode)
            {
                case Type12:
                    return Type12;
                case Type3:
                    return Type3;
                case Type4:
                    return Type4;
                case Type5:
                    return Type5;
                case Type6:
                    return Type6;
                default:
                    return null;
            }
        }

        public static string FitzpatrickFromType(string type)
        {
            switch (type.ToLower().Replace("_", ""))
            {
                case "type12":
                    return Type12;
                case "type3":
                    return Type3;
                case "type4":
                    return Type4;
                case "type5":
                    return Type5;
                case "type6":
                    return Type6;
                default:
                    return null;
            }
        }

        public static string Find(char[] chars, int startIndex)
        {
            if (chars.Length < startIndex + 1)
                return null;
            var ch = chars[startIndex];
            if (ch != '\uD83C')
                return null;
            ch = chars[startIndex + 1];
            switch (ch)
            {
                case '\uDFFB':
                    return Type12;
                case '\uDFFC':
                    return Type3;
                case '\uDFFD':
                    return Type4;
                case '\uDFFE':
                    return Type5;
                case '\uDFFF':
                    return Type6;
            }
            return null;
        }

    }
}

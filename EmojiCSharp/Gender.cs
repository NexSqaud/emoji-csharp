using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp
{
    public enum GenderType 
    {
        None,
        Male,
        Female,
        Person
    }

    public class Gender
    {
        public static GenderType Find(char[] chars, int startIndex)
        {
            if (startIndex >= chars.Length)
                return GenderType.None;
            var ch = chars[startIndex];
            switch (ch)
            {
                case '♂':
                    return GenderType.Male;
                case '♀':
                    return GenderType.Female;
                default:
                    return GenderType.None;
            }
        }

        public static GenderType Find(string emoji)
        {
            return FindByUnicode(emoji.ToCharArray(), 0);
        }

        public static GenderType FindByUnicode(char[] chars, int startIndex)
        {
            if (startIndex + 2 > chars.Length)
                return GenderType.None;
            var ch = chars[startIndex];
            switch (ch)
            {
                case '\uD83D':
                    switch (chars[startIndex + 1])
                    {
                        case '\uDC68':
                            return GenderType.Male;
                        case '\uDC69':
                            return GenderType.Female;
                        default:
                            return GenderType.None;
                    }
                case '\uD83E':
                    switch (chars[startIndex + 1])
                    {
                        case '\uDDD1':
                            return GenderType.Person;
                        default:
                            return GenderType.None;
                    }
                default:
                    return GenderType.None;
            }
        }

    }
}

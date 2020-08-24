using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp
{
    public class EmojiResult
    {
        public Emoji Emoji { get; }
        public string Fitzpatrick { get; }
        public GenderType Gender { get; }
        public char[] Source { get; }
        public int StartIndex { get; }
        public int LastIndex { get; }

        private string sub = null;

        public EmojiResult(Emoji emoji, string fitzpatrick, GenderType gender, char[] source, int startIndex, int lastIndex)
        {
            Emoji = emoji;
            Fitzpatrick = fitzpatrick;
            Gender = gender;
            Source = source;
            StartIndex = startIndex;
            LastIndex = lastIndex;
        }

        public override string ToString()
        {
            if (sub != null)
                return sub;
            var len = LastIndex - StartIndex;
            char[] vs = new char[len];
            Array.ConstrainedCopy(Source, StartIndex, vs, 0, len);
            sub = new string(vs);
            return sub;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp
{
    public class AliasCandidate
    {
        public Emoji Emoji { get; }
        public string Fitzpatrick { get; }
        public int StartIndex { get; }
        public int LastIndex { get; }
        
        public AliasCandidate(Emoji emoji, string fitzpatrick, int startIndex, int lastIndex)
        {
            Emoji = emoji;
            Fitzpatrick = fitzpatrick;
            StartIndex = startIndex;
            LastIndex = lastIndex;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmojiCSharp.EmojiTransformers
{
    class RemoveEmojiTransformer : IEmojiTransformer
    {
        public IEnumerable<Emoji> EmojisToRemove;

        public string Transform(EmojiResult unicodeCandidate)
        {
            if (EmojisToRemove.Contains(unicodeCandidate.Emoji))
                return "";
            else return unicodeCandidate.Emoji.Unicode + unicodeCandidate.Fitzpatrick;
        }
    }
}

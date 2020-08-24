using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmojiCSharp.EmojiTransformers
{
    class RemoveAllExceptEmojiTransformer : IEmojiTransformer
    {
        public IEnumerable<Emoji> EmojisToKeep;

        public string Transform(EmojiResult unicodeCandidate)
        {
            if (EmojisToKeep.Contains(unicodeCandidate.Emoji))
                return unicodeCandidate.Emoji.Unicode + unicodeCandidate.Fitzpatrick;
            else return "";
        }
    }
}

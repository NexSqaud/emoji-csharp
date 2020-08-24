using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp.EmojiTransformers
{
    class RemoveAllEmojiTransformer : IEmojiTransformer
    {
        public string Transform(EmojiResult unicodeCandidate)
        {
            return "";
        }
    }
}

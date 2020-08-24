using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EmojiCSharp.EmojiTransformers
{
    class ReplaceEmojiTransform : IEmojiTransformer
    {
        public string Replace;

        public string Transform(EmojiResult unicodeCandidate) => Replace;
    }
}

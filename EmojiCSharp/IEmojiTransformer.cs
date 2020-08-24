using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp
{
    public interface IEmojiTransformer
    {
        string Transform(EmojiResult unicodeCandidate);
    }
}

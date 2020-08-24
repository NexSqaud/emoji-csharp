using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp.EmojiTransformers
{
    class HTMLHexEmojiTransformer : IEmojiTransformer
    {
        public FitzpatrickAction Action;

        public string Transform(EmojiResult unicodeCandidate)
        {
            switch (Action)
            {
                default:
                case FitzpatrickAction.Parse:
                case FitzpatrickAction.Remove:
                    return unicodeCandidate.Emoji.HtmlHexadecimal;
                case FitzpatrickAction.Ignore:
                    return unicodeCandidate.Emoji.HtmlHexadecimal + unicodeCandidate.Fitzpatrick;
            }
        }
    }
}

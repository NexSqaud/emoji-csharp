using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp.EmojiTransformers
{
    class HTMLDecimalEmojiTransformer : IEmojiTransformer
    {
        public FitzpatrickAction Action;

        public string Transform(EmojiResult unicodeCandidate)
        {
            switch (Action) 
            {
                default:
                case FitzpatrickAction.Parse:
                case FitzpatrickAction.Remove:
                    return unicodeCandidate.Emoji.HtmlDecimal;
                case FitzpatrickAction.Ignore:
                    return unicodeCandidate.Emoji.HtmlDecimal + unicodeCandidate.Fitzpatrick;
            }
        }
    }
}

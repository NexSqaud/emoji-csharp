using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp.EmojiTransformers
{
    class AliasesEmojiTransformer : IEmojiTransformer
    {

        public FitzpatrickAction Action;

        public string Transform(EmojiResult unicodeCandidate)
        {
            switch (Action) 
            {
                default:
                case FitzpatrickAction.Parse:
                    if(unicodeCandidate.Fitzpatrick != null)
                        return $":{unicodeCandidate.Emoji.Aliases[0]}|{unicodeCandidate.Fitzpatrick}:";
                    else return $":{unicodeCandidate.Emoji.Aliases[0]}:";
                case FitzpatrickAction.Remove:
                    return $":{unicodeCandidate.Emoji.Aliases[0]}:";
                case FitzpatrickAction.Ignore:
                    return $":{unicodeCandidate.Emoji.Aliases[0]}:{unicodeCandidate.Fitzpatrick}";
            }
        }
    }
}

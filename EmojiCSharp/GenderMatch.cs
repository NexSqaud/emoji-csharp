using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp
{
    class GenderMatch
    {
        public GenderType Gender;
        public int LastIndex;

        public GenderMatch(GenderType gender, int lastIndex)
        {
            Gender = gender;
            LastIndex = lastIndex;
        }
    }
}

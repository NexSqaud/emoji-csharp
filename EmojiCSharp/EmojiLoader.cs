using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmojiCSharp
{
    class EmojiLoader
    {
        public static List<Emoji> LoadEmojis(string emojis)
        {
            JArray emojisJson = JArray.Parse(emojis);
            List<Emoji> emojisList = new List<Emoji>();
            for(int i = 0; i < emojisJson.Count; i++)
            {
                var emoji = BuildEmojiFromJson(emojisJson[i].ToObject<JObject>());
                if (emoji != null)
                    emojisList.Add(emoji);
            }
            return emojisList;
        }

        protected static Emoji BuildEmojiFromJson(JObject jObject)
        {
            if (!jObject.ContainsKey("emoji"))
                return null;
            var unicode = jObject["emoji"].ToString();
            if (unicode.StartsWith("\uD83D\uDC69\u200D") || unicode.StartsWith("\uD83D\uDC68\u200D"))
                return null;
            var bytes = Encoding.UTF8.GetBytes(unicode);
            string description = null;
            if (jObject.ContainsKey("description"))
                description = jObject["description"].ToString();
            int sequence = 0;
            if (jObject.ContainsKey("sequence_type"))
                sequence = jObject["sequence_type"].ToObject<int>();
            List<string> aliases = jObject["aliases"].ToObject<List<string>>();
            List<string> tags = jObject["tags"].ToObject<List<string>>();
            return new Emoji(description, (Sequence)sequence, aliases, tags, bytes);
        }

    }
}

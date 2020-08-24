using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EmojiCSharp
{
    public enum Matches
    {
        Exactly,
        Possibly,
        Imposible
    }

    public class EmojiTrie
    {
        Node root = new Node();
        public int MaxDepth;

        public EmojiTrie(IEnumerable<Emoji> emojis)
        {
            var maxDepth = 0;
            foreach(var emoji in emojis)
            {
                var tree = root;
                var chars = emoji.Unicode.ToCharArray();
                maxDepth = Math.Max(maxDepth, chars.Length);
                foreach(var c in chars)
                {
                    if(!tree.Children.ContainsKey(c))
                        tree.Children.Add(c, new Node());
                    tree = tree.Children[c];
                }
                tree.Emoji = emoji;
            }
            MaxDepth = maxDepth;
        }

        public Matches IsEmoji(char[] sequence) =>
            IsEmoji(sequence, 0, sequence.Length);

        public Matches IsEmoji(char[] sequence, int startIndex, int lastIndex)
        {
            if (startIndex < 0 || startIndex > lastIndex || lastIndex > sequence.Length)
                throw new IndexOutOfRangeException($"startIndex {startIndex}, lastIndex {lastIndex}, length {sequence.Length}");
            var tree = root;
            for(int i = startIndex; i < lastIndex; i++)
            {
                if (!tree.Children.ContainsKey(sequence[i]))
                    return Matches.Imposible;
                tree = tree.Children[sequence[i]];
            }
            return tree.Emoji != null ? Matches.Exactly : Matches.Possibly;
        }

        public Emoji GetBestEmoji(char[] sequence, int startIndex)
        {
            if (startIndex < 0)
                throw new IndexOutOfRangeException($"startIndex {startIndex}, length {sequence.Length}");
            var tree = root;
            for(int i = startIndex; i < sequence.Length; i++)
            {
                if (!tree.Children.ContainsKey(sequence[i]))
                    return tree.Emoji;
                tree = tree.Children[sequence[i]];
            }
            return tree.Emoji;
        }

        public Emoji GetEmoji(string unicode) =>
            GetEmoji(unicode.ToCharArray(), 0, unicode.Length);

        public Emoji GetEmoji(char[] sequence, int startIndex, int lastIndex)
        {
            if (startIndex < 0 || startIndex > lastIndex || lastIndex > sequence.Length)
                throw new IndexOutOfRangeException($"startIndex {startIndex}, lastIndex {lastIndex}, length {sequence.Length}");
            var tree = root;
            for(int i = startIndex; i < lastIndex; i++)
            {
                if (!tree.Children.ContainsKey(sequence[i]))
                    return null;
                tree = tree.Children[sequence[i]];
            }
            return tree.Emoji;
        }

    }

    class Node
    {
        public Dictionary<char, Node> Children = new Dictionary<char, Node>();
        public Emoji Emoji;
    }

}

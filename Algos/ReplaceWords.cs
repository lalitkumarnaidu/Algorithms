using System;
using System.Collections.Generic;
using System.Text;

namespace Algos.ReplaceWords
{
    public class ReplaceWords
    {
        public string Replace(IList<string> dictionary, string sentence)
        {
            var root = new TrieNode();
            for (int i = 0; i < dictionary.Count; i++)
            {
                root.Insert(dictionary[i]);
            }

            var senArr = sentence.Split(" ");
            for (int i = 0; i < senArr.Length; i++)
            {
                var curRoot = root.PrefixOf(senArr[i]);
                if (curRoot != "")
                {
                    senArr[i] = curRoot;
                }
            }

            return string.Join(" ", senArr);
        }
    }

    public class TrieNode
    {
        public bool IsWord;
        public Dictionary<char, TrieNode> Children;
        public TrieNode()
        {
            IsWord = false;
            Children = new Dictionary<char, TrieNode>();
        }

        public void Insert(string word)
        {
            int i = 0;
            var curNode = this;
            while (i < word.Length)
            {
                curNode.Children.TryAdd(word[i], new TrieNode());
                curNode = curNode.Children[word[i]];
                i++;
            }
            curNode.IsWord = true;
        }

        public string PrefixOf(string word)
        {
            var curNode = this;
            int i = 0;
            StringBuilder sb = new StringBuilder();
            while (i < word.Length && curNode.Children.ContainsKey(word[i]) && !curNode.IsWord)
            {
                sb.Append(word[i]);
                curNode = curNode.Children[word[i]];
                i++;
            }

            return curNode.IsWord ? sb.ToString() : "";
        }
    }
}

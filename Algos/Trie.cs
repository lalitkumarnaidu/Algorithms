using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetSolutions.Algos
{
    public static partial class Solution {
        public class Trie
        {

            /** Initialize your data structure here. */
            public Dictionary<char, Trie> children;
            public bool isaWordEnd;
            public Trie()
            {
                children = new Dictionary<char, Trie>();
                isaWordEnd = false;
            }

            public void Insert(IList<string> words)
            {
                foreach (var w in words) Insert(w);
            }
            /** Inserts a word into the trie. */
            public void Insert(string word)
            {
                int i = 0;
                var node = this;
                while (i < word.Length)
                {
                    if (!node.children.ContainsKey(word[i]))
                    {
                        node.children.Add(word[i], new Trie());
                    }
                    node = node.children[word[i]];
                    i++;
                }
                node.isaWordEnd = true;
            }

            public int Search(string word, int sidx)
            {
                var curNode = this;
                int i = sidx;
                while (i < word.Length)
                {
                    if (!curNode.children.ContainsKey(word[i]))
                        return -1;
                    curNode = curNode.children[word[i]];
                    if (curNode.isaWordEnd) return i + 1;
                    i++;
                }

                return curNode.isaWordEnd ? i - 1 : -1;
            }

            /** Returns if the word is in the trie. */
            public bool Search(string word)
            {
                int i = 0;
                var node = this;
                while (i < word.Length)
                {
                    if (!node.children.ContainsKey(word[i]))
                    {
                        return false;
                    }
                    node = node.children[word[i]];
                    i++;
                }
                return node.isaWordEnd;
            }

            /** Returns if there is any word in the trie that starts with the given prefix. */
            public bool StartsWith(string prefix)
            {
                int i = 0;
                var node = this;
                while (i < prefix.Length)
                {
                    if (!node.children.ContainsKey(prefix[i]))
                    {
                        return false;
                    }
                    node = node.children[prefix[i]];
                    i++;
                }
                return true;
            }

            /*Word Break*/
            public bool WordBreak(string s, IList<string> wordDict)
            {
                var wordDictSet = wordDict.ToHashSet();
                string word = "";
                for (int i = 0; i < s.Length; i++)
                {
                    word += s[i].ToString();
                    if (wordDictSet.Contains(word))
                    {
                        word = "";
                    }
                }
                return word.Length == 0;
            }
        }
    }

}

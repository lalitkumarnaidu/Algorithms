using System;
using System.Collections.Generic;

namespace LeetSolutions.Algos.MapSum
{
    /*
     * Map Sum Pairs
     Implement a MapSum class with insert, and sum methods.

        For the method insert, you'll be given a pair of (string, integer).
        The string represents the key and the integer represents the value.
        If the key already existed, then the original key-value pair will be overridden to the new one.

        For the method sum, you'll be given a string representing the prefix,
        and you need to return the sum of all the pairs' value whose key starts with the prefix.

        Example 1:
        Input: insert("apple", 3), Output: Null
        Input: sum("ap"), Output: 3
        Input: insert("app", 2), Output: Null
        Input: sum("ap"), Output: 5
     */
    public class MapSum
    {

        /** Initialize your data structure here. */
        private Dictionary<string, int> _keyValues;
        private TrieNode _keys;
        public MapSum()
        {
            _keys = new TrieNode();
            _keyValues = new Dictionary<string, int>();
        }

        public void Insert(string key, int val)
        {
            _keyValues.TryGetValue(key, out int d);
            int curVal = val - d;
            _keyValues[key] = curVal;
            int i = 0;
            var curNode = _keys;
            curNode.Value += d;
            while (i < key.Length)
            {
                if (!curNode.Children.ContainsKey(key[i]))
                {
                    curNode.Children.Add(key[i], new TrieNode());

                }
                curNode = curNode.Children[key[i]];
                curNode.Value += curVal;
                i++;
            }
        }

        public int Sum(string prefix)
        {
            int i = 0;
            var cur = _keys;
            while (i < prefix.Length)
            {
                if (!cur.Children.ContainsKey(prefix[i]))
                {
                    return 0;
                }
                cur = cur.Children[prefix[i]];
                i++;
            }
            return cur.Value;
        }
    }

    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children;
        public int Value;
        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
            Value = 0;
        }
    }
}




using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AutoComplete
{
    class Trie
    {
        private TrieNode root;
        private List<string> wordSuggestions;
        private List<Query> querySuggestions;

        public Trie()
        {
            root = new TrieNode() { value = ' ' };
        }
        public List<string> getWordSuggestions()
        {
            return wordSuggestions;
        }
        public List<Query> getQuerySuggestions()
        {
            if (querySuggestions != null)
                return querySuggestions;
            return null;
        }
        public void InsertWords(string s)
        {
            TrieNode tmp = null;
            TrieNode current = root;
            foreach (char x in s)
            {
                if (current.nodes == null)
                    current.nodes = new Dictionary<int, TrieNode>();
                if (!current.nodes.ContainsKey(x))
                {
                    tmp = new TrieNode() { value = x };
                    current.nodes.Add(x, tmp);
                }
                current = current.nodes[x];
            }
            current.finalNode = true;
        }
        public void InsertQueries(int weight, string s)
        {
            TrieNode current = root;
            int j = 0;
            foreach (char x in s)
            {
                Query q = new Query(s, weight);
                if (x == ' ')
                {
                    current = root;
                    j++;
                    continue;
                }
                if (current.nodes == null)
                    current.nodes = new Dictionary<int, TrieNode>();
                if (!current.nodes.ContainsKey(x))
                {
                    TrieNode tmp = new TrieNode() { value = x };
                    current.nodes.Add(x, tmp);
                }
                current = current.nodes[x];
                if (current.queries == null)
                    current.queries = new List<Query>();
                if (j > 0)
                    q.index = j;
                current.queries.Add(q);
            }
        }
        private TrieNode find(string s)
        {
            if (s == string.Empty || s == null)
                return null;
            TrieNode current = root;
            foreach (char x in s)
            {
                if (x == ' ')
                {
                    current = root;
                    continue;
                }
                if (current.nodes == null)
                    return null;
                if (current.nodes.ContainsKey(x))
                    current = current.nodes[x];
                else
                    return null;
            }
            return current;
        }
        public void suggestQueries(string s)
        {
            TrieNode current = new TrieNode();
            if (querySuggestions != null)
                if (querySuggestions.Count > 0)
                    querySuggestions.Clear();
            current = find(s);
            if (current == root || current == null)
                return;
            if (current.queries == null)
                return;
            querySuggestions = new List<Query>(current.queries);
        }
        public void suggestWords(TrieNode current, string s)
        {
            if (current == null)
                return;
            if (current.nodes == null || current == root)
                return;
            foreach (TrieNode node in current.nodes.Values)
            {
                string tmp = s + node.value;
                if (node.finalNode)
                {
                    if (wordSuggestions.Count > 5)
                        return;
                    else
                        wordSuggestions.Add(tmp);
                }
                suggestWords(node, tmp);
            }
        }
    }
}
using System.Collections.Generic;

namespace AutoComplete
{
    class TrieNode
    {
        public char value { set; get; }
        public List<Query> queries { set; get; }
        public Dictionary<int, TrieNode> nodes { set; get; }
        public bool finalNode { set; get; }
    }
}
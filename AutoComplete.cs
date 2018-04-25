using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoComplete
{
    class AutoComplete
    {
        private static Trie t = new Trie();
        private static List<string> myDictionary = new List<string>();
        private static List<Query> Suggestions;
        public static void ImportWords()
        {
            StreamReader sr = new StreamReader("test.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                t.InsertWords(line.ToLower());
            }
        }
        public static void ImportQueries()
        {
            StreamReader sr = new StreamReader("Queries.txt");
            string line, s;
            string[] words;
            int w;
            while ((line = sr.ReadLine()) != null)
            {
                words = line.Split(',');
                w = int.Parse(words[0]);
                s = words[1].ToLower();
                foreach (string ss in words[1].Split(' '))
                {
                    if (!myDictionary.Contains(ss))
                        myDictionary.Add(ss.ToLower());
                }
                t.InsertQueries(w, s);
            }
        }
        public static List<Query> getSuggestions(string userInput)
        {
            if (userInput == null)
                return null;
            if (userInput != string.Empty)
            {
                t.suggestQueries(userInput);
                if (t.getQuerySuggestions() != null)
                {
                    if (t.getQuerySuggestions().Count > 0)
                    {
                        Suggestions = new List<Query>(t.getQuerySuggestions());
                        return Suggestions;
                    }
                    else
                    {
                        return getSuggestions(Rectify(userInput));
                    }
                }
            }
            return null;
        }
        private static string Rectify(string s)
        {
            if (s == null)
                return null;
            if (s.Length < 3)
                return null;
            string possible = null;
            int dist;
            int min = 100;
            foreach (string x in myDictionary)
            {
                if (Enumerable.Range(s.Length - 2, s.Length + 2).Contains(x.Length))
                {
                    dist = editDistance(s, x);
                    if (dist <= (s.Length % 2 == 0 ? s.Length / 2 : s.Length / 2 + 1))
                    {
                        if (dist < min)
                        {
                            min = dist;
                            possible = x;
                            if (min == 1)
                                break;
                        }
                    }
                }
            }
            return possible;
        }
        public static int editDistance(string s, string t)
        {
            if (String.IsNullOrEmpty(s) || String.IsNullOrEmpty(t)) return 0;

            int lengthS = s.Length;
            int lengthT = t.Length;
            var distances = new int[lengthS + 1, lengthT + 1];
            for (int i = 0; i <= lengthS; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthT; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthS; i++)
                for (int j = 1; j <= lengthT; j++)
                {
                    int cost = t[j - 1] == s[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min(Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost);
                }
            return distances[lengthS, lengthT];
        }
    }
}


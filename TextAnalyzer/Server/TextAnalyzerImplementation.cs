using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;


namespace Server
{
    /// <summary>
    /// The server implementation of method calls
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    class TextAnalyzerImplementation : ITextAnalyzerContract
    {
        public KeyValuePair<string, int> AnalyzeText(string text)
        {
            Dictionary<string, int> wordCount = new Dictionary<string, int>(); //Creat a dictionary to store word and count
            char[] notNeededChars = {'\n',';',',','.','\r',' '};
            string[] words = text.Split(notNeededChars);
            foreach (string word in words)
            {
                if (word.Length >= 1)
                { 
		            if (!wordCount.ContainsKey(word)) //Adding a new word
		            {
			            wordCount.Add(word, 1);
		            }
		            else //Updating existing word
		            {
			            wordCount[word] += 1;
		            }
                }
	        }
            var maxKey = wordCount.OrderByDescending(x => x.Value).FirstOrDefault().Key;
            var maxValue = wordCount[maxKey];
            return new KeyValuePair<string, int>(maxKey, maxValue);
        }

        public Dictionary<string, HashSet<string>> FindTyposForWordsInText(string text)
        {
            Dictionary<string, HashSet<string>> wordCount = new Dictionary<string, HashSet<string>>();
            char[] notNeededChars = {'\n',';',',','.','\r',' '};
            string[] words = text.Split(notNeededChars);
            foreach (string word in words)
            {
                if(word.Length >= 1) { 
                    bool exists = false;  //to check if the word is a typo and doesn't need to be added separately
                    foreach (var wordAndHash in wordCount)
                    {
                        if(levenshteinDis(wordAndHash.Key,word) == 1) { //found Typo 
                            wordAndHash.Value.Add(word); 
                            exists = true;
                            break;
                        }
                    }
                    if (!wordCount.ContainsKey(word) && !exists)  //add new word
                    {
                      wordCount.Add(word, new HashSet<string>());
                    }
                    words = words.Where(sameWord => sameWord != word).ToArray(); // remove all other instances of the word
	            }
            }
            foreach (var wordAndHash in wordCount.ToList()) //remove words that have no typo
            {
                if (wordAndHash.Value.Count == 0)
                    wordCount.Remove(wordAndHash.Key);
            }
            return wordCount;
        }

        public  int LetterCount(string text) //receives a string of text and returns the total number of letters (letters are ‘a’-‘z’, ‘A’-‘Z’)
        {
            int textLength = text.Length;
            int textLengthCount = 0;
            int letterCount = 0;
            char letter;
            while(textLengthCount<textLength)
            {
                letter = text[textLengthCount]; 
               if((letter >='a' && letter <='z') || (letter >='A' && letter <='Z'))
               {
                    letterCount++;
               }
               textLengthCount++;
            }
            return letterCount;
        }

        private int levenshteinDis(string s, string t) //Levenshtein distance algorithm
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}

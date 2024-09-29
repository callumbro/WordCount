namespace WordCount.CountAlgorithms
{
    using System.Collections.Generic;

    public class LoopCountWords : ICountWords
    {
        public async Task<Dictionary<string, int>> CountWords(string[] words, int wordCount)
        {
            Dictionary<string, int> wordCounts = new();

            for (int wordIndex = 0; wordIndex < wordCount; wordIndex++)
            {
                if (wordCounts.ContainsKey(words[wordIndex]))
                {
                    wordCounts[words[wordIndex]]++;
                }
                else
                {
                    wordCounts[words[wordIndex]] = 1;
                }
            }

            return wordCounts;
        }
    }
}

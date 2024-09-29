namespace WordCount.CountAlgorithms
{
    using System.Collections.Generic;

    public class LinqCountWords : ICountWords
    {
        public async Task<Dictionary<string, int>> CountWords(string[] words, int wordCount)
        {
            Dictionary<string, int> wordCounts = new();

            wordCounts = words
                .Take(wordCount)
                .GroupBy(word => word)
                .ToDictionary(word => word.Key, word => word.Count());

            return wordCounts;
        }
    }
}

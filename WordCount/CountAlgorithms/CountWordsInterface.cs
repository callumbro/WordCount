namespace WordCount.CountAlgorithms
{
    public interface ICountWords
    {
        public Task<Dictionary<string, int>> CountWords(string[] words, int wordCount);
    }
}

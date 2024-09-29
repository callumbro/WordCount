namespace WordCount
{
    using WordCount.CountAlgorithms;

    /// <summary>
    /// Counts the number of each word in a specified file. 
    /// </summary>
    public class CountWords
    {
        #region Members

        // Master list of word counts.
        // Using a Dictionary because the key is indexed for very fast lookups.
        public Dictionary<string, int> WordCounts { get; private set; }

        private static int WordsPerBatch;

        #endregion

        #region Constructors

        /// <summary>
        /// Will count the number of each word from the input file and will save to the output file.
        /// </summary>
        /// <param name="wordsPerBatch">The number of words to process for each thread, default is 5000 as that seemed to be the best on my computer.</param>
        public CountWords(int wordsPerBatch = 5000)
        {
            WordCounts = new Dictionary<string, int>();
            WordsPerBatch = wordsPerBatch;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Counts the number of each work occurance in the specified input file.
        /// </summary>
        /// <param name="inputFilePath">The path to the input file, either relative or absolute.</param>
        /// <returns>Async void task.</returns>
        public async Task ComputeWordCounts(string inputFilePath)
        {
            #region Validation

            if (!File.Exists(inputFilePath))
            {
                // File does not exist.
                Console
                    .WriteLine($"The specified input file '{inputFilePath}' does not exist.");
            }

            #endregion


            // The standard English punctuation is as follows: period, comma, apostrophe, quotation, question, exclamation, brackets, braces, parenthesis, dash, hyphen, ellipsis, colon, semicolon.
            // https://www.lynchburg.edu/
            char[] separators = [' ', '\r', '\n', '.', ',', '\'', '’', '"', '“', '”', '?', '!', '-', '–', '—', '(', ')', '[', ']', '{', '}', '…', ':', ';'];

            // Read in the file, using a FileStream and StreamReader so large files do not have to be completely loaded into memory.
            using (FileStream fileStream = File.OpenRead(inputFilePath))
            using (StreamReader fileStreamReader = new StreamReader(fileStream))
            {
                #region Stream Reading Variables

                // The character read in from the file.
                char singleLetter;

                // Stores the word as it is being assembled.
                string word = string.Empty;

                // Buffer to store a batch of words for processing.
                string[] wordBuffer = new string[WordsPerBatch];
                int wordBufferCount = 0;

                #endregion


                // Loop to read in the file and build the words one character at a time.
                // Thought of loading in a set amount of bytes at a time from the file, but did not want to cut a word in half.
                while (!fileStreamReader.EndOfStream)
                {
                    // Read in the next character.
                    singleLetter = (char)fileStreamReader.Read();

                    if (separators.Contains(singleLetter)
                        || fileStreamReader.EndOfStream)
                    {
                        if (fileStreamReader.EndOfStream)
                        {
                            // Add the last character before the file ends on the next loop.
                            word += singleLetter;
                        }

                        // Separator reached, we have found a word.

                        // Skip processing if the word is empty.
                        // Chose to include single letter words.
                        if (word.Length == 0)
                        {
                            continue;
                        }

                        // Add the new word to the buffer.
                        wordBuffer[wordBufferCount] = word.ToLower();
                        wordBufferCount++;

                        // Reset for the next word.
                        word = "";

                        // Check if the buffer is full and can be sent for processing.
                        if (wordBufferCount == WordsPerThread)
                        {
                            await ProcessWordBuffer(wordBuffer, wordBufferCount);

                            // Reset buffer count so new set of words can be assigned.
                            wordBufferCount = 0;
                        }
                    }
                    else
                    {
                        // Add character to continue spelling the word.
                        word += singleLetter;
                    }
                }

                // Process last partial word buffer
                if (wordBufferCount < WordsPerThread)
                {
                    await ProcessWordBuffer(wordBuffer, wordBufferCount);
                }
            }
        }

        public async Task ProcessWordBuffer(string[] wordBuffer, int wordBufferCount)
        {
            // Easily swiitchable class for different word counting techniques.
            LinqCountWords countWords = new();
            //LoopCountWords countWords = new();

            // Word buffer is full send to a new thread for processing.
            Dictionary<string, int> newWordCounts = await countWords
                .CountWords(wordBuffer, wordBufferCount);

            // Update the master list with the results.
            foreach (var newWord in newWordCounts.Keys)
            {
                if (WordCounts.ContainsKey(newWord))
                {
                    WordCounts[newWord] += newWordCounts[newWord];
                }
                else
                {
                    WordCounts
                        .Add(newWord, newWordCounts[newWord]);
                }
            }
        }

        /// <summary>
        /// Outputs the results to the file specified. 
        /// </summary>
        public void OutputWordCounts(string outputFilePath, bool writeToConsole = false)
        {
            // Sort by the count, and then the words before printing.
            WordCounts = WordCounts
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key)
                .ToDictionary();

            // Print out the results, overwrite the file if it already exists.
            using (StreamWriter fileWriter = new StreamWriter(outputFilePath, false))
            {
                string output = "";
                foreach (var word in WordCounts)
                {
                    output = $"{word.Key}, {word.Value}";

                    if (writeToConsole)
                    {
                        Console
                            .WriteLine(output);
                    }

                    fileWriter
                        .WriteLine(output);
                }
            }
        }

        #endregion
    }
}
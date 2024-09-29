namespace WordCountTest
{
    using System.Text;
    using WordCount;

    public class CountWordsTest
    {
        [Fact]
        public async Task WordCountComputeTest()
        {
            // Arrange
            string inputFile = "../../../TestFiles/basic file.txt";

            // Act
            CountWords wordCount = new();
            await wordCount
                .ComputeWordCounts(inputFile);
            Dictionary<string, int> result = wordCount.WordCounts;

            // Assert
            Dictionary<string, int> correctResult = new Dictionary<string, int>
            {
                { "test", 3 },
                { "brackets", 2 },
                { "some", 2 },
                { "too", 2 },
                { "word", 2 },
                { "&", 1 },
                { "a", 1 },
                { "also", 1 },
                { "and", 1 },
                { "basic", 1 },
                { "braces", 1 },
                { "capitals", 1 },
                { "colons", 1 },
                { "counts", 1 },
                { "curly", 1 },
                { "even", 1 },
                { "file", 1 },
                { "is", 1 },
                { "lines", 1 },
                { "maybe", 1 },
                { "need", 1 },
                { "new", 1 },
                { "other", 1 },
                { "quotation", 1 },
                { "this", 1 },
                { "to", 1 }
            };

            result = result
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key)
                .ToDictionary();

            Assert
                .True(correctResult.SequenceEqual(result));
        }

        [Fact]
        public async Task WordCountOutputTest()
        {
            // Arrange
            string inputFile = "../../../TestFiles/basic file.txt";
            string outputFile = "output.txt";

            // Act
            CountWords wordCount = new();
            await wordCount
                .ComputeWordCounts(inputFile);
            wordCount
                .OutputWordCounts(outputFile);

            // Assert
            string output = File
                .ReadAllText(outputFile);
            
            StringBuilder correctOutput = new();
            correctOutput.AppendLine("test, 3");
            correctOutput.AppendLine("brackets, 2");
            correctOutput.AppendLine("some, 2");
            correctOutput.AppendLine("too, 2");
            correctOutput.AppendLine("word, 2");
            correctOutput.AppendLine("&, 1");
            correctOutput.AppendLine("a, 1");
            correctOutput.AppendLine("also, 1");
            correctOutput.AppendLine("and, 1");
            correctOutput.AppendLine("basic, 1");
            correctOutput.AppendLine("braces, 1");
            correctOutput.AppendLine("capitals, 1");
            correctOutput.AppendLine("colons, 1");
            correctOutput.AppendLine("counts, 1");
            correctOutput.AppendLine("curly, 1");
            correctOutput.AppendLine("even, 1");
            correctOutput.AppendLine("file, 1");
            correctOutput.AppendLine("is, 1");
            correctOutput.AppendLine("lines, 1");
            correctOutput.AppendLine("maybe, 1");
            correctOutput.AppendLine("need, 1");
            correctOutput.AppendLine("new, 1");
            correctOutput.AppendLine("other, 1");
            correctOutput.AppendLine("quotation, 1");
            correctOutput.AppendLine("this, 1");
            correctOutput.AppendLine("to, 1");

            Assert
                .Equal(output, correctOutput.ToString());
        }
    }
}
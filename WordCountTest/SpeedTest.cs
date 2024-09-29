namespace WordCountTest
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using WordCount;

    public class SpeedTest
    {
        // Test file provided free by https://www.gutenberg.org
        [Fact]
        public async Task ShakespeareTestAsync()
        {
            // Arrange
            string inputFile = "../../../TestFiles/The Complete Works of William Shakespeare by William Shakespeare.txt";
            string outputFile = "output.txt";

            // Act
            CountWords wordCount = new(5000);
            var s = Stopwatch.StartNew();
            await wordCount
                .ComputeWordCounts(inputFile);
            s.Stop();
            string t = s.ToString();
            wordCount
                .OutputWordCounts(outputFile);

            // Assert
            Assert
                .True(true);
        }

        // https://meta.wikimedia.org/wiki/Data_dump_torrents#English_Wikipedia
        // File not provided in repo due to size
        [Fact]
        public async Task WikipediaTest()
        {
            // Arrange
            string inputFile = "simplewiki-20230820-pages-articles-multistream.xml";
            string outputFile = "output.txt";

            // Act
            CountWords wordCount = new(5000);
            await wordCount
                .ComputeWordCounts(inputFile);
            wordCount
                .OutputWordCounts(outputFile);

            // Assert
            Assert
                .True(true);
        }
    }
}

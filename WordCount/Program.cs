namespace WordCount
{
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {
            string inputFilePath = string.Empty;
            string outputFileName = string.Empty;

            if (args.Length == 0)
            {
                Console
                    .WriteLine("Please specify an input file path and an output file name.");

                return;
            }

            // Get the values from the passed commandline parameters.
            for (int parameterNumber = 0; parameterNumber < args.Length; parameterNumber++)
            {
                if (args[parameterNumber] == "-h"
                    || args[parameterNumber] == "-help"
                    || args[parameterNumber] == "-?")
                {
                    Console
                        .WriteLine("Usage: wordcount <input file path> <output file name>");

                    return;
                }

                // Use a switch statement to easily support more input parameters.
                // A whole bunch of else if statements is not as easy to read.
                switch (parameterNumber)
                {
                    case 0:
                        inputFilePath = args[parameterNumber];
                        break;
                    case 1:
                        outputFileName = args[parameterNumber];
                        break;
                }
            }

            #region Validation

            // Validate input parameters.

            if (!File.Exists(inputFilePath))
            {
                // File does not exist.
                Console
                    .WriteLine($"The specified input file '{inputFilePath}' does not exist.");
            }

            if (string.IsNullOrEmpty(outputFileName))
            {
                Console
                    .WriteLine("Please specify an output file name after the input file path.");
            }

            #endregion

            // Try and get the folder the input file is located in, to put the output file in the same folder.
            string outputFilePath = outputFileName;
            string outputDirectory = Path.GetDirectoryName(inputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory))
            {
                outputFilePath = Path.Combine(outputDirectory, outputFileName);
            }

            CountWords countWords = new();
            countWords
                .ComputeWordCounts(inputFilePath)
                .Wait();
            countWords
                .OutputWordCounts(outputFilePath);
        }

    }
}

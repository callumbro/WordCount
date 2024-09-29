namespace WordCount
{
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {
            string inputFilePath = string.Empty;
            string outputFileName = string.Empty;
            bool outputToConsole = false;
            int wordsPerBatch = 0;

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
                    Console
                        .WriteLine("Optional parameters:");
                    Console
                        .WriteLine("'-console' to output the results to console and the output file.");
                    Console
                        .WriteLine("'batch <integer>' to change the word batch size away from the default of 5000.");

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

                if (args[parameterNumber] == "-console"
                    || args[parameterNumber] == "-c")
                {
                    outputToConsole = true;
                }
                else if (args[parameterNumber] == "-batch")
                {
                    parameterNumber++;
                    if (parameterNumber > args.Length)
                    {
                        Console
                            .WriteLine("No number of words per batch specified.");
                        return;
                    }

                    if (!int.TryParse(args[parameterNumber], out wordsPerBatch))
                    {
                        Console
                            .WriteLine("Specified number of words per batch is not an integer.");
                        return;
                    }
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
            string outputDirectory = Path
                .GetDirectoryName(inputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory))
            {
                outputFilePath = Path
                    .Combine(outputDirectory, outputFileName);
            }

            CountWords countWords;
            if (wordsPerBatch > 0)
                countWords = new(wordsPerBatch);
            else
                countWords = new();

            countWords
                .ComputeWordCounts(inputFilePath)
                .Wait();
            countWords
                .OutputWordCounts(outputFilePath, outputToConsole);
        }

    }
}

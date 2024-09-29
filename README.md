# WordCount

This commandline program is written in C# using .NET Core 8 as it was the most recent LTS supporting release.

The program takes in 2 mandatory arguments to run:
1. The location of the text file to parse through.
2. The name of the output file. This file will be placed in the same folder as the input file.

And 2 optional Arguments:
1. "-console" or "-c" if included will display the output to the console and to the output file.
2. "-batch <integer>" to specify the number of words to include per processing batch. Default is 5000.

The output file is sorted first by the number of word occurances with the most common word appearing at the top; then  aplhabetically by word.


I chose a Dictionary type to store the words and their occurance count. This was because a Dictionary is already indexed on the Key therefore, the lookup and storage would be really fast. So because of this efficiency I did not see the point in spending the time to implement a similar storage medium myself. 

All the words are converted to lowercase before storing the word dictionary.


When testing I was unable to see a significant difference between using LINQ or just a simple loop to count the word occurances. I am suspecting that most of the slowdown could be caused by the file reading portion. 

The file reading is currently implemented by loading the file in one character at a time and parsing those characters into words using the delimiting characters:
`[' ', '\r', '\n', '.', ',', '\'', '’', '"', '“', '”', '?', '!', '-', '–', '—', '(', ')', '[', ']', '{', '}', '…', ':', ';']`
 These words are then grouped into a word array with a default size of 5000 words. Once there is a full array it is sent for processing as one batch. I tested with different array sizes and 5000 seemed to run the fastest for my computer. 

### Secondary Test Branch
I figured it was worth another try to read in the files to see if the file reading could be easily optimized. My idea was to try reading in the file in line by line. I tested this implementation with the provided Shakespeare file and it ended up being 300 ms slower on average to process. I imagine that part of this slowdown could be from iterating over the list of word strings more times due to the implementation being less intertwined. I put this implementation into the branch "file-parsing-by-line". 


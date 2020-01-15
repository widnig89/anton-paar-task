# Anton Paar Task

## Project Structure

There are 4 projects inside:

1. AntonPaarTask: containing the application
2. AntonPaarTaskTest: tests according to the application and logic
3. ParseLibrary: the extracted library parsing strings into words
4. StringParseLibraryTest: tests for the parsing functionality

## Application Features

After starting the application you can import a text file (.txt) which will be processed by a background worker to extract word occurrences out of it.
A datagridview shows in a 2 column table the words with the number of occurrences. 
As it was unclear from the requirements whether this output should be shown in the application or outside, I added an export menu item which saves the imported results in a .csv file (tab-separated).
Since the files can be huge and word occurrences is the most interesting thing in this application, I added a line chart visualizing it.

## Further Notes

The project structure could of course be more splitted in patterns (e.g. MVP), but for this minimal tool I deciced to only split it up into a data layer, handling the occurences and the view. 
The ParseLibrary only is cabable of reading strings and split them into words. Since text files are huge, the code was written to just read line per line to avoid having the whole text file in memory, and the lib's method will hence be called for each line in a file.
 
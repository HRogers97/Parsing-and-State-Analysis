# Parsing and State Analysis

This program contains an array of strings and manually parses the text. 

It will then print data on how many words of each length appeared and also prints all of the doubles and ints found whithin the text.

## States
This parsing is done by going through various states relating to what kind of character is being processed. For example, 'A' would enter the word state, and '1' would enter the number state.

The state changes also depend on the previous characters that have been processed. For example, 'e' could be used in word state to parse 'Eric' or it could also be used in double state to process '+.1234567e+05'

The program uses the following states to parse text:

### White State
White space (e.g. spaces, tabs)

### Word State
Any normal string words

### Num state
Integers

### Double state
Doubles
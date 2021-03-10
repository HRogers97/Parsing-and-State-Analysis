/*********************************************** ID BLOCK *****************************************************
 * Software Designer:    Hunter Rogers
 * 
 * Description:          This program processes an array of strings for words and numbers. The programs
 *                       uses states and character types to determine what to do with each character. 
 *                       Transitions to and from different states will result in different actions. The
 *                       states are determined by the character type and the current state (e.g. if the
 *                       next character is 'E' and the current state is num, it will transition into 
 *                       exponent state, otherwise it will go into word state). It then stores the lengths
 *                       of the words and the frequency of each length. It will also convert all the numbers
 *                       into integers or doubles depending on the state. Then the results of the data 
 *                       processing is printed.
 **************************************************************************************************************/

/************************************* PRELIMINARIES *******************************************/
using static System.Console;

namespace rogeHa4v4
{
    class Program
    {
        enum StateType { white, word, num, dble, expt };
        enum CharType { whsp, lett, expo, digit, plus, minus, point, quote, stop };
        
        static char[] line;                         //Current line being processed
        static char ch;                             //Current character being processed
        static CharType type = CharType.whsp;       //Character type of ch
        static StateType state = StateType.white;   //Current state
        static int wlen;                            //Length of current word
        static int k;                               //Current index in the current line
        static int len;                             //Length of current line
        static char[] currentNum = new char[100];   //Array of chars in num being processed
        static int y;                               //Index to store next char in currentNum
        static int[] myWrds = new int[100];         //Array of frequencies for each word length
        static int w = 0;                           //Store largest word length (used for printing)
        static int[] myInts = new int[100];         //Array of integer numbers from lines
        static double[] myDbles = new double[100];  //Array of double numbers from lines
        static int it = 0, d = 0;                   //Index to store next number in respective array

        /************************************* MAIN CODE BLOCK *******************************************/
        static void Main(string[] args)
        {
            int nLines = 4;  //Number of lines in array

            //Array of string lines to be processed
            string[] lines = new string[] {
                "    first 123		and then -.1234 but you'll need 123.456		 and 7e-4 plus one like +321. all quite avant-",
                "garde   whereas ellen's true favourites are 123.654E-2	exponent-form which can also be -54321E-03 or this -.9E+5 ",
                "We'll prefer items like			fmt1-decimal		+.1234567e+05 or fmt2-dec -765.3245 or fmt1-int -837465 and vice-",
                "versa or even format2-integers -19283746   making one think of each state's behaviour for 9 or even 3471e-7 states " };

            //Print out the text lines as single strings
            for (int k = 0; k < nLines; k++)
            {
                WriteLine(lines[k], "\n");
            }

            //Process lines
            for (int i = 0; i < nLines; i++)
            {
                line = lines[i].ToCharArray();      //Process each line char by char
                len = line.Length;                  //Length of current line
                k = 0;                              //Reset index of char array to process next line

                //Continue processing word if previous line ended mid-word
                if (state != StateType.word || type != CharType.minus)
                    state = StateType.white;

                ch = line[0];
                type = getType(ch);


                while (k < len - 1)
                {
                    //Which state are we currently in?
                    switch (state)
                    {
                        case StateType.white:
                            WhiteState();
                            break;
                        case StateType.word:
                            WordState();
                            break;
                        case StateType.num:
                            NumState();
                            break;
                        case StateType.dble:
                            DblState();
                            break;
                        case StateType.expt:
                            ExpoState();
                            break;
                    }
                }
            }

            //Results of processing strings
            WriteLine("\n");
            WriteLine("Analysis Results");
            WriteLine("----------------\n");

            //Pieces of table for displaying results
            string top =    "╔═════════════════╗";
            string mid =    "╠═════════════════╣";
            string bottom = "╚═════════════════╝";

            WriteLine("Word Results:");
            WriteLine(top);
            WriteLine("║Length  Frequency║");
            WriteLine(mid);
            for(int i = 1; i <= w; i++) //Go through array up to index of biggest length
            {
                if(myWrds[i] != 0)      //Only display results where frequency is > 0
                {
                    WriteLine("║" + i.ToString().PadRight(2) + "\t\t" + myWrds[i].ToString().PadLeft(2) + "║");

                    //Display correct section of the table
                    if (i != w) 
                        WriteLine(mid);
                    else
                        WriteLine(bottom);
                }
            }
            Write("\n");
            ReadLine();

            WriteLine("Integer Results:");
            WriteLine(top);
            WriteLine("║index       value║");
            WriteLine(mid);
            for (int i = 0; i < it; i++)    //Go through array up to last index where a num was stored
            {
                WriteLine("║" + i.ToString().PadRight(7) + myInts[i].ToString().PadLeft(10) + "║");
                
                //Display correct section of the table
                if (i != it - 1)
                    WriteLine(mid);
                else
                    WriteLine(bottom);
            }
            Write("\n");
            ReadLine();

            WriteLine("Double Results:");
            WriteLine(top);
            WriteLine("║index       value║");
            WriteLine(mid);
            for (int i = 0; i < d; i++)     //Go through array up to last index where a num was stored
            {
                WriteLine("║" + i.ToString().PadRight(7) + myDbles[i].ToString().PadLeft(10) + "║");
                if (i != d - 1)
                    WriteLine(mid);
                else
                    WriteLine(bottom);
            }
            Write("\n");
            ReadLine();
        }

        /**************************************** GET TYPE **********************************************/
        static CharType getType(char ch)
        {
            CharType type = CharType.whsp;  //Default char tyoe

            if (isSpace(ch))
                type = CharType.whsp;
            else if (isAlpha(ch))
                if (toUpper(ch) == 'E')
                    type = CharType.expo;
                else
                    type = CharType.lett;
            else if (isDigit(ch))
                type = CharType.digit;
            else
            {
                switch (ch)
                {
                    case '+':
                        type = CharType.plus;
                        break;
                    case '-':
                        type = CharType.minus;
                        break;
                    case '.':
                        type = CharType.point;
                        break;
                    case '\'':
                        type = CharType.quote;
                        break;
                }
            }
            return type;
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isSpace(char ch)
        {
            return (ch == ' ' || ch == '\t' || ch == '\n');
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isAlpha(char ch)
        {
            return ((toUpper(ch) >= 'A' && toUpper(ch) <= 'Z'));
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static char toUpper(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                ch = (char)(ch - ('a' - 'A'));
            return ch;
        }
        /************************************************************************************************/
        /*************************************** WHITE STATE ********************************************/
        static void WhiteState()
        {
            while (state == StateType.white && k < len)
            {
                //Which state are we transitioning to?
                switch (type)
                {
                    case CharType.lett:
                    case CharType.expo:
                        WhiteToWord();
                        break;
                    case CharType.digit:
                    case CharType.plus:
                    case CharType.minus:
                        WhiteToNum();
                        break;
                    case CharType.point:
                        WhiteToDble();
                        break;
                    default:                //No state transition
                        if (k < len - 1)
                            ch = line[++k]; //Get next character
                        else
                            return;         //Exit loop if end of the line
                        type = getType(ch);
                        break;
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WHITE TO WORD ******************************************/
        static void WhiteToWord()
        {
            state = StateType.word;
            wlen = 1;               //Start of a new word
            if (k < len - 1)
                ch = line[++k];     //Get next character
            else
                return;             //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /*************************************** WHITE TO NUM *******************************************/
        static void WhiteToNum()
        {
            //Start of new num
            y = 0;
            currentNum[y] = ch;

            state = StateType.num;
            if (k < len - 1)
                ch = line[++k]; //Get next character
            else
                return;         //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /*************************************** WHITE TO DBLE ******************************************/
        static void WhiteToDble()
        {
            state = StateType.dble;
            if (k < len - 1)
                ch = line[++k]; //Get next character
            else
                return;         //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /*************************************** WORD STATE *********************************************/
        static void WordState()
        {
            while (state == StateType.word && k < len)
            {
                //Which state are we transitioning to?
                switch (type)
                {
                    case CharType.whsp:
                        WordToWhite();
                        break;
                    default:                //No state transition
                        wlen++;             //Length of word increases
                        if (k < len - 1)
                            ch = line[++k]; //Get next character
                        else
                            return;
                        type = getType(ch); //Exit loop if end of the line
                        break;
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WORD TO WHITE ******************************************/
        static void WordToWhite()
        {
            myWrds[wlen]++;     //Increase frequency count for current word length
            if (wlen > w)       //Is this the biggest word so far?
                w = wlen;       //Store biggest length

            state = StateType.white;
            if (k < len - 1)
                ch = line[++k]; //Get next charcter
            else
                return;
            type = getType(ch); //Exit loop if end of the line
        }
        /************************************************************************************************/
        /**************************************** NUM STATE *********************************************/
        static void NumState()
        {
            while (state == StateType.num && k < len)
            {
                //Which state are we transitioning to?
                switch (type)
                {
                    case CharType.whsp:
                        NumToWhite();
                        break;
                    case CharType.point:
                        NumToDbl();
                        break;
                    case CharType.expo:
                        NumToExpo();
                        break;
                    default:                    //No state transition
                        currentNum[++y] = ch;   //Store num into array
                        if (k < len - 1)
                            ch = line[++k];     //Get next character
                        else
                            return;             //Exit loop if end of the line
                        type = getType(ch);    
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** NUM TO WHITE ******************************************/
        static void NumToWhite()
        {
            myInts[it] = (int) AtoF(currentNum);   //Convert array of chars into integer
            it++;                                  //Store index of last num in array
            state = StateType.white;
            if (k < len - 1)
                ch = line[++k];                    //Get next charcter
            else
                return;                            //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** NUM TO DBL ********************************************/
        static void NumToDbl()
        {
            currentNum[++y] = ch;           //Store num into array
            state = StateType.dble;
            if (k < len - 1)
                ch = line[++k];             //Get next charcter
            else
                return;                     //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** NUM TO EXPO *******************************************/
        static void NumToExpo()
        {
            currentNum[++y] = ch;           //Store num into array
            state = StateType.expt;
            if (k < len - 1)
                ch = line[++k];             //Get next charcter
            else
                return;                     //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** DBL STATE *********************************************/
        static void DblState()
        {
            while (state == StateType.dble && k < len)
            {
                switch (type)
                {
                    //Which state are we transitioning to?
                    case CharType.whsp:
                        DblToWhite();
                        myDbles[d] = AtoF(currentNum);  //Convert array of chars into num
                        d++;
                        break;
                    case CharType.expo:
                        DblToExpo();
                        break;
                    default:                            //No state transition
                        currentNum[++y] = ch;           //Store num into array
                        if (k < len - 1)
                            ch = line[++k];             //Get next charcter
                        else
                            return;                     //Exit loop if end of the line
                        type = getType(ch);
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** DBL TO WHITE ******************************************/
        static void DblToWhite()
        {
            state = StateType.white;
            if (k < len - 1)
                ch = line[++k];     //Get next charcter
            else
                return;             //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** DBL TO EXPO *******************************************/
        static void DblToExpo()
        {
            currentNum[++y] = ch;   //Store num into array
            state = StateType.expt;
            if (k < len - 1)
                ch = line[++k];     //Get next charcter
            else
                return;             //Exit loop if end of the line
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** EXPO STATE ********************************************/
        static void ExpoState()
        {
            while (state == StateType.expt && k < len)
            {
                //Which state are we transitioning to?
                switch (type)
                {
                    case CharType.whsp:
                        ExpoToWhite();
                        myDbles[d] = AtoF(currentNum);  //Convert array of chars into double
                        d++;
                        break;
                    default:                            //No state transition
                        currentNum[++y] = ch;           //Store num into array
                        if (k < len - 1)
                            ch = line[++k];             //Get next charcter
                        else
                            return;                     //Exit loop if end of the line
                        type = getType(ch);
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** EXPO TO WHITE *****************************************/
        static void ExpoToWhite()
        {
            state = StateType.white;
            if (k < len - 1)
                ch = line[++k];     //Get next charcter
            else
                return;             //Exit loop if end of the line
            type = getType(ch);
        }

        /************************************************************************************************/
        /************************************* ASCII TO  FLOAT ******************************************/
        static double AtoF(char[] s)
        {
            double val;                                       //Double value of num
            double power;                                     //How many decimal places?
            int i;                                            //Current index of char array
            int sign = 1;                                     //Sign of num (positive or negative)

            for (i = 0; isSpace(s[i]) && i <= y; i++) ;       //Skip empty spaces in array
            if (s[i] == '-')                                  //Is num negative?
                sign = -1;
            if (s[i] == '-' || s[i] == '+')                   //Get past sign to first num in array
                i++;
            for (val = 0.0; isDigit(s[i]) && i <= y; i++)     //Process digits
                val = 10.0 * val + s[i] - '0';                //Get double value of digit chars in array
            if (s[i] == '.')                                  //Skip past decimal to next digit
                i++;
            for (power = 1.0; isDigit(s[i]) && i <= y; i++)   //Process digits after decimal
            {
                val = val * 10.0 + s[i] - '0';                //Get double value of digit chars in array
                power *= 10;                                  //Get position to place decimal
            }

            val = val * sign / power;                         //Place decimal into correct position, and add sign if needed

            if (toUpper(s[i]) == 'E')                         //Is there an exponent?
            {
                i++;                                          //Skip past exponent
                int expt;                                     //Store value of exponent
                int esign = 1;                                //Sign of exponent (positive or negative)
                if (s[i] == '-')                              //Is exponent negative?
                    esign = -1;                     
                if (s[i] == '-' || s[i] == '+')               //Skip past sign to next digit
                    i++;
                for (expt = 0; isDigit(s[i]) && i <= y; i++)  //Process digits
                    expt = expt * 10 + s[i] - '0';            //Get double value of digits in exponent

                //Apply the exponent, multiply or divide as needed
                if (esign == 1)
                    while (expt-- > 0)
                        val *= 10;
                else
                    while (expt-- > 0)
                        val /= 10;
            }

            return val;
        }
    }
}

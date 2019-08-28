using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IttezanPos.Helpers
{
   public static class NumbersToEnglishText
    {
       
    public static int ToInt(this string str)
    {

        int output;
        int.TryParse(str, out output);
        return output;
    }


static List<string> oneTo19Text = new List<string> {
"Zero", "One" , "Two", "Three", "Four", 
  "Five", "Six", "Seven", "Eight", 
  "Nine", "Ten" , "Eleven" , "Twelve",
  "Thirteen", "Fourteen", "Fifteen" , 
  "Sixteen" , "Seventeen", "Eighteen" , "Nineteen"
};

static Dictionary<int, string> tensDigit = 
       new Dictionary<int, string>() 
{  
    { 2,"Twenty"},
    { 3,"Thirty"},
    { 4,"Fourty"},
    { 5,"Fifty"},
    { 6,"Sixty"},
    { 7,"Seventy"},
    { 8,"Eighty"},
    { 9,"NineTy"}
};

static Dictionary<int, string> HundredDigit = 
       new Dictionary<int, string>() 
{  
    { 3,"Hundred"},
    { 4,"Thousand"},
    { 6,"Million"},    
    { 8,"Billion"},
    {10,"Trillion" }
    };

public static string ConvertNumberAsText(int num)
{
    int numberLength = num.ToString().Length;
    string numberString = num.ToString();
    int position = numberLength;
    if (numberLength == 1)
        return oneTo19Text[num];
    string result = string.Empty;
    int number = 0;

    // loop the position in number string
    for (int startPosition = 0; startPosition < numberLength; startPosition++)
    {
        // check the position is equal to hundred,
        // thousand and Lakhs or its hundred ,tenthousand .....
        Dictionary<int,string> getHundtedWord = HundredDigit.Where(
           p => p.Key == position).ToDictionary(p => p.Key, p => p.Value);

        if (getHundtedWord.Count == 0)
        {
            number = numberString.Substring(startPosition, 1).ToInt();
            if (number < 2)
            {
                number = numberString.Substring(startPosition, 2).ToInt();
                startPosition++;
                position--;
            }
            else
            {
                result += " " + tensDigit[number];
                startPosition++;
                position--;
                number = numberString.Substring(startPosition, 1).ToInt();
            }

            result += " " + oneTo19Text[number];
            if (position > 2)
                result += " " + HundredDigit[position] + ",";
        }
        else
        {
            number = numberString.Substring(startPosition, 1).ToInt();
            result += " " + oneTo19Text[number];
            if (position > 2)
            result += " " + HundredDigit[position] + ",";
        }

        position--;
    }

    return result;
}
    }
}

using System;
namespace MyFirstCSharp
{
    public class FizzBuzz
    {
        private FizzBuzz()
        {
        }

        public static string Calculate(int i)
        {
            bool fizzOrBuzz = false;
            string result = "";
            if (i % 3 == 0)
            {
                fizzOrBuzz = true;
                result += "fizz";
            }
            if (i % 5 == 0)
            {
                fizzOrBuzz = true;
                result += "buzz";
            }
            if (!fizzOrBuzz)
            {
                result += i;
            }
            return result;
        }
    }
}

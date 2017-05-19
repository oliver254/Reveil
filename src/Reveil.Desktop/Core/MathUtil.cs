using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Core
{
    /// <summary>
    /// Helper class that contains methods that execute mathematical operations
    /// </summary>
    public static class MathUtil
    {

        public static void ConvertToAngle(DateTime? end, out double minute, out double second)
        {
            DateTime now = DateTime.Now;
            minute = 0;
            second = 0;

            if(end == null)
            {
                minute = Convert.ToDouble(now.Minute);
                second = Convert.ToDouble(now.Second);
            }
            else if(end > now)
            {
                TimeSpan duration = end.Value.Subtract(now);
                minute = Convert.ToDouble(duration.Minutes);
                second = Convert.ToDouble(duration.Seconds);
            }
            minute *= 6;
            second *= 6;
        }

        /// <summary>
        /// Validates the string passed by parsing it as int and checking if it is inside the bounds specified 
        /// then the resulting int will be incremented/decremented
        /// </summary>
        /// <param name="num">The string to parse as int and increment/decrement</param>
        /// <param name="minValue">The min value for the bound checking</param>
        /// <param name="maxVal">The max value for the bounds checking</param>
        /// <param name="increment">Pass true to increment and false to decrement</param>
        /// <returns>Returns the new number incremented or decremeneted</returns>
        public static int IncrementDecrementNumber(string num, int minValue, int maxVal, bool increment)
        {
            int newNum = ValidateNumber(num, minValue, maxVal);
            if (increment)
                newNum = Math.Min(newNum + 1, maxVal);
            else
                newNum = Math.Max(newNum - 1, 0);
            return newNum;
        }

        /// <summary>
        /// Parses the number and makes sure that it is within the bounds specified
        /// </summary>
        /// <param name="newNum">The string to parse and validate</param>
        /// <param name="minValue">The min value for the bound checking</param>
        /// <param name="maxValue">The max value for the bound checking</param>
        /// <returns>Returns the int that was constructed from the string + bound checking</returns>
        public static int ValidateNumber(string newNum, int minValue, int maxValue)
        {
            int num;
            if (!int.TryParse(newNum, out num))
                return 0;

            return ValidateNumber(num, minValue, maxValue);
        }

        /// <summary>
        /// makes sure that the number is within the bounds specified
        /// </summary>
        /// <param name="newNum">The number to validate</param>
        /// <param name="minValue">The min value for the bound checking</param>
        /// <param name="maxValue">The max value for the bound checking</param>
        /// <returns>Returns the int that was constructed from the string + bound checking</returns>
        public static int ValidateNumber(int newNum, int minValue, int maxValue)
        {
            newNum = Math.Max(newNum, minValue);
            newNum = Math.Min(newNum, maxValue);

            return newNum;
        }
    }
}

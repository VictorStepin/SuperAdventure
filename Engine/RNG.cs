﻿using System;
using System.Security.Cryptography;

namespace Engine
{
    // This is the more complex version
    public static class RNG
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        /// <summary>
        /// Returns randomly generated integer value between inclusively minimum and maximum borders.
        /// </summary>
        /// <param name="minimumValue">Mininum possible value.</param>
        /// <param name="maximumValue">Maximum possible value.</param>
        /// <returns></returns>
        public static int NumberBetween(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];

            _generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            // We are using Math.Max, and substracting 0.00000000001,
            // to ensure "multiplier" will always be between 0.0 and .99999999999
            // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // We need to add one to the range, to allow for the rounding done with Math.Floor
            int range = maximumValue - minimumValue + 1;

            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);
        }
    }
}
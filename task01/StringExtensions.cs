using System;
using System.Linq;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var filteredInputString = new string(input
                .ToLowerInvariant()
                .Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
                .ToArray());

            string ReversedfilteredInputString = "";

            for (int i = 0; i < filteredInputString.Length; i++)
                ReversedfilteredInputString += filteredInputString[filteredInputString.Length - 1 - i];

            return filteredInputString.SequenceEqual(ReversedfilteredInputString);
        }
    }
}

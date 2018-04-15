namespace iParking
{
    public static class StringExtensions
    {
        /// <summary>
        /// Indicates whether the string has an actual value and is not null
        /// </summary>
        /// <param name="input">The string to verify</param>
        /// <returns></returns>
        public static bool Exists(this string input)
            => !string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input);
    }
}

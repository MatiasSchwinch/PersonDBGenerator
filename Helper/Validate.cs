namespace PersonDBGenerator.Helper
{
    public static class Validate
    {
        public static bool ValidateRange(string input, int min = 1, int max = int.MaxValue)
        {
            if (input == null & int.TryParse(input, out int stringToNumber))
                return false;

            return stringToNumber >= min & stringToNumber <= max;
        }
    }
}

namespace Algorithms.Algorithm {
    internal class Filter {
        /// <summary>
        /// Simple filter with most common mistakes, meant for a slight adjustment that stays mostly true to the reading
        /// </summary>
        /// <param name="term"> term to be substituted</param>
        /// <returns>term with character substitutions</returns>
        public static string SimpleFilter(string? term) {

            if (term is null)
                return "";

            return term
                .Replace("O", "0")
                .Replace("I", "1")
                .Replace("i", "1")
                .Replace("l", "1")
                .Replace("L", "1")
                .Replace("B", "8")
                ;
        }

        /// <summary>
        /// Vague filter that replaces a lot of characters in order to try and scrap out what the inpput meant
        /// </summary>
        /// <param name="term"> term to be substituted</param>
        /// <returns>term with character substitutions</returns>
        public static string VagueFilter(string? term) {

            if (term is null)
                return "";

            return term
                .Replace("O", "0")
                .Replace("o", "0")
                .Replace("Q", "0")
                .Replace("I", "1")
                .Replace("i", "1")
                .Replace("L", "1")
                .Replace("l", "1")
                .Replace("A", "4")
                .Replace("H", "4")
                .Replace("S", "5")
                .Replace("s", "5")
                .Replace("T", "7")
                .Replace("B", "8")
                ;
        }
    }
}

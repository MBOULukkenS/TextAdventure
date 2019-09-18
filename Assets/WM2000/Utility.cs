namespace WM2000
{
    public static class StringExtension
    {
        public static string Anagram(this string str) // Note use of this
        {
            string attempt = Shuffle(str);
            
            while (attempt == str) 
                attempt = Shuffle(str);
            
            return attempt;
        }

        // Based on something we got from the web, not re-written for clarity
        private static string Shuffle(string str)
        {
            char[] characters = str.ToCharArray();
            System.Random randomRange = new System.Random();
            
            int numberOfCharacters = characters.Length;
            
            while (numberOfCharacters > 1)
            {
                int index = randomRange.Next(numberOfCharacters);
                char value = characters[index];
                numberOfCharacters--;

                characters[index] = characters[numberOfCharacters];
                characters[numberOfCharacters] = value;
            }
            
            return new string(characters);
        }
    }
}
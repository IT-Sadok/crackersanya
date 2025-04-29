namespace LibraryApp.Helpers
{
    public static class InputHelper
    {
        public static string PromptForInput(string prompt, string errorMessage)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine(errorMessage);
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        public static DateTime? PromptForDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }
                if (DateTime.TryParse(input, out DateTime parsedDate))
                {
                    return parsedDate;
                }
                Console.WriteLine("Invalid date format. Please try again.");
            }
        }
    }
}

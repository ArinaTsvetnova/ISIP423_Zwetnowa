using System.Text.RegularExpressions;

class Program
{
    private static List<TextStats> allStats = new List<TextStats>();

    static void Main()
    {
        bool continueWork = true;

        while (continueWork)
        {
            Console.WriteLine("Введите текст (минимум 100 символов):");
            string text = Console.ReadLine();

            if (text.Length < 100)
            {
                Console.WriteLine("Текст слишком короткий! Повторите ввод.");
                continue;
            }

            TextStats stats = ProcessText(text);
            allStats.Add(stats);

            Console.WriteLine("\nРезультаты анализа:");
            Console.WriteLine($"Количество слов: {stats.WordCount}");
            Console.WriteLine($"Самое короткое слово: {stats.ShortestWord}");
            Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
            Console.WriteLine($"Гласных: {stats.VowelsCount}, Согласных: {stats.ConsonantsCount}");
            Console.WriteLine($"Самое длинное слово: {stats.LongestWord}");
            Console.WriteLine("Частота букв:");
            foreach (var item in stats.LetterFrequency)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            Console.WriteLine("\nПродолжить работу? (y/n)");
            string answer = Console.ReadLine().ToLower();
            continueWork = answer == "y";
        }

        Console.WriteLine("\nСтатистика по всем обработанным текстам:");
        for (int i = 0; i < allStats.Count; i++)
        {
            Console.WriteLine($"Текст {i + 1}:");
            Console.WriteLine($"Количество слов: {allStats[i].WordCount}");
            Console.WriteLine($"Количество предложений: {allStats[i].SentenceCount}");
            Console.WriteLine($"------------------------");
        }
    }

    private class TextStats
    {
        public int WordCount { get; set; }
        public string ShortestWord { get; set; }
        public int SentenceCount { get; set; }
        public int VowelsCount { get; set; }
        public int ConsonantsCount { get; set; }
        public string LongestWord { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }
    }

    private static TextStats ProcessText(string text)
    {
        TextStats stats = new TextStats
        {
            LetterFrequency = new Dictionary<char, int>()
        };

        // Подсчёт предложений
        stats.SentenceCount = Regex.Matches(text, @"[^\.!\?]+[\.!\?]").Count;

        // Обработка слов
        string[] words = Regex.Split(text, @"\W+")
            .Where(w => !string.IsNullOrEmpty(w))
            .ToArray();

        stats.WordCount = words.Length;
        stats.ShortestWord = words.OrderBy(w => w.Length).FirstOrDefault() ?? "";
        stats.LongestWord = words.OrderByDescending(w => w.Length).FirstOrDefault() ?? "";

        // Подсчёт гласных и согласных
        string vowels = "ауоыиэяюёеАУОЫИЭЯЮЁЕ";
        string consonants = "бвгджзйклмнпрстфхцчшщБВГДЖЗЙКЛМНПРСТФХЦЧШЩ";

        foreach (char c in text)
        {
            if (vowels.Contains(c))
                stats.VowelsCount++;
            else if (consonants.Contains(c))
                stats.ConsonantsCount++;

            if (char.IsLetter(c))
            {
                char lowerC = char.ToLower(c);
                if (stats.LetterFrequency.ContainsKey(lowerC))
                {
                    stats.LetterFrequency[lowerC]++;
                }
                else
                {
                    stats.LetterFrequency.Add(lowerC, 1);
                }
            }
        }

        return stats;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Модель книги
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }

    // Доступные жанры
    private static readonly string[] Genres = { "Фантастика", "Детектив", "Роман", "Приключения" };

    // Список книг и счетчик ID
    private static List<Book> books = new List<Book>();
    private static int currentId = 1;

    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Удалить книгу");
            Console.WriteLine("3. Найти книги");
            Console.WriteLine("4. Отсортировать книги");
            Console.WriteLine("5. Найти самую дорогую/дешёвую книгу");
            Console.WriteLine("6. Группировка по авторам");
            Console.WriteLine("7. Выход");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1: AddBook(); break;
                case 2: RemoveBook(); break;
                case 3: SearchBooks(); break;
                case 4: SortBooks(); break;
                case 5: FindPriceExtremes(); break;
                case 6: GroupByAuthor(); break;
                case 7: running = false; break;
                default: Console.WriteLine("Неверный выбор"); break;
            }
        }
    }

    // Добавление книги
    private static void AddBook()
    {
        Console.WriteLine("Добавление новой книги");

        var book = new Book
        {
            Id = currentId++,
            Title = GetInput("Название"),
            Author = GetInput("Автор"),
            Genre = GetGenre(),
            Year = GetIntInput("Год издания"),
            Price = GetDecimalInput("Цена")
        };

        books.Add(book);
        Console.WriteLine("Книга добавлена!");
    }

    // Удаление книги
    private static void RemoveBook()
    {
        Console.Write("Введите ID книги для удаления: ");
        int id = int.Parse(Console.ReadLine());

        var book = books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            books.Remove(book);
            Console.WriteLine("Книга удалена");
        }
        else
        {
            Console.WriteLine("Книга не найдена");
        }
    }

    // Поиск книг
    private static void SearchBooks()
    {
        Console.WriteLine("Выберите параметр поиска:");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По автору");
        Console.WriteLine("3. По жанру");

        int choice = int.Parse(Console.ReadLine());
        string query = GetInput("Введите поисковый запрос");

        switch (choice)
        {
            case 1:
                var results = books.Where(b => b.Title.ToLower().Contains(query.ToLower()));
                foreach (var book in results)
                {
                    DisplayBookInfo(book);
                }
                break;
            case 2:
                var results1 = books.Where(b => b.Author.ToLower().Contains(query.ToLower()));
                foreach (var book in results1)
                {
                    DisplayBookInfo(book);
                }
                break;
            case 3:
                var results2 = books.Where(b => b.Genre.ToLower().Contains(query.ToLower()));
                foreach (var book in results2)
                {
                    DisplayBookInfo(book);
                }
                break;
        }
    }

    // Сортировка книг
    private static void SortBooks()
    {
        Console.WriteLine("Выберите параметр сортировки:");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По году издания");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1: books = books.OrderBy(b => b.Title).ToList(); break;
            case 2: books = books.OrderBy(b => b.Year).ToList(); break;
        }

        DisplayBooks();
    }

    // Поиск самой дорогой/дешёвой книги
    private static void FindPriceExtremes()
    {
        Console.WriteLine("Выберите, что хотите найти:");
        Console.WriteLine("1. Самую дорогую книгу");
        Console.WriteLine("2. Самую дешевую книгу");

        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                if (books.Count == 0)
                {
                    Console.WriteLine("Список книг пуст");
                    return;
                }

                var mostExpensive = books.OrderByDescending(b => b.Price).FirstOrDefault();
                DisplayBookInfo(mostExpensive);
                break;
            case 2:
                if (books.Count == 0)
                {
                    Console.WriteLine("Список книг пуст");
                    return;
                }

                var cheapest = books.OrderBy(b => b.Price).FirstOrDefault();
                DisplayBookInfo(cheapest);
                break;
            default: Console.WriteLine("Неверный выбор"); break;
        }
    }
    private static void GroupByAuthor()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Список книг пуст");
            return;
        }

        var grouped = books.GroupBy(b => b.Author)
                          .Select(g => new { Author = g.Key, Count = g.Count() });

        foreach (var group in grouped)
        {
            Console.WriteLine($"{group.Author}: {group.Count} книг");
        }
    }
    private static string GetInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine() ?? "";
    }

    private static int GetIntInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return int.Parse(Console.ReadLine());
    }

    private static decimal GetDecimalInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return decimal.Parse(Console.ReadLine());
    }

    private static string GetGenre()
    {
        Console.WriteLine("Выберите жанр:");
        for (int i = 0; i < Genres.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Genres[i]}");
        }

        int choice;

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out choice) &&
                choice >= 1 && choice <= Genres.Length)
            {
                return Genres[choice - 1];
            }
            else
            {
                Console.WriteLine("Неверный выбор. Попробуйте снова:");
            }
        }
    }
    private static void DisplayBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Список книг пуст");
            return;
        }

        foreach (var book in books)
        {
            DisplayBookInfo(book);
        }
    }
    private static void DisplayBookInfo(Book book)
    {
        Console.WriteLine($"ID: {book.Id}");
        Console.WriteLine($"Название: {book.Title}");
        Console.WriteLine($"Автор: {book.Author}");
        Console.WriteLine($"Жанр: {book.Genre}");
        Console.WriteLine($"Год издания: {book.Year}");
        Console.WriteLine($"Цена: {book.Price}");
        Console.WriteLine("------------------------");
    }
}

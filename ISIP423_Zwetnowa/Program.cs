public class Product
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable => Quantity > 0;
    public string Category { get; set; }
}

public enum ProductCategory
{
    Electronics,
    Clothing,
    Food,
}

class Program
{
    private static List<Product> products = new List<Product>();
    private static int nextCode = 1;

    static void Main()
    {
        Console.WriteLine("Программа учёта товаров в магазине");
        Console.WriteLine("1. Добавить товар");
        Console.WriteLine("2. Удалить товар");
        Console.WriteLine("3. Заказать поставку");
        Console.WriteLine("4. Продать товар");
        Console.WriteLine("5. Поиск товаров");
        Console.WriteLine("0. Выход");

        while (true)
        {
            //Console.Clear();
            Console.WriteLine("Выберите действие:");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        Console.WriteLine("Добавление товара");
                        string name = GetInput("Название:");
                        decimal price = GetDecimalInput("Цена:");
                        int quantity = GetIntInput("Количество:");
                        string category = GetInput("Категория (Электроника/Одежда/Еда):");

                        products.Add(new Product
                        {
                            Code = nextCode++.ToString(),
                            Name = name,
                            Price = price,
                            Quantity = quantity,
                            Category = category
                        });

                        Console.WriteLine("Товар добавлен успешно!");
                        Console.ReadKey();
                    }
                    break;

                case "2":
                    {
                        Console.WriteLine("Удаление товара");
                        string code = GetInput("Введите код товара:");

                        Product product = products.FirstOrDefault(p => p.Code == code);

                        if (product != null)
                        {
                            products.Remove(product);
                            Console.WriteLine("Товар успешно удалён!");
                        }
                        else
                        {
                            Console.WriteLine("Товар не найден!");
                        }

                        Console.ReadKey();
                    }
                    break;

                case "3":
                    {
                        Console.WriteLine("Заказ поставки");
                        string code = GetInput("Введите код товара:");

                        Product product = products.FirstOrDefault(p => p.Code == code);

                        if (product != null)
                        {
                            int quantity = GetIntInput("Введите количество для поставки:");
                            product.Quantity += quantity;
                            Console.WriteLine($"Поступление {quantity} единиц товара успешно оформлено!");
                        }
                        else
                        {
                            Console.WriteLine("Товар не найден!");
                        }

                        Console.ReadKey();
                    }
                    break;

                case "4":
                    {
                        Console.WriteLine("Продажа товара");
                        string code = GetInput("Введите код товара:");

                        Product product = products.FirstOrDefault(p => p.Code == code);

                        if (product != null)
                        {
                            if (!product.IsAvailable)
                            {
                                Console.WriteLine("Товар отсутствует на складе!");
                                return;
                            }

                            int quantity = GetIntInput("Введите количество для продажи:");

                            if (quantity > product.Quantity)
                            {
                                Console.WriteLine("Недостаточно товара на складе!");
                                return;
                            }

                            product.Quantity -= quantity;
                            Console.WriteLine($"Успешно продано {quantity} единиц товара!");
                        }
                        else
                        {
                            Console.WriteLine("Товар не найден!");
                        }

                        Console.ReadKey();
                    }
                    break;

                case "5":
                    {
                        Console.WriteLine("Поиск товара");
                        Console.WriteLine("Выберите способ поиска:");
                        Console.WriteLine("1. По коду");
                        Console.WriteLine("2. По названию");
                        Console.WriteLine("3. По категории");

                        string searchType = Console.ReadLine();

                        switch (searchType)
                        {
                            case "1":
                                {
                                    string code = GetInput("Введите код товара:");
                                    Product product = products.FirstOrDefault(p => p.Code == code);

                                    if (product != null)
                                    {
                                        DisplayProductInfo(product);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Товар не найден!");
                                    }
                                }
                                break;

                            case "2":
                                {
                                    string name = GetInput("Введите название товара:");
                                    List<Product> foundProducts = products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();

                                    if (foundProducts.Count > 0)
                                    {
                                        foreach (var product in foundProducts)
                                        {
                                            DisplayProductInfo(product);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Товары не найдены!");
                                    }
                                }
                                break;

                            case "3":
                                {
                                    string category = GetInput("Введите категорию:");
                                    List<Product> foundProducts = products.Where(p => p.Category.ToLower() == category.ToLower()).ToList();

                                    if (foundProducts.Count > 0)
                                    {
                                        foreach (var product in foundProducts)
                                        {
                                            DisplayProductInfo(product);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Товары не найдены!");
                                    }
                                }
                                break;

                            default:
                                Console.WriteLine("Неверный выбор");
                                break;
                        }

                        Console.ReadKey();
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }
    }

    private static void DisplayProductInfo(Product product)
    {
        Console.WriteLine($"Код: {product.Code}");
        Console.WriteLine($"Название: {product.Name}");
        Console.WriteLine($"Цена: {product.Price}");
    }

    private static string GetInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    private static decimal GetDecimalInput(string prompt)
    {
        decimal result;
        while (!decimal.TryParse(GetInput(prompt), out result))
        {
            Console.WriteLine("Неверный формат. Попробуйте ещё раз.");
        }
        return result;
    }

    private static int GetIntInput(string prompt)
    {
        int result;
        while (!int.TryParse(GetInput(prompt), out result))
        {
            Console.WriteLine("Неверный формат. Попробуйте ещё раз.");
        }
        return result;
    }
}

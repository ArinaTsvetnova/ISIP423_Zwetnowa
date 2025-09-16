using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

int a = Convert.ToInt32(Console.ReadLine());
int q = 0;
string[] c = new string[40];
int[] d = new int[40];
for (int i = 0; i < a; i++)
{ 
    string b = Console.ReadLine();
    string[] u = b.Split(new char[] { ';' });
    c[i] = u[0];
    d[i] = Convert.ToInt32(u[1]);
}

Console.WriteLine("");
int e = Convert.ToInt32(Console.ReadLine());
switch (e)
{
    case 1:
        Console.WriteLine("Вы выбрали опцию 1");
        for (int j = 0; j < a; j++)
        { 
            Console.WriteLine(c[j], d[j]);
        }
        break; 
    case 2:
        Console.WriteLine(d.Max());
        Console.WriteLine(d.Min());
        for (int i = 0; i < a; i++)
        {
            q += i;
            Console.WriteLine(q/a);
        }
        break;
    case 3:
        for (int i = 0; i < a; i++)
        {
            if (d[i] < d[i + 1])
            { 
              Swap<int>(ref d[i], ref d[i + 1]);
              Swap<string>(ref c[i], ref c[i + 1])
            }
          Console.WriteLine(c[i],d[i]);
        }
        break;
    case 4:
        Console.WriteLine("Введите курс валюты");
        int kurs = Convert.ToInt32(Console.ReadLine());
        int[] g = new int[40];
        for (int i = 0; i < a; i++)
        {
            g[i] = d[i] * kurs;
            Console.WriteLine(g[i]);
        }
        break;
    case 5:
        Console.WriteLine("Введите товар или услугу:");
        string? word = Console.ReadLine();
        word = word.ToLower();
        for (int j = 0; j < 6; j++)
        {
            int indexOfChar = c[j].IndexOf(word);
            if (indexOfChar >= 0)
            {
                Console.WriteLine(c[j], d[j]);
            }
        }
        break;
    case 0:
        break;
    default:
        Console.WriteLine("Error");
        break;
}

static void Swap<T>(ref T lhs, ref T rhs)
{
  T temp;
  temp = lhs;
  lhs = rhs;
  rhs = tenp;
}

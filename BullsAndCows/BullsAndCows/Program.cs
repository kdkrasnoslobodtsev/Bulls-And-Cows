using System;

class Program
{
    /// <summary>
    /// Приветствие и правила
    /// </summary>
    static void Greeting()
    {
        Console.WriteLine("Добро пожаловать в игру \"Быки и коровы\"\n");
        Console.WriteLine("Правила:");
        Console.WriteLine("Игрок вводит уровень сложности(целое число от 1 до 10) - количество цифр загадываемого числа");
        Console.WriteLine("Компьютер случайным образом загадывает число с разными цифрами");
        Console.WriteLine("Игроку требуется отгадать это число");
        Console.WriteLine("Важно!!!");
        Console.WriteLine("1) Во время отгадывания обязательно вводить число с количеством цифр, совпадающим с уровнем сложности");
        Console.WriteLine("2) У введенного числа все цифры должны различаться");
        Console.WriteLine("При введении некорректных значений игроку будет предоставлена возможность повторить ввод");
        Console.WriteLine("Если игрок ввел загаданное число, то он побеждает");
        Console.WriteLine("Иначе компьютер выводить следующие значения:");
        Console.WriteLine("\"Количество быков\" - количество цифр введенного числа, стоящих на своем месте относительно загаданного числа");
        Console.WriteLine("\"Количество коров\" - количество цифр введенного числа, стоящих не на своем месте относительно загаданного числа");
        Console.WriteLine("Игра продолжается до тех пор, пока игрок не угадает число");
        Console.WriteLine("После окончания игры, есть возможность сыграть еще раз\n");
    }

    /// <summary>
    /// Ввод уровня сложности
    /// </summary>
    /// <param name="difficultyLevel">Уровень сложности</param>
    static void Difficulty(out int difficultyLevel)
    {
        Console.WriteLine("\n\nВведите уровень сложности:");
        string inputValue = Console.ReadLine();
        // проверка корректности введённого значения
        while (!int.TryParse(inputValue, out difficultyLevel) || difficultyLevel < 1 || difficultyLevel > 10)
        {
            Console.WriteLine("\nПохоже на то, что вы ввели некорректное значение, обратитесь к правилам, чтобы выяснить причину");
            Console.WriteLine("Попробуйте снова:");
            inputValue = Console.ReadLine();
        }
    }

    /// <summary>
    /// Генерирует случайное число с различными цифрами.
    /// </summary>
    /// <param name="difficultyLevel">Уровень сложности</param>
    /// <returns></returns>
    static long Generation(int difficultyLevel)
    {
        Random rand = new Random();
        bool[] digits = { false, false, false, false, false, false, false, false, false, false };
        long value = 0;
        int made = 0;
        // Генерируем число из случайных цифр,
        // учитывая их использование ранее
        while (made < difficultyLevel)
        {
            int digit = rand.Next(10);
            // Проверка на использование
            // случайной цифры ранее
            if (!digits[digit])
            {
                // Исключаем появления в начале числа нуля
                if (digit == 0 && made == 0)
                {
                    continue;
                }
                made++;
                value = value * 10 + digit;
                digits[digit] = true;
            }
        }
        return value;
    }

    /// <summary>
    /// Проверка корректности введенного
    /// пользователем значения.
    /// </summary>
    /// <param name="guess">Введенное значение</param>
    /// <param name="difficultyLevel">Уровень сложности</param>
    /// <returns></returns>
    static bool CheckValue(long guess, int difficultyLevel)
    {
        // Проверка на соответствие количества
        // цифр уровню сложности
        if (guess < Math.Pow(10, difficultyLevel - 1) || guess >= Math.Pow(10, difficultyLevel))
        {
            return false;
        }
        bool[] digits = { false, false, false, false, false, false, false, false, false, false };
        long help = guess;
        // Проверка на различие цифр в числе
        while (help > 0)
        {
            if (digits[help % 10])
            {
                return false;
            }
            digits[help % 10] = true;
            help /= 10;
        }
        return true;
    }

    /// <summary>
    /// Угадывание числа пользователем
    /// </summary>
    /// <param name="difficultyLevel">Уровень сложности</param>
    /// <param name="guess">Введенное игроком число</param>
    static void Assumption(int difficultyLevel, out long guess)
    {
        Console.WriteLine("Введите " + difficultyLevel + "-значное число:");
        string inputGuess = Console.ReadLine();
        // Проверка корректности введённого значения.
        while (!long.TryParse(inputGuess, out guess) || !CheckValue(guess, difficultyLevel))
        {
            Console.WriteLine("\nПохоже на то, что вы ввели некорректное значение, обратитесь к правилам, чтобы выяснить причину");
            Console.WriteLine("Попробуйте снова:");
            inputGuess = Console.ReadLine();
        }
    }

    /// <summary>
    /// Обработка введенных предположений
    /// </summary>
    /// <param name="value">Загаданное число</param>
    /// <param name="difficultyLevel">Уровень сложности</param>
    /// <param name="digits">Цифры в введенном числе</param>
    static void BullsAndCows(long value, int difficultyLevel, bool[] digits)
    {
        long guess;
        // Ввод числа
        Assumption(difficultyLevel, out guess);
        // Подсчет количества быков и коров
        while (value != guess)
        {
            int bulls = 0;
            int cows = 0;
            long help1 = value, help2 = guess;
            // Сопоставление цифр загаданного числа
            // и введенного пользователем числа
            while (help1 > 0)
            {
                if (help2 % 10 == help1 % 10)
                {
                    bulls++;
                }
                else
                {
                    if (digits[help2 % 10])
                    {
                        cows++;
                    }
                }
                help1 /= 10;
                help2 /= 10;
            }
            Console.WriteLine("\"Количество быков\" - " + bulls + "\n" + "\"Количество коров\" - " + cows + "\n");
            // Ввод числа
            Assumption(difficultyLevel, out guess);
        }
        return;
    }

    /// <summary>
    /// Определяет, какие цифры есть в числе
    /// </summary>
    /// <param name="value">Число, цифры которого нужно найти</param>
    /// <returns></returns>
    static bool[] FindDigits(long value)
    {
        bool[] digits = { false, false, false, false, false, false, false, false, false, false };
        while(value > 0)
        {
            digits[value % 10] = true;
            value /= 10;
        }
        return digits;
    }

    /// <summary>
    /// Точка входа
    /// </summary>
    static void Main()
    {
        // Приветствие
        Greeting();
        // Возможность играть несколько раз
        do
        {
            int difficultyLevel;
            // Ввод количества цифр загадываемого числа
            Difficulty(out difficultyLevel);
            Console.WriteLine("\nОтличный выбор, давайте начинать");
            // Генерация случайного числа
            long value = Generation(difficultyLevel);
            long help = value;
            // Определяем цифры числа
            bool[] digits = FindDigits(value);
            Console.WriteLine("Попробуйте угадать " + difficultyLevel + "-значное число:");
            // Обрабатываем предположения игрока
            BullsAndCows(value, difficultyLevel, digits);
            Console.WriteLine("\nПоздравляю, вы победили!!!");
            Console.WriteLine("Было загаданно число " + value);
            Console.WriteLine("Если хотите выйти, нажмите Esc, если хотите еще поиграть, нажмите любую другую клавишу");
        } while (Console.ReadKey().Key != ConsoleKey.Escape);
    }
}
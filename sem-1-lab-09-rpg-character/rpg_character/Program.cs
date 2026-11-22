using System.Reflection;

namespace rpg_character;

public class Program
{
    public class Player
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public int maxHealth { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }

        public Player(string name, int max_health)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Имя персонажа не может быть пустым!");
            }

            if (max_health <= 0)
            {
                throw new ArgumentException("Показатель максимального здоровья должен являться положительным числом!");
            }
            
            Name = name;
            maxHealth = max_health;
            Health = max_health;
        }

        public static Player LoadFromFile(string path)
        {
            string ch = File.ReadAllText(path);
            string[] parameters = ch.Split("|");
            Player player = new Player(parameters[0], int.Parse(parameters[2]));
            player.Class = parameters[1];
            int level = int.Parse(parameters[3]);
            if ((level < 1) || (level % 1 != 0))
            {
                throw new ArgumentException("Уровень должен стартовать от единицы и быть целым числом!");
            }
            player.Level = level;
            int exp = int.Parse(parameters[4]);
            if (exp < 0)
            {
                throw new ArgumentException("Показатель опыта не может быть отрицательным числом!");
            }
            player.Experience = exp;
            int gold = int.Parse(parameters[5]);
            if (gold < 0)
            {
                throw new ArgumentException("Показатель золота не может быть отрицательным числом!");
            }
            player.Gold = gold;
            int st = int.Parse(parameters[6]);
            if (st < 0)
            {
                throw new ArgumentException("Показатель силы не может быть отрицательным числом!");
            }
            player.Strength = st;
            int defence = int.Parse(parameters[7]);
            if (defence < 0)
            {
                throw new ArgumentException("Показатель защиты не может быть отрицательным числом!");
            }
            player.Defence = defence;

            return player;
        }

        public static void ShowInfo(Player player)
        {
            Console.WriteLine($"Имя: {player.Name}");
            Console.WriteLine($"Класс: {player.Class}");
            Console.WriteLine($"Уровень: {player.Level}");
            Console.WriteLine($"Опыт: {player.Experience}");
            Console.WriteLine($"Здоровье: {player.Health}");
            Console.WriteLine($"Максимальное здоровье: {player.maxHealth}");
            Console.WriteLine($"Золото: {player.Gold}");
            Console.WriteLine($"Сила: {player.Strength}");
            Console.WriteLine($"Защита: {player.Defence}");
        }

        public static void GetDamage(Player player, int damage)
        {
            if (damage < 0) Console.WriteLine("Ошибка: значение урона не может быть отрицательным!");
            else if (damage >= player.Health) player.Health = 0;
            else
            {
                int old_ph = player.Health;
                player.Health = old_ph - damage;
            }
        }

        public static void Regeneration(Player player)
        {
            player.Health = player.maxHealth;
        }

        public static void GetEx(Player player, int ex)
        {
            if (ex < 0) Console.WriteLine("Ошибка: опыт не может быть отрицательным!");
            else
            {
                int new_ex = ex + player.Experience;
                player.Experience = new_ex;
            }
        }

        public static void GetGold(Player player, int gold)
        {
            if (gold < 0) Console.WriteLine("Ошибка: количество золота не может быть отрицательно!");
            else
            {
                int new_g = gold + player.Gold;
                player.Gold = new_g;
            }
        }

        public static void SpendGold(Player player, int gold)
        {
            if (gold < 0) Console.WriteLine("Ошибка: значение потраченного золота не может быть отрицательным!");
            else if (gold > player.Gold) Console.WriteLine("Ошибка: у вас недостаточно золота!");
            else
            {
                int old_g = player.Gold;
                player.Gold = old_g - gold;
            }
        }

        public static void LevelUp(Player player, int lvl)
        {
            if (lvl < player.Level) Console.WriteLine("Ошибка: понизить уровень невозможно!");
            else player.Level = lvl;
        }
        
    }

    public static void Main()
    {
        Player player = Player.LoadFromFile("character.txt");

        while (true)
        {
            Console.Clear();
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("     УПРАВЛЕНИЕ ПЕРСОНАЖЕМ        ");
            Console.WriteLine("==================================");
            Console.WriteLine("1 - Показать информацию о персонаже");
            Console.WriteLine("2 - Получить урон");
            Console.WriteLine("3 - Восстановить здоровье");
            Console.WriteLine("4 - Получить опыт");
            Console.WriteLine("5 - Получить золото");
            Console.WriteLine("6 - Потратить золото");
            Console.WriteLine("7 - Повысить уровень");
            Console.WriteLine("0 - Выход");
            Console.WriteLine("==================================");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Player.ShowInfo(player);
                    Console.Write("Нажмите на любую кнопку для выхода: ");
                    Console.ReadKey();
                    break;
                
                case "2":
                    Console.Clear();
                    Console.Write("Введите нанесенный урон: ");
                    string d = Console.ReadLine();
                    int damage = int.Parse(d);
                    Player.GetDamage(player, damage);
                    Console.Write("Нажмите на любую кнопку для выхода: ");
                    Console.ReadKey();
                    break;
                
                case "3":
                    Console.Clear();
                    Player.Regeneration(player);
                    break;
                
                case "4":
                    Console.Clear();
                    Console.Write("Введите количество опыта: ");
                    string ex = Console.ReadLine();
                    int exp = int.Parse(ex);
                    Player.GetEx(player, exp);
                    Console.Write("Нажмите на любую кнопку для выхода: ");
                    Console.ReadKey();
                    break;
                
                case "5":
                    Console.Clear();
                    Console.Write("Введите количество золота: ");
                    string g = Console.ReadLine();
                    int gold = int.Parse(g);
                    Player.GetGold(player, gold);
                    Console.Write("Нажмите на любую кнопку для выхода: ");
                    Console.ReadKey();
                    break;
                
                case "6":
                    Console.Clear();
                    Console.Write("Введите количество золота: ");
                    string gold1 = Console.ReadLine();
                    int gold2 = int.Parse(gold1);
                    Player.SpendGold(player, gold2);
                    Console.Write("Нажмите на любую кнопку для выхода: ");
                    Console.ReadKey();
                    break;
                
                case "7":
                    Console.Clear();
                    Console.Write("Введите новый уровень: ");
                    string lvl = Console.ReadLine();
                    int level = int.Parse(lvl);
                    Player.LevelUp(player, level);
                    Console.Write("Нажмите на любую кнопку для выхода: ");
                    Console.ReadKey();
                    break;
                
                case "0":
                    return;
            }
            
        }
    }
}

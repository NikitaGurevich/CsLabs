using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace game_server;

public class Program
{
    public class Player
    {
        public string Login { get; set; }
        public string Level { get; set; }
        public string Health { get; set; }
        public string isOnline { get; set; }

        public Player(string login, string lvl, string health, string is_online)
        {
            Login = login;
            Level = lvl;
            Health = health;
            isOnline = is_online;
        }

        public static List<Player> CreateList(string path)
        {
            string[] temp = File.ReadAllLines(path);
            List<Player> players = new List<Player>();
            foreach (string line in temp)
            {
                string[] line_t = line.Split("|");
                Player player = new Player(line_t[0], line_t[1], line_t[2], line_t[3]);
                players.Add(player);
            }

            return players;
        }

        public static Dictionary<string, Player> CreateDictionary(string path)
        {
            string[] temp = File.ReadAllLines(path);
          Dictionary<string, Player> players = new Dictionary<string, Player>();
          foreach (string line in temp)
          {
              string[] line_t = line.Split("|");
              Player player = new Player(line_t[0], line_t[1], line_t[2], line_t[3]);
              players[player.Login] = player;
          }
          return players;
        }

        public static void AddPlayer(string login, string lvl, string health, string is_online, List<Player> players_list,
            Dictionary<string, Player> players_dict)
        {
            Player player = new Player(login, lvl, health, is_online);
            players_list.Add(player);
            players_dict[player.Login] = player;
        }

        public static void RemovePlayer(List<Player> players_list, Dictionary<string, Player> players_dict,
            string login)
        {
            foreach (Player line in players_list)
            {
                if (line.Login == login)
                {
                    players_list.Remove(line);
                    break;
                }
                
            }

            players_dict.Remove(login);
        }

        public static void ShowPlayers(List<Player> players_list)
        {
            foreach (Player line in players_list)
            {
                Console.WriteLine(line.Login);
            }
        }

        public static void FindByLoginDict(Dictionary<string, Player> players_dict, string login)
        {
            Player player = players_dict[login];
            Console.WriteLine($"Login: {player.Login}");
            Console.WriteLine($"Level: {player.Level}");
            Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine($"Is online: {player.isOnline}");
        }

        public static void FindByLoginList(List<Player> players_list, string login)
        {
            foreach (Player player in players_list)
            {
                if (player.Login == login)
                {
                    Console.WriteLine($"Login: {player.Login}");
                    Console.WriteLine($"Level: {player.Level}");
                    Console.WriteLine($"Health: {player.Health}");
                    Console.WriteLine($"Is online: {player.isOnline}");
                    break;
                }
            }
        }

        public static void ChangeCh(Dictionary<string, Player> players_dict, List<Player> players_list, string login, string level, string health, string status)
        {
            RemovePlayer(players_list, players_dict, login);
            AddPlayer(login, level, health, status, players_list, players_dict);
        }
        
    }

    public static void Main()
    {
        List<Player> players_list = Player.CreateList("players.txt");
        Dictionary<string, Player> players_dict = Player.CreateDictionary("players.txt");

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("        СЕРВЕР ИГРЫ               ");
            Console.WriteLine("==================================");
            Console.WriteLine("1 - Подключить игрока");
            Console.WriteLine("2 - Отключить игрока");
            Console.WriteLine("3 - Показать всех игроков");
            Console.WriteLine("4 - Найти игрока");
            Console.WriteLine("5 - Изменить характеристики");
            Console.WriteLine("6 - Сравнение скорости поиска игрока (по списку и словарю)");
            Console.WriteLine("0 - Выход");
            Console.WriteLine("==================================");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": 
                    Console.Clear();
                   Console.WriteLine("Заполните данные нового игрока:");
                   Console.Write("Login: ");
                   string login = Console.ReadLine();
                   Console.Write("Level: ");
                   string level = Console.ReadLine();
                   try
                   {
                       int num = int.Parse(level);
                       if (num < 0)
                       {
                           Console.WriteLine("Ошибка: значение уровня должно быть не меньше нуля!");
                           Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                           Console.ReadKey();
                           break;
                       }
                   }
                   catch (Exception e)
                   {
                       Console.WriteLine("Ошибка: значение уровня должно быть числом!");
                       Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                       Console.ReadKey();
                       break;
                   }
                   Console.Write("Health: ");
                   string health = Console.ReadLine();
                   try
                   {
                       int num = int.Parse(health);
                       if (num < 0)
                       {
                           Console.WriteLine("Ошибка: значение здоровья должно быть не меньше нуля!");
                           Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                           Console.ReadKey();
                           break;
                       }
                   }
                   catch (Exception e)
                   {
                       Console.WriteLine("Ошибка: значение здоровья должно быть числом!");
                       Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                       Console.ReadKey();
                       break;
                   }
                   Console.Write("Is online? (true/false): ");
                   string is_online = Console.ReadLine();
                   if (is_online != "true" && is_online != "false")
                   {
                       Console.WriteLine("Ошибка: недопустимое значение!");
                       Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                       Console.ReadKey();
                       break;
                   }
                   Player.AddPlayer(login, level, health, is_online, players_list, players_dict);
                   Console.Write("Успешно! Нажмите на любую кнопку для выхода в меню: ");
                   Console.ReadKey();
                   break;
                
                case "2":
                    Console.Clear();
                    Console.Write("Введите логин игрока, которого требуется удалить: ");
                    string login1 = Console.ReadLine();
                    if (players_dict.ContainsKey(login1))
                    {
                        Player.RemovePlayer(players_list, players_dict, login1);
                        Console.Write("Успешно! Нажмите на любую кнопку для выхода в меню: ");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: игрок с данным логином не найден!");
                        Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                        Console.ReadKey();
                        break;
                    }
                
                case "3":
                    Console.Clear();
                    Player.ShowPlayers(players_list);
                    Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                    Console.ReadKey();
                    break;
                
                case "4":
                    Console.Clear();
                    Console.Write("Введите логин игрока: ");
                    string login2 = Console.ReadLine();
                    if (players_dict.ContainsKey(login2))
                    {
                        Player.FindByLoginDict(players_dict, login2);
                        Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: игрок с данным логином не найден!");
                        Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                        Console.ReadKey();
                        break;
                    }
                    
                case "5":
                    Console.Clear();
                   Console.Write("Введите логин игрока, данные которого хотите изменить: ");
                   string login3 = Console.ReadLine(); 
                    if (players_dict.ContainsKey(login3) == false)
                    {
                        Console.WriteLine("Ошибка: игрок с данным логином не найден!");
                        Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                        Console.ReadKey();
                        break;
                    }
                   Console.Write("Введите новый уровень (введите прежний, если не хотите менять): ");
                   string level3 = Console.ReadLine();
                   try
                   {
                       int num = int.Parse(level3);
                       if (num < 0)
                       {
                           Console.WriteLine("Ошибка: значение уровня должно быть не меньше нуля!");
                           Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                           Console.ReadKey();
                           break;
                       }
                   }
                   catch (Exception e)
                   {
                       Console.WriteLine("Ошибка: значение уровня должно быть числом!");
                       Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                       Console.ReadKey();
                       break;
                   }
                   Console.Write("Введите новый показатель здоровья (введите прежний, если не хотите менять): ");
                   string health3 = Console.ReadLine();
                   try
                   {
                       int num = int.Parse(health3);
                       if (num < 0)
                       {
                           Console.WriteLine("Ошибка: значение здоровья должно быть не меньше нуля!");
                           Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                           Console.ReadKey();
                           break;
                       }
                   }
                   catch (Exception e)
                   {
                       Console.WriteLine("Ошибка: значение здоровья должно быть числом!");
                       Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                       Console.ReadKey();
                       break;
                   }
                   Console.Write("Введите новый статус (true/false; введите прежний, если не хотите менять): ");
                   string is_online3 = Console.ReadLine();
                   if (is_online3 != "true" && is_online3 != "false")
                   {
                       Console.WriteLine("Ошибка: недопустимое значение!");
                       Console.Write("Нажмите на любую кнопку для выхода в меню: ");
                       Console.ReadKey();
                       break;
                   }
                    Player.ChangeCh(players_dict, players_list, login3, level3, health3, is_online3);
                    Console.Write("Успешно! Нажмите на любую кнопку для выхода в меню: ");
                    Console.ReadKey();
                    break;
                
                case "6":
                    Console.Clear();
                    Console.Write("Введите любой существующий логин для многократного поиска: ");
                    string login4 = Console.ReadLine();
                    var stopwatch = Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        Player.FindByLoginDict(players_dict, login4);
                    }
                    stopwatch.Stop();
                    var stopwatch2 = Stopwatch.StartNew();
                    for (var i = 0; i < 100000; i++)
                    {
                        Player.FindByLoginList(players_list, login4);
                    }
                    stopwatch2.Stop();
                    Console.WriteLine($"Время поиска по словарю: {stopwatch.Elapsed.TotalMilliseconds} мс");
                    Console.WriteLine($"Время поиска по списку: {stopwatch2.Elapsed.TotalMilliseconds} мс");
                    Console.Write("Нажмите любую кнопку для выхода в меню: ");
                    Console.ReadKey();
                    break;
                
                case "0":
                    return;
            }

        }
    }
 
}

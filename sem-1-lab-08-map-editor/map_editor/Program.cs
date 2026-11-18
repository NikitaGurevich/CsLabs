namespace map_editor;

public class Program
{
    public struct Cell
    {
        public char Symbol;
    }

    public struct Position
    {
        public int X;
        public int Y;
    }

    public struct Player
    {
        public Position Position;
    }

    public class Map
    {
        public Cell[,] Cells;
        public int Width;
        public int Height;
        public Player Player;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cells[x, y] = new Cell { Symbol = '.' };
                }
            }
        }

        public void Draw()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x == Player.Position.X && y == Player.Position.Y)
                    {
                        Console.Write('P');
                    }
                    else
                    {
                        Console.Write(Cells[x, y].Symbol);
                    }
                }

                Console.WriteLine();
            }
        }
        
        public static Map LoadFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string width = lines[0].Split('|')[0];
            string height = lines[0].Split('|')[1];
            int i_width = int.Parse(width);
            int i_height = int.Parse(height);

            Map map = new Map(i_width, i_height);

            for (int y = 1; y < i_height + 1; y++)
            {
                string currentLine = lines[y];
                for (int x = 0; x < i_width; x++)
                {
                    if (currentLine[x] == 'P')
                    {
                        map.Player = new Player
                        {
                            Position = new Position { X = x, Y = y - 1}
                        };
                            
                        map.Cells[x, y - 1] = new Cell {Symbol = '.'};
                    }
                    else
                    {
                        map.Cells[x, y - 1] = new Cell {Symbol = currentLine[x]};
                    }
                }
            }
            return map;
        }

        public void ChangeCellType(int x, int y, char new_symbol)
        {
            Cells[x, y].Symbol = new_symbol;
        }
        
        public void ChangePlayerPosition(int x, int y)
        {
            Player.Position.X = x;
            Player.Position.Y = y;
        }

        public Map Copy()
        {
            Map copy = new Map(Width, Height);
            copy.Player = Player;
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    copy.Cells[x, y] = Cells[x, y];
                }
            }

            return copy;
        }
        
    }

    public static void Main()
    {
        Map current_map = Map.LoadFromFile("map.txt");
        Map backup = null;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("         РЕДАКТОР КАРТ            ");
            Console.WriteLine("==================================");
            Console.WriteLine("1 - Показать карту");
            Console.WriteLine("2 - Изменить клетку");
            Console.WriteLine("3 - Переместить игрока");
            Console.WriteLine("4 - Создать снимок карты");
            Console.WriteLine("5 - Восстановить снимок");
            Console.WriteLine("0 - Выход");
            Console.WriteLine("==================================");
            Console.Write("Введите номер команды: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    current_map.Draw();
                    Console.Write("Нажмите любую клавишу для выхода в меню ");
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    Console.Write("Укажите X координаты, которую хотите изменить: ");
                    string x = Console.ReadLine();
                    Console.Write("Укажите Y координаты, которую хотите изменить: ");
                    string y = Console.ReadLine();
                    int new_x = int.Parse(x);
                    int new_y = int.Parse(y);
                    if (new_x >= current_map.Width && new_y >= current_map.Height)
                    {
                        Console.Write("Ошибка: выход за пределы карты. Нажмите на любую кнопку для выхода в меню и попробуйте еще раз ");
                        Console.ReadKey();
                        break;
                    }
                    Console.WriteLine("# - стена");
                    Console.WriteLine(". - земля");
                    Console.WriteLine("W - вода");
                    Console.Write("На что хотите заменить объект? Введите символ из доступных: ");
                    string symbol = Console.ReadLine();
                    char new_symbol = char.Parse(symbol);
                    if ("#.W".Contains(new_symbol)) current_map.ChangeCellType(new_x, new_y, new_symbol);
                    else
                    {
                        Console.Write("Ошибка: вы ввели не тот символ. Нажмите на любую кнопку для выхода в меню и попробуйте еще раз ");
                        Console.ReadKey();
                    }
                    break;
                
                case "3":
                    Console.Clear();
                    Console.Write("Укажите X координаты, куда хотите переместить игрока: ");
                    string x3 = Console.ReadLine();
                    Console.Write("Укажите Y координаты, куда хотите переместить игрока: ");
                    string y3 = Console.ReadLine();
                    int new_x3 = int.Parse(x3);
                    int new_y3 = int.Parse(y3);
                    if (new_x3 >= current_map.Width && new_y3 >= current_map.Height)
                    {
                        Console.Write("Ошибка: выход за пределы карты. Нажмите на любую кнопку для выхода в меню и попробуйте еще раз ");
                        Console.ReadKey();
                        break;
                    }
                    if ("#W".Contains(current_map.Cells[new_x3, new_y3].Symbol))
                    {
                        Console.Write("Ошибка: на заданной клетке находится препятствие! Нажмите на любую кнопку для выхода в меню и попробуйте еще раз ");
                        Console.ReadKey();
                        break;
                    }
                    current_map.ChangePlayerPosition(new_x3, new_y3);
                    break;
                
                case "4":
                    backup = current_map.Copy();
                    break;
                
                case "5":
                    if (backup == null)
                    {
                        Console.Clear();
                        Console.Write("Ошибка: снимок карты не найден! Нажмите на любую кнопку для выхода в меню и попробуйте еще раз ");
                        Console.ReadKey();
                        break;
                    }

                    current_map = backup.Copy();
                    break;
                
                case "0":
                    return;
            }
        }
    }
    
}

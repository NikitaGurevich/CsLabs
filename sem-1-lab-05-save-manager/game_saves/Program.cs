using Newtonsoft.Json;

namespace game_saves;

public class Position
{
   public int X { get; set; }
   public int Y { get; set; }
}
   
public class SaveData
{
   public string Character { get; set; }
   public string Map { get; set; }
   public Position Position { get; set; }
   public int Health { get; set; }
   public List<string> Inventory { get; set; }
}

public class SaveEntry
{
   public string Name { get; set; }
   public string File { get; set; }
   public DateTime UpdatedAt { get; set; }
}

public class Program
{
   public static void Main()
   {
      List<SaveEntry> entries = LoadIndex("index.json");

      while (true)
      {
         Console.Clear();
         Console.WriteLine("ВНИМАНИЕ! Для того, чтобы программа работала корректно, требуется запуск из терминала");
         Console.WriteLine("");
         Console.WriteLine("=== МЕНЕДЖЕР СОХРАНЕНИЙ ===");
         Console.WriteLine("1. Показать список сохранений");
         Console.WriteLine("2. Посмотреть данные сохранения");
         Console.WriteLine("3. Изменить данные персонажа");
         Console.WriteLine("4. Создать резервную копию");
         Console.WriteLine("5. Восстановить резервную копию");
         Console.WriteLine("6. Удалить сохранение");
         Console.WriteLine("7. Выйти \n");
         Console.Write("Выберите действие: ");

         string num = Console.ReadLine();

         switch (num)
         {
            case "1":
               Console.Clear();
               foreach (SaveEntry entry in entries)
               {
                  Console.WriteLine($"Name: {entry.Name}");
                  Console.WriteLine($"File: {entry.File}");
                  Console.WriteLine($"Updated at: {entry.UpdatedAt} \n");

               }

               Console.Write("Нажмите любую кнопку для возврата в меню");
               Console.ReadKey();
               break;

            case "2":
               Console.Clear();
               Console.Write("Введите имя сохранения: ");
               string name2 = Console.ReadLine();
               if (FindByName(entries, name2) != null)
               {
                  Console.WriteLine("");
                  PrintSave(FindByName(entries, name2), LoadSave("saves", FindByName(entries, name2)));
                  Console.WriteLine("");
                  Console.WriteLine("Нажмите любую кнопку для возврата в меню");
                  Console.ReadKey();
                  break;
               }
               else
               {
                  Console.WriteLine("Ошибка: сохранения с данным именем не существует");
                  Console.WriteLine("Нажмите любую кнопку для возврата в меню");
                  Console.ReadKey();
                  break;
               }

            case "3":
               Console.Clear();
               Console.Write("Введите имя сохранения: ");
               string name3 = Console.ReadLine();
               try
               {
                  SaveEntry entry1 = FindByName(entries, name3);
                  SaveData save = LoadSave("saves", entry1);

                  Console.WriteLine("Актуальные данные сохранения:");
                  PrintSave(entry1, LoadSave("saves", entry1));
                  Console.WriteLine("");
                  Console.Write("Новое имя персонажа (если не хотите менять, нажмите Enter): ");
                  string new_name = Console.ReadLine();
                  Console.Write("Новая карта (если не хотите менять, нажмите Enter): ");
                  string new_map = Console.ReadLine();
                  Console.Write(
                     "Новая позиция - координата X и координата Y через пробел (если не хотите менять, запишите старые координаты): ");
                  string[] coords = Console.ReadLine().Split();
                  string X = coords[0];
                  string Y = coords[1];
                  Console.Write("Новое значение показателя здоровья (если не хотите менять, нажмите Enter): ");
                  string new_health = Console.ReadLine();
                  Console.Write("Новый инвентарь - через запятую (если не хотите менять, нажмите Enter): ");
                  List<string> new_inventory = new List<string>(Console.ReadLine().Split(","));

                  if (new_name != "") save.Character = new_name;
                  if (new_map != "") save.Map = new_map;
                  if (X != "" && Y != "")
                  {
                     save.Position.X = int.Parse(X);
                     save.Position.Y = int.Parse(Y);
                  }

                  if (new_health != "") save.Health = int.Parse(new_health);
                  if ((new_inventory.Contains("")) == false) save.Inventory = new_inventory;

                  SaveGame("saves", entry1, save);
                  entry1.UpdatedAt = DateTime.Now;
                  SaveIndex("index.json", entries);

                  break;
               }
               catch (Exception)
               {
                  Console.WriteLine(
                     "Ошибка: убедитесь, что сохранение с таким именем существует и имя введено корректно");
                  Console.Write("Нажмите любую кнопку для возврата в меню");
                  Console.ReadKey();
                  break;
               }


            case "4":
               Console.Clear();
               Console.Write("Введите имя сохранения: ");
               string name_save = Console.ReadLine();
               try
               {
                  SaveEntry entry2 = FindByName(entries, name_save);
                  CreateBackup("saves", "backups", entry2);

                  break;
               }
               catch (Exception)
               {
                  Console.WriteLine(
                     "Ошибка: убедитесь, что сохранение с таким именем существует и имя введено корректно");
                  Console.Write("Нажмите любую кнопку для возврата в меню");
                  Console.ReadKey();
                  break;
               }

               break;

            case "5":
               Console.Clear();
               Console.Write("Введите имя сохранения: ");
               string name_save1 = Console.ReadLine();
               try
               {
                  SaveEntry entry3 = FindByName(entries, name_save1);
                  RestoreBackup("saves", "backups", entry3);
                  break;
               }
               catch (Exception)
               {
                  Console.WriteLine(
                     "Ошибка: убедитесь, что сохранение с таким именем существует и имя введено корректно");
                  Console.Write("Нажмите любую кнопку для возврата в меню");
                  Console.ReadKey();
                  break;
               }
               break;
            
            case "6":
               Console.Clear();
               Console.Write("Введите имя сохранения: ");
               string name_save2 = Console.ReadLine();
               try
               {
                  SaveEntry entry4 = FindByName(entries, name_save2);
                  DeleteSave("saves", "index.json", entries, entry4);
                  break;
               }
               catch (Exception)
               {
                  Console.WriteLine(
                     "Ошибка: убедитесь, что сохранение с таким именем существует и имя введено корректно");
                  Console.Write("Нажмите любую кнопку для возврата в меню");
                  Console.ReadKey();
                  break;
               }
               break;
            
            case "7":
               return;
         }

      }
   }
   
   public static List<SaveEntry> LoadIndex(string indexPath)
   {
      try
      {
         string r = File.ReadAllText(indexPath);
         List<SaveEntry> lines = JsonConvert.DeserializeObject<List<SaveEntry>>(r);
         return lines;
      }

      catch (FileNotFoundException)
      {
         Console.WriteLine("Ошибка: файл не найден");
         return null;
      }

      catch (JsonException)
      {
         Console.WriteLine("Ошибка: JSON некорректен");
         return null;
      }
   }

   public static SaveData LoadSave(string savesDirectory, SaveEntry entry)
   {
      try
      {
         string r = File.ReadAllText(Path.Combine(savesDirectory, entry.File));
         SaveData line = JsonConvert.DeserializeObject<SaveData>(r);
         return line;
      }
      
      catch (FileNotFoundException)
      {
         Console.WriteLine("Ошибка: файл не найден");
         return null;
      }

      catch (JsonException)
      {
         Console.WriteLine("Ошибка: JSON некорректен");
         return null;
      }
   }

   public static void PrintSave(SaveEntry entry, SaveData save)
   {
      Console.WriteLine($"Файл сохранения: {entry.File} \n");
      Console.WriteLine($"Персонаж: {save.Character}");
      Console.WriteLine($"Карта: {save.Map}");
      Console.WriteLine($"Позиция: {save.Position.X}, {save.Position.Y}");
      Console.WriteLine($"Здоровье: {save.Health}");
      Console.Write("Инвентарь: ");
      for (int i = 0; i < save.Inventory.Count; i++)
      {
         if (i == save.Inventory.Count - 1)
         {
            Console.Write($"{save.Inventory[i]}");
            break;
         }
         else Console.Write($"{save.Inventory[i]}, ");
      }
   }

   public static SaveEntry FindByName(List<SaveEntry> entries, string name)
   {
      foreach (SaveEntry entry in entries)
      {
         if (string.Equals(entry.Name, name, StringComparison.OrdinalIgnoreCase))
         {
            return entry;
         }
      }
      return null;
   }

   public static void SaveGame(string savesDirectory, SaveEntry entry, SaveData save)
   {
      string jsontext = JsonConvert.SerializeObject(save, Formatting.Indented);
      File.WriteAllText(Path.Combine(savesDirectory, entry.File), jsontext);
   }

   public static void SaveIndex(string indexPath, List<SaveEntry> entries)
   {
      string jsontext = JsonConvert.SerializeObject(entries, Formatting.Indented);
      File.WriteAllText(indexPath, jsontext);
   }

   public static void CreateBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
   {
      string upload = Path.Combine(savesDirectory, entry.File);
      string download = Path.Combine(backupsDirectory, entry.File);
      Directory.CreateDirectory(backupsDirectory);
      File.Copy(upload, download, true);
   }

   public static void RestoreBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
   {
      string upload = Path.Combine(backupsDirectory, entry.File);
      string download = Path.Combine(savesDirectory, entry.File);
      if (File.Exists(upload))
      {
         Directory.CreateDirectory(savesDirectory);
         File.Copy(upload, download, true);
      }
      else Console.WriteLine("Сохранение не найдено");
   }

   public static void DeleteSave(string savesDirectory, string indexPath, List<SaveEntry> entries, SaveEntry entry)
   {
      string path = Path.Combine(savesDirectory, entry.File);
      if (File.Exists(path))
      {
         File.Delete(path);
         entries.Remove(entry);
      }
      else
      {
         entries.Remove(entry);
         Console.WriteLine("Файл отсутствует");
      }
      
      entries.Remove(entry);
      SaveIndex(indexPath, entries);
   }
   
}

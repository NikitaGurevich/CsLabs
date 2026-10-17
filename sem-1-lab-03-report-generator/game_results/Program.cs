namespace game_results;

public class Program
{
   static void Main()
   {
      string[] temp = File.ReadAllLines("event_server.log");
      string res = BuildReport(temp);
      Console.WriteLine(res);
   }
   static (DateTime datetime, string level, string category, string message) GetValues(string line)
   {
      string datetime = line.Substring(0, 23);
      
      int index1 = line.IndexOf('[');
      int index2 = line.IndexOf(']');
      string level = line.Substring(index1 + 1, index2 - index1 - 1);

      int index3 = index2 + 1;
      int index4 = line.IndexOf(']', index3);
      string category = line.Substring(index3 + 1, index4 - index3 - 1);

      string message = line.Substring(index4 + 2);

      return (DateTime.Parse(datetime), level, category, message);
   }
   
   public static string BuildReport(string[] lines)
   {
      string name = GetName(CorrectList(lines));
      string date = GetDate(CorrectList(lines));
      string winner = GetWinner(CorrectList(lines));
      string points1 = GetWinnerScores(CorrectList(lines));
      string event_thing = GetEventThing(CorrectList(lines));
      string consolation_prize = GetConsolationPrize(CorrectList(lines));
      string warnings = CountWarnings(CorrectList(lines));
      string errors = CountErrors(CorrectList(lines));

      return "# Итоги события: " + name + "\n" +
             "Дата: " + date + "\n" +
             "Победитель: " + winner + "\n" +
             "Очки победителя: " + points1 + "\n" +
             "Ивентовый предмет: " + event_thing + "\n" +
             "Утешительная награда Железных волков: " + consolation_prize + " очков" + "\n" + 
             "Предупреждений во время события: " + warnings + "\n" +
             "Ошибок во время события: " + errors;
   }

   static string[] CorrectList(string[] lines)
   {
      int index1 = lines.IndexOf(FindLine(lines, "Событие началось: Восстание Ледяного Пламени",
         "Событие началось: Восстание Ледяного Пламени"));
      int index2 = lines.IndexOf(FindLine(lines, "Событие \"Восстание Ледяного Пламени\" закрыто",
         "Событие \"Восстание Ледяного Пламени\" закрыто"));
      string[] res = new string[index2 - index1 + 1];
      int c = 0;
      for (int i = index1; i <= index2; i++)
      {
         res[c] = lines[i];
         c++;
      }

      return res;
   }
   
   static string FindLine(string[] lines, string key, string key2)
   {
      foreach (string line in lines)
      {
         if (line.Contains(key) && line.Contains(key2))
            return line;
      }
      
      return null;
   }
   
   static string GetName(string[] lines)
   {
      string line = FindLine(lines, "Событие началось", ":");
      string temp = GetValues(line).message;
      int index = temp.IndexOf(":") + 2;
      string res = temp.Substring(index);
      return res;
   }

   static string GetDate(string[] lines)
   {
      string line = FindLine(lines, "Событие началось", ":");
      DateTime temp = GetValues(line).datetime;
      return temp.ToShortDateString();
   }

   static string GetWinner(string[] lines)
   {
      string line = FindLine(lines, "объявлены победителями события", "Reward");
      string temp = GetValues(line).message;
      int index = temp.IndexOf(" объявлены победителями события");
      string res = temp.Substring(0, index);
      return res;
   }

   static string GetWinnerScores(string[] lines)
   {
      string line = FindLine(lines, "получили", "очков события");
      string temp = GetValues(line).message;
      int index1 = temp.IndexOf("получили") + 9;
      int index2 = temp.IndexOf(" очков");
      string res = temp.Substring(index1, index2 - index1);
      return res;
   }

   static string GetEventThing(string[] lines)
   {
      string line = FindLine(lines, "получили ивентовый предмет", "Loot");
      string temp = GetValues(line).message;
      int index = temp.IndexOf(':') + 2;
      string res = temp.Substring(index);
      return res;
   }

   static string GetConsolationPrize(string[] lines)
   {
      string line = FindLine(lines, "утешительную награду", "Reward");
      string temp = GetValues(line).message;
      int index1 = temp.IndexOf(':') + 2;
      int index2 = temp.IndexOf(" очков события");
      string res = temp.Substring(index1, index2 - index1);
      return res;
   }

   static string CountWarnings(string[] lines)
   {
      int c = 0;
      foreach (string line in lines)
      {
         string temp = GetValues(line).level;
         if (temp == "Warning") 
            c++;
      }
      return c.ToString();
   }
   
   static string CountErrors(string[] lines)
   {
      int c = 0;
      foreach (string line in lines)
      {
         string temp = GetValues(line).level;
         if (temp == "Error") 
            c++;
      }
      return c.ToString();
   }
   
}

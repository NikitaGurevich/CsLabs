namespace logentry_type;

public struct LogEntry
{
    public DateTime Timestamp;
    public string Level;
    public string Category;
    public string Message;
}
public class Program
{
    
    public static void Main()
    {
        string[] temp = File.ReadAllLines(*адрес файла с логами*);
        foreach (LogEntry line in FilterByDate(ParseLog(temp), new DateTime(2026, 9, 4)))
        {
            Console.WriteLine($"{line.Timestamp} [{line.Level}][{line.Category}] {line.Message}");
        }
        
        
    }
    
    public static LogEntry[] ParseLog(string[] temp)
    {
        LogEntry[] lines = new LogEntry[temp.Length];
        int c = 0;
        foreach (string line in temp)
        {
            string Timestamp_temp = line.Substring(0, 23);
            DateTime Timestamp = DateTime.Parse(Timestamp_temp);
            
            int index1 = line.IndexOf('[') + 1;
            int index2 = line.IndexOf(']');
            string Level = line.Substring(index1, index2 - index1);

            int index3 = line.IndexOf('[', index2 + 1) + 1;
            int index4 = line.IndexOf(']', index2 + 1);
            string Category = line.Substring(index3, index4 - index3);

            string Message = line.Substring(index4 + 2);

            LogEntry entry;
            entry.Timestamp = Timestamp;
            entry.Level = Level;
            entry.Category = Category;
            entry.Message = Message;

            lines[c] = entry;
            c++;
        }

        return lines;
    }

    public static List<LogEntry> FilterByDate(LogEntry[] entries, DateTime date)
    {
        List<LogEntry> lines = new List<LogEntry>();
        foreach (LogEntry line in entries)
        {
            if (line.Timestamp.Date == date.Date)
            {
               lines.Add(line);
            }
        }
        
        return lines;
    }

    public static List<LogEntry> FilterByLevel(LogEntry[] entries, string level)
    {
        List<LogEntry> lines = new List<LogEntry>();
        foreach (LogEntry line in entries)
        {
            if (line.Level == level)
            {
               lines.Add(line);
            }
        }
        return lines;
    }

    public static List<LogEntry> FilterByCategory(LogEntry[] entries, string category)
    {
        List<LogEntry> lines = new List<LogEntry>();
        foreach (LogEntry line in entries)
        {
            if (line.Category == category)
            {
                lines.Add(line);
            }
        }
        return lines;
    }

    public static List<LogEntry> Search(LogEntry[] entries, string text)
    {
        List<LogEntry> lines = new List<LogEntry>();
        foreach (LogEntry line in entries)
        {
            if (line.Message.Contains(text, StringComparison.OrdinalIgnoreCase))
                lines.Add(line);
        }

        return lines;
    }

    public static int CountByLevel(LogEntry[] entries, string level)
    {
        int c = 0;
        foreach (LogEntry line in entries)
        {
            if (line.Level == level)
                c++;
        }

        return c;
    }

    public static string GetServerStatus(LogEntry[] entries)
    {
        int fatal = 0;
        int error = 0;
        foreach (LogEntry line in entries)
        {
            if (line.Level == "Error")
                error++;
            else if (line.Level == "Fatal")
                fatal++;
        }
        if (error == 0 && fatal == 0)
            return "Сервер работает штатно";
        else if (error > 0 && fatal == 0)
            return "Есть ошибки: требуется проверка";
        else if (fatal > 0)
            return "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";
        else return null;
    }
}

namespace check_server;

public static class Program
{
    
    public static void Main()
    {
        Console.Write("Количество игроков на сервере: ");
        int num_of_players = int.Parse(Console.ReadLine());
        
        Console.Write("Количество оперативной памяти сервера: ");
        int ram = int.Parse(Console.ReadLine());
        
        Console.Write("Является ли сервер публичным? (true/false): ");
        bool is_public = bool.Parse(Console.ReadLine());
        
        Console.Write("Защищен ли сервер паролем? (true/false): ");
        bool password = bool.Parse(Console.ReadLine());

        Console.WriteLine(CheckConfiguration(num_of_players, ram, is_public, password));
    }
    
    public static string CheckConfiguration(int num_of_players, int ram, bool is_public, bool password)
    {
        if (ram == 0)
        {
            return "Ошибка оперативной памяти";
        }
        
        float ppl_per_gb_normal = 6.25f;
        float ppl_per_gb_fact = (num_of_players / ram);
        
        if (num_of_players == 0)
        {
            return "Запуск невозможен: количество игроков должно быть больше нуля.";
        }
        else if (ppl_per_gb_fact >= 50)
        {
            return "Запуск невозможен: серверу недостаточно оперативной памяти.";
        }
        else if (password == true)
        {
            return "Запуск возможен с предупреждением: публичный сервер защищён паролем.";
        }
        else if (50 > ppl_per_gb_fact && ppl_per_gb_fact >= 37)
        {
            return
                "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.";
        }
        else
        {
            return "Сервер готов к запуску.";
        }
    }
}
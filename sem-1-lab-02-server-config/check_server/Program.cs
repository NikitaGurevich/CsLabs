namespace check_server;

public static class Program
{
    public static void Main()
    {
        
    }
    public static string CheckConfiguration(int num_of_players, int ram, bool some_ch, bool password)
    {
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

namespace check_server
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Добро пожаловать в утилиту состояния сервера!");
            Console.Write("Введите любой символ для демонстрации информации: ");
            char start = (char)Console.Read();
            Console.WriteLine("---------------------------------------------------");

            string TimeOfWork = "10:53:41";
            float MidPing = 90.2f;
            float ConnectionSpeed = 100.4f;
            ushort NumOfPlayers = 57500;
            
            Console.WriteLine("----------------Основные параметры-----------------");
            Console.WriteLine($"Время работы сервера: {TimeOfWork}");
            Console.WriteLine($"Средний пинг: {MidPing} м/с");
            Console.WriteLine($"Скорость соединения: {ConnectionSpeed} Мбит/с");
            Console.WriteLine($"Количество игроков на сервере: {NumOfPlayers}");
        }
    }
}

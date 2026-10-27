using System;

namespace realtime_monitor;

public class Program
{
    static int[] cpuHistory = new int [20];
    static int[] ramHistory = new int [20];
    static int[] tempHistory = new int [20];

    static int count = 0;
    
    public static void Main()
    {
        var monitor = new ServerMonitor();
        
        monitor.DataReceived += OnDataReceived;
        
        monitor.Start();

        Console.WriteLine("Мониторинг запущен.");
        Console.WriteLine("Для завершения программы нажмите Enter.");
        
        Console.ReadLine();
        
        monitor.Stop();
        
    }
    
    static void OnDataReceived(double cpu, double ram, double temperature)
    {
        int index = count % 20;

        cpuHistory[index] = (int)cpu;
        ramHistory[index] = (int)ram;
        tempHistory[index] = (int)temperature;
        
        count++;
        
        DrawGraphs();
    }

    static void DrawGraphs()
    {
        Console.Clear();
        
        Console.WriteLine("CPU:");
        for (int i = 0; i < 20; i++)
        {
            int real_index;
            
            if (count < 20)
            {
                if (i >= count) break;
                real_index = i;
            }

            else
            {
                int start_index = count % 20;
                real_index = (start_index + i) % 20;
            }

            int num = cpuHistory[real_index];
            int fill = (int)((20 / 100.0) * num);
            string bar = new string('█', fill) + new string(' ', 20 - fill);
            
            Console.WriteLine($"[{bar}] {num}%");
        }
        
        Console.WriteLine("");
        Console.WriteLine("RAM:");
        for (int i = 0; i < 20; i++)
        {
            int real_index;
            
            if (count < 20)
            {
                if (i >= count) break;
                real_index = i;
            }

            else
            {
                int start_index = count % 20;
                real_index = (start_index + i) % 20;
            }

            int num = ramHistory[real_index];
            int fill = (int)((20 / 100.0) * num);
            string bar = new string('█', fill) + new string(' ', 20 - fill);
            
            Console.WriteLine($"[{bar}] {num}%");
        }
        
        Console.WriteLine("");
        Console.WriteLine("CPU Temperature:");
        for (int i = 0; i < 20; i++)
        {
            int real_index;
            
            if (count < 20)
            {
                if (i >= count) break;
                real_index = i;
            }

            else
            {
                int start_index = count % 20;
                real_index = (start_index + i) % 20;
            }

            int num = tempHistory[real_index];
            int fill = (int)((20 / 100.0) * num);
            if (fill > 20) fill = 20;
            if (fill < 0) fill = 0;
            string bar = new string('█', fill) + new string(' ', 20 - fill);
            
            Console.WriteLine($"[{bar}] {num}°C");
        }
        
    }
}

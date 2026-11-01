using System;
using System.IO;


// Что было исправлено:
// 1) Были убраны все лишние отступы в выводе
// 2) Было скорректировано накопление значений переменной _lineIndex в методе PrintTree
// 3) Фикс бесконечной рекурсии
// 4) Фикс системы отступов в методе GetIndent
// 5) Изменение у классов модификаторов доступа на public для использования Unit-тестов


public class Program
{
    // Current line in file
    public static int _lineIndex;

    // Current nesting level
    public static int _depth;

    // File content
    public static string[] _lines = null!;

    public static void Main()
    {
        _lines = File.ReadAllLines("tree.txt");

        Console.WriteLine(_lines[0]);

        _lineIndex = 1;
        
        PrintTree();
    }

    public static void PrintTree()
    {
        while (_lineIndex < _lines.Length)
        {
            var line = _lines[_lineIndex].Trim();

            if (line == "")
            {
                _lineIndex ++;
                continue;
            }

            if (line == "END")
            {
                _depth--;

                _lineIndex++;

                return;
            }

            if (line.StartsWith("BEGIN "))
            {
                var name = line.Substring(6);

                Console.Write(GetIndent());
                Console.WriteLine("─" + name);

                _depth++;
                
                _lineIndex++;
                
                PrintTree();
            }
            
        }
    }

    public static string GetIndent()
    {
        var result = "";

        for (var i = 0; i <= _depth; i++)
        { 
            if (result == "") result += "│";
            else
            {
                if (_depth > 0 && i < _depth) result += "  "; 
                if (_depth > 0 && i == _depth) result += " │";
            }
        }

        return result;
    }
}

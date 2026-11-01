using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace fixed_directoryTree.Tests
{
    [TestFixture]
    public class TreeTests
    {
        [Test]
        public void PrintTree_ChecksIndentationLevels_ByCharIndex()
        {
            // 1. Подготовка: создаем тестовый файл
            string[] testLines = {
                "A",          // Корень (выводится до вызова PrintTree)
                "BEGIN B",    // Уровень 1
                "BEGIN C",    // Уровень 2
                "END",        // Закрываем C
                "BEGIN D",    // Уровень 2 (снова, т.к. C закрыт)
                "END",        // Закрываем D
                "END",        // Закрываем B
                "BEGIN E",    // Уровень 1 (снова, т.к. B закрыт)
                "END"         // Закрываем E
            };

            // Записываем во временный файл (т.к. твой Main читает из tree.txt)
            File.WriteAllLines("tree.txt", testLines);

            // 2. Перехватываем вывод в консоль
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter); // Теперь всё, что пишет Console.WriteLine, идет в stringWriter

            // 3. Запускаем программу
            // Мы вызываем Main напрямую.
            Program.Main(); 

            // Возвращаем консоль обратно (чтобы тест-раннер мог вывести результаты)
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            // 4. Анализируем результат
            string output = stringWriter.ToString();
            
            // Разбиваем вывод на строки, убирая пустые
            string[] outputLines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // 5. Проверки (Assert)
            
            // Ищем строку, где есть буква B
            string lineB = outputLines.FirstOrDefault(l => l.Contains("B"));
            Assert.That(lineB, Is.Not.Null, "Строка с B не найдена в выводе!");
            int indexB = lineB.IndexOf('B');

            // Ищем строку, где есть буква C
            string lineC = outputLines.FirstOrDefault(l => l.Contains("C"));
            Assert.That(lineC, Is.Not.Null, "Строка с C не найдена в выводе!");
            int indexC = lineC.IndexOf('C');

            // Ищем строку, где есть буква D
            string lineD = outputLines.FirstOrDefault(l => l.Contains("D"));
            Assert.That(lineD, Is.Not.Null, "Строка с D не найдена в выводе!");
            int indexD = lineD.IndexOf('D');

            // Ищем строку, где есть буква E
            string lineE = outputLines.FirstOrDefault(l => l.Contains("E"));
            Assert.That(lineE, Is.Not.Null, "Строка с E не найдена в выводе!");
            int indexE = lineE.IndexOf('E');

            // ГЛАВНЫЕ ПРОВЕРКИ (сравниваем индексы):
            
            // 1. B и E должны быть на одном уровне (на первом), значит их индексы равны.
            Assert.That(indexB, Is.EqualTo(indexE), "B и E должны быть на одном уровне вложенности!");

            // 2. C и D должны быть на одном уровне (на втором).
            Assert.That(indexC, Is.EqualTo(indexD), "C и D должны быть на одном уровне вложенности!");

            // 3. C и D должны быть глубже, чем B (их индекс должен быть больше).
            Assert.That(indexC, Is.GreaterThan(indexB), "C должен быть вложен глубже, чем B!");
            Assert.That(indexD, Is.GreaterThan(indexB), "D должен быть вложен глубже, чем B!");
            
            // Удаляем тестовый файл после проверки
            File.Delete("tree.txt");
        }
    }
}

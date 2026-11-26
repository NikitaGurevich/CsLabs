using NUnit.Framework;
using game_server;
using System.Collections.Generic;

namespace game_server.Tests
{
    [TestFixture]
    public class GameServerTests
    {
        // Вспомогательный метод для создания чистых коллекций перед каждым тестом
        private (List<Program.Player>, Dictionary<string, Program.Player>) CreateEmptyCollections()
        {
            var list = new List<Program.Player>();
            var dict = new Dictionary<string, Program.Player>();
            return (list, dict);
        }

        [Test]
        public void AddPlayer_AddsPlayerToBothCollections()
        {
            // Arrange (Подготовка)
            var (list, dict) = CreateEmptyCollections();

            // Act (Действие)
            Program.Player.AddPlayer("NewGuy", "1", "100", "true", list, dict);

            // Assert (Проверка)
            Assert.That(list.Count, Is.EqualTo(1), "Игрок не добавился в список!");
            Assert.That(dict.ContainsKey("NewGuy"), Is.True, "Игрок не добавился в словарь!");
            Assert.That(dict["NewGuy"].Level, Is.EqualTo("1"));
        }

        [Test]
        public void RemovePlayer_RemovesPlayerFromBothCollections()
        {
            // Arrange
            var (list, dict) = CreateEmptyCollections();
            // Сначала добавим двух игроков
            Program.Player.AddPlayer("Alice", "10", "100", "true", list, dict);
            Program.Player.AddPlayer("Bob", "5", "50", "false", list, dict);

            // Act
            Program.Player.RemovePlayer(list, dict, "Alice");

            // Assert
            Assert.That(list.Count, Is.EqualTo(1), "Игрок не удалился из списка!");
            Assert.That(list[0].Login, Is.EqualTo("Bob"), "В списке остался не тот игрок!");
            Assert.That(dict.ContainsKey("Alice"), Is.False, "Игрок не удалился из словаря!");
            Assert.That(dict.ContainsKey("Bob"), Is.True, "Случайно удалили не того игрока!");
        }

        [Test]
        public void ChangeCh_UpdatesPlayerCharacteristics()
        {
            // Arrange
            var (list, dict) = CreateEmptyCollections();
            Program.Player.AddPlayer("Charlie", "2", "20", "true", list, dict);

            // Act
            Program.Player.ChangeCh(dict, list, "Charlie", "99", "999", "false");

            // Assert
            var updatedPlayer = dict["Charlie"];
            Assert.That(updatedPlayer.Level, Is.EqualTo("99"), "Уровень не обновился!");
            Assert.That(updatedPlayer.Health, Is.EqualTo("999"), "Здоровье не обновилось!");
            Assert.That(updatedPlayer.isOnline, Is.EqualTo("false"), "Статус онлайна не обновился!");
        }

        [Test]
        public void CreateList_And_CreateDictionary_ReadSameData()
        {
            // Этот тест требует наличия файла players.txt в папке тестового проекта.
            // Если файла нет, тест упадет, поэтому мы пропускаем его, если файл не найден.
            string testFile = "players.txt";
            if (!System.IO.File.Exists(testFile))
            {
                Assert.Ignore("Файл players.txt не найден в директории тестов. Тест пропущен.");
            }

            // Act
            var list = Program.Player.CreateList(testFile);
            var dict = Program.Player.CreateDictionary(testFile);

            // Assert
            Assert.That(list.Count, Is.GreaterThan(0), "Список пуст!");
            Assert.That(dict.Count, Is.GreaterThan(0), "Словарь пуст!");
            
            // Проверяем, что данные совпадают
            Assert.That(dict.Count, Is.EqualTo(list.Count), "Размеры коллекций не совпадают (возможно дубликаты логинов)");
            
            string firstLogin = list[0].Login;
            Assert.That(dict.ContainsKey(firstLogin), Is.True, "Игрок из списка не найден в словаре!");
            Assert.That(dict[firstLogin].Level, Is.EqualTo(list[0].Level));
        }
    }
}

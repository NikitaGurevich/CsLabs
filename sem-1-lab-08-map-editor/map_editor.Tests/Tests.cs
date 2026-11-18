using NUnit.Framework;
using map_editor;

namespace map_editor.Tests
{
    [TestFixture]
    public class MapTests
    {
        [Test]
        public void Snapshot_RestoreUsingCopy_ReturnsSavedPlayerPosition()
        {
            var map = Program.Map.LoadFromFile("map.txt");
            
            var backup = map.Copy();
            
            var originalPosition = backup.Player.Position;
            
            map.ChangePlayerPosition(2, 2);
            
            map = backup.Copy();
            
            Assert.That(map.Player.Position.X, Is.EqualTo(originalPosition.X));
            Assert.That(map.Player.Position.Y, Is.EqualTo(originalPosition.Y));
        }
    }
}

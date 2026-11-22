using NUnit.Framework;
using rpg_character;
public class CharacterTests
{
    [Test]
    public void TakeDamage_DoesNotMakeHealthNegative()
    {
        var character = new Program.Player("Mage", 100);
        Program.Player.GetDamage(character, 150);
        Assert.That(character.Health, Is.EqualTo(0));
    }
}

using Bogus;

namespace EM.Authentication.UnitTests.Fixtures;

public sealed class PasswordFixture
{
    public static string GeneratePassword(int passwordLenght)
    {
        var random = new Faker().Internet.Random;
        char[] possibleSymbol = { '#', '?', '!', '@', '$', '%', '^', '&', '*', '-' };

        var uppercase = random.Char('A', 'Z').ToString();
        var number = random.Number(100, 999);
        var symbol = new Faker().PickRandom(possibleSymbol);
        var padding = random.String2(passwordLenght - 5);

        var chars = (uppercase + padding + number + symbol).ToArray();
        var shuffledChars = random.Shuffle(chars).ToArray();

        return new string(shuffledChars);
    }
}

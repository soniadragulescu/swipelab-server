using System.Security.Cryptography;

namespace SwipeLab.Domain.DatingProfile.Utils;

public static class WeightedRandom
{
    public static T SelectItem<T>(Dictionary<T, double> weights) where T : notnull
    {
        var totalWeight = weights.Values.Sum();
        var random = RandomNumberGenerator.GetInt32(0, (int)totalWeight + 1);
        double cumulative = 0;

        foreach (var item in weights)
        {
            cumulative += item.Value;
            if (random <= cumulative)
                return item.Key;
        }

        return weights.Last().Key;
    }
}
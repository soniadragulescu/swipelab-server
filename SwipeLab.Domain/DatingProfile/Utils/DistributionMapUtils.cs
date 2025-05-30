namespace SwipeLab.Domain.DatingProfile.Utils;

public static class DistributionMapUtils
{
    public static Dictionary<T, int> CalculateDistribution<T>(int totalCount, Dictionary<T, double> distribution)
        where T : notnull
    {
        if (totalCount < 0)
            throw new ArgumentException("Total count must be non-negative", nameof(totalCount));

        if (distribution == null)
            throw new ArgumentNullException(nameof(distribution));

        // Verify all percentages are non-negative
        if (distribution.Values.Any(p => p < 0))
            throw new ArgumentException("All distribution percentages must be non-negative", nameof(distribution));

        // Normalize percentages to sum to 1 (100%) to handle cases where they don't sum exactly to 1
        double sum = distribution.Values.Sum();
        if (sum <= 0)
            throw new ArgumentException("Sum of distribution percentages must be positive", nameof(distribution));

        var normalizedDistribution = distribution.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value / sum
        );

        var result = new Dictionary<T, int>();
        int remaining = totalCount;

        // First pass: assign integer parts
        foreach (var kvp in normalizedDistribution)
        {
            double exactCount = kvp.Value * totalCount;
            int integerCount = (int)Math.Floor(exactCount);

            result.Add(kvp.Key, integerCount);
            remaining -= integerCount;
        }

        // Second pass: distribute remaining counts to items with largest fractional parts
        if (remaining > 0)
        {
            var fractionalParts = normalizedDistribution
                .Select(kvp => new
                {
                    Key = kvp.Key,
                    FractionalPart = (kvp.Value * totalCount) - Math.Floor(kvp.Value * totalCount)
                })
                .OrderByDescending(x => x.FractionalPart)
                .ThenBy(x => Guid.NewGuid()) // Add randomness for equal fractions
                .Take(remaining)
                .ToList();

            foreach (var item in fractionalParts)
            {
                result[item.Key]++;
            }
        }

        return result;
    }
}
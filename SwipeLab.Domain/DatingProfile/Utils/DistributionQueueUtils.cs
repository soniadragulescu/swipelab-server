using System.Security.Cryptography;

namespace SwipeLab.Domain.DatingProfile.Utils;

public class DistributionQueueUtils
{
    public static Queue<T> CreateDistributionQueue<T>(
        Dictionary<T, int>? counts,
        Func<T> defaultGenerator,
        int totalCount) where T : notnull
    {
        var queue = new Queue<T>();

        if (counts != null)
        {
            foreach (var (value, count) in counts)
            {
                for (int i = 0; i < count; i++)
                {
                    queue.Enqueue(value);
                }
            }
        }
        else
        {
            for (int i = 0; i < totalCount; i++)
            {
                queue.Enqueue(defaultGenerator());
            }
        }

        return new Queue<T>(queue.OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue)));
    }
}
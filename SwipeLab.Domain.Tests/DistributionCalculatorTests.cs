using SwipeLab.Domain.DatingProfile.Utils;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Domain.Tests;

[TestClass]
public class DistributionMapUtilsTests
{
    private enum Fruit { Apple, Banana, Orange }

    [TestMethod]
    public void CalculateDistribution_ExactDistribution_ReturnsCorrectCounts()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 0.5 },
            { Gender.Female, 0.5 }
        };

        // Act
        var result = DistributionMapUtils.CalculateDistribution(8, distribution);

        // Assert
        Assert.AreEqual(4, result[Gender.Male]);
        Assert.AreEqual(4, result[Gender.Female]);
        Assert.AreEqual(8, result.Values.Sum());
    }

    [TestMethod]
    public void CalculateDistribution_UnevenDistribution_HandlesRounding()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 0.6 },
            { Gender.Female, 0.4 }
        };

        // Act
        var result = DistributionMapUtils.CalculateDistribution(10, distribution);

        // Assert
        Assert.AreEqual(6, result[Gender.Male]);
        Assert.AreEqual(4, result[Gender.Female]);
        Assert.AreEqual(10, result.Values.Sum());
    }

    [TestMethod]
    public void CalculateDistribution_ThreeCategoriesWithRemainder_DistributesRemainder()
    {
        // Arrange
        var distribution = new Dictionary<Fruit, double>()
        {
            { Fruit.Apple, 0.333 },
            { Fruit.Banana, 0.333 },
            { Fruit.Orange, 0.334 }
        };

        // Act
        var result = DistributionMapUtils.CalculateDistribution(10, distribution);

        // Assert
        Assert.AreEqual(10, result.Values.Sum());
        CollectionAssert.AreEquivalent(new[] { 3, 3, 4 }, result.Values.ToArray());
    }

    [TestMethod]
    public void CalculateDistribution_PercentagesDontSumToOne_NormalizesCorrectly()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 75 },
            { Gender.Female, 25 },
            { Gender.Unspecified, 25 }
        };

        // Act
        var result = DistributionMapUtils.CalculateDistribution(10, distribution);

        // Assert
        Assert.AreEqual(6, result[Gender.Male]);  // 75/125 = 0.6 → 6 of 10
        Assert.AreEqual(2, result[Gender.Female]);  // 25/125 = 0.2 → 2 of 10
        Assert.AreEqual(2, result[Gender.Unspecified]);  // 25/125 = 0.2 → 2 of 10
        Assert.AreEqual(10, result.Values.Sum());
    }

    [TestMethod]
    public void CalculateDistribution_ZeroTotalCount_ReturnsAllZeros()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 0.5 },
            { Gender.Female, 0.5 }
        };

        // Act
        var result = DistributionMapUtils.CalculateDistribution(0, distribution);

        // Assert
        Assert.AreEqual(0, result[Gender.Male]);
        Assert.AreEqual(0, result[Gender.Female]);
        Assert.AreEqual(0, result.Values.Sum());
    }

    [TestMethod]
    public void CalculateDistribution_OneCategory_ReturnsFullCount()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 1.0 }
        };

        // Act
        var result = DistributionMapUtils.CalculateDistribution(5, distribution);

        // Assert
        Assert.AreEqual(5, result[Gender.Male]);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CalculateDistribution_NegativeTotalCount_ThrowsException()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 0.5 },
            { Gender.Female, 0.5 }
        };

        // Act
        DistributionMapUtils.CalculateDistribution(-1, distribution);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CalculateDistribution_NegativePercentage_ThrowsException()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 0.8 },
            { Gender.Female, -0.2 }
        };

        // Act
        DistributionMapUtils.CalculateDistribution(10, distribution);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CalculateDistribution_AllZeroPercentages_ThrowsException()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 0 },
            { Gender.Female, 0 }
        };

        // Act
        DistributionMapUtils.CalculateDistribution(10, distribution);
    }

    [TestMethod]
    public void CalculateDistribution_LargeCountWithSmallPercentages_DistributesCorrectly()
    {
        // Arrange
        var distribution = new Dictionary<Fruit, double>()
        {
            { Fruit.Apple, 0.01 },
            { Fruit.Banana, 0.09 },
            { Fruit.Orange, 0.9 }
        };

        // Act
        var result = DistributionMapUtils.CalculateDistribution(1000, distribution);

        // Assert
        Assert.AreEqual(10, result[Fruit.Apple]);  // 1% of 1000
        Assert.AreEqual(90, result[Fruit.Banana]);  // 9% of 1000
        Assert.AreEqual(900, result[Fruit.Orange]);  // 90% of 1000
        Assert.AreEqual(1000, result.Values.Sum());
    }

    [TestMethod]
    public void CalculateDistribution_RemainderDistribution_IsFair()
    {
        // Arrange
        var distribution = new Dictionary<Gender, double>()
        {
            { Gender.Male, 1.0/3 },
            { Gender.Female, 1.0/3 },
            { Gender.Unspecified, 1.0/3 }
        };

        // Act (3 items with 10 total should give 3,3,4 distribution)
        var result = DistributionMapUtils.CalculateDistribution(10, distribution);

        // Assert
        Assert.AreEqual(10, result.Values.Sum());
        var counts = result.Values.OrderBy(x => x).ToArray();
        CollectionAssert.AreEqual(new[] { 3, 3, 4 }, counts);
    }
}
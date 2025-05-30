using SwipeLab.Domain.DatingProfile;
using SwipeLab.Domain.DatingProfile.Constants;
using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.DatingProfile.Utils;
using SwipeLab.Domain.Enums;
using SwipeLab.Services.Interfaces.Interfaces;
using SwipeLab.Services.Services;

namespace SwipeLab.Domain.Tests
{
    [TestClass]
    public class DatingProfileGenerationTests
    {
        private readonly DatingProfileGenerationService _generator;
        private readonly MockProfilePictureService _mockPictureService;

        public DatingProfileGenerationTests()
        {
            _mockPictureService = new MockProfilePictureService();
            _generator = new DatingProfileGenerationService(_mockPictureService);
        }

        [TestMethod]
        public void CreateDistributionQueue_WithCounts_CreatesCorrectDistribution()
        {
            // Arrange
            var counts = new Dictionary<Ethnicity, int>
            {
                { Ethnicity.White, 2 },
                { Ethnicity.Black, 1 }
            };

            // Act
            var queue = DistributionQueueUtils.CreateDistributionQueue(
                counts,
                () => Ethnicity.Asian,
                3);

            // Assert
            Assert.AreEqual(3, queue.Count);
            Assert.AreEqual(2, queue.Count(x => x == Ethnicity.White));
            Assert.AreEqual(1, queue.Count(x => x == Ethnicity.Black));
        }

        [TestMethod]
        public void CreateDistributionQueue_WithoutCounts_UsesDefaultGenerator()
        {
            // Arrange
            var fixedEthnicity = Ethnicity.Latino;

            // Act
            var queue = DistributionQueueUtils.CreateDistributionQueue<Ethnicity>(
                null,
                () => fixedEthnicity,
                5);

            // Assert
            Assert.AreEqual(5, queue.Count);
            CollectionAssert.AllItemsAreInstancesOfType(queue.ToList(), typeof(Ethnicity));
            CollectionAssert.AreEquivalent(queue.ToList(), Enumerable.Repeat(fixedEthnicity, 5).ToList());
        }

        [TestMethod]
        public void CreateDistributionQueue_ReturnsShuffledQueue()
        {
            // Arrange
            var counts = new Dictionary<Ethnicity, int>
            {
                { Ethnicity.White, 100 },
                { Ethnicity.Black, 100 }
            };

            // Act
            var queue1 = DistributionQueueUtils.CreateDistributionQueue(
                counts,
                () => Ethnicity.Asian,
                200);
            var queue2 = DistributionQueueUtils.CreateDistributionQueue(
                counts,
                () => Ethnicity.Asian,
                200);

            // Assert - very unlikely to have same order twice
            CollectionAssert.AreNotEqual(queue1.ToList(), queue2.ToList());
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_CreatesCorrectNumberOfProfiles()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 10
            };

            // Act
            var result = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert
            Assert.AreEqual(10, result.DatingProfiles.Count);
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_RespectsEthnicityDistribution()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 100,
                EthnicityCounts = new Dictionary<Ethnicity, int>
                {
                    { Ethnicity.White, 60 },
                    { Ethnicity.Black, 40 }
                }
            };

            // Act
            var result = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert
            var whiteCount = result.DatingProfiles.Count(p => p.Ethnicity == Ethnicity.White);
            var blackCount = result.DatingProfiles.Count(p => p.Ethnicity == Ethnicity.Black);

            Assert.AreEqual(60, whiteCount);
            Assert.AreEqual(40, blackCount);
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_RespectsGenderDistribution()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 100,
                GenderCounts = new Dictionary<Gender, int>
                {
                    { Gender.Male, 70 },
                    { Gender.Female, 30 }
                }
            };

            // Act
            var result = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert
            var maleCount = result.DatingProfiles.Count(p => p.Gender == Gender.Male);
            var femaleCount = result.DatingProfiles.Count(p => p.Gender == Gender.Female);

            Assert.AreEqual(70, maleCount);
            Assert.AreEqual(30, femaleCount);
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_CanRandomizeOrder()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 10,
                ShouldRandomizeOrder = true
            };

            // Act
            var result1 = await _generator.GenerateDatingProfileSetAsync(constraints);
            var result2 = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert - very unlikely to have same order twice
            CollectionAssert.AreNotEqual(
                result1.DatingProfiles.Select(p => p.DatingProfileId).ToList(),
                result2.DatingProfiles.Select(p => p.DatingProfileId).ToList());
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_CreatesValidProfile()
        {
            // Arrange
            var constraints = new DatingProfileGenerationConstraints();

            // Act
            var datingProfile = await _generator.GenerateDatingProfileAsync(constraints);

            // Assert
            Assert.AreNotEqual(Guid.Empty, datingProfile.DatingProfileId);
            Assert.IsNotNull(datingProfile.Name);
            Assert.IsTrue(datingProfile.Age >= 18 && datingProfile.Age <= 50);
            Assert.IsNotNull(datingProfile.PhotoUrl);
            Assert.IsTrue(datingProfile.Hobbies.Count > 0);
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_RespectsAgeRange()
        {
            // Arrange
            var constraints = new DatingProfileGenerationConstraints
            {
                AgeRange = new AgeRange(25, 30)
            };

            // Act
            var profile = await _generator.GenerateDatingProfileAsync(constraints);

            // Assert
            Assert.IsTrue(profile.Age >= 25 && profile.Age <= 30);
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_HeightMatchesGenderDistribution()
        {
            var genders = new[] { Gender.Male, Gender.Female };
            var sampleSize = 1000;
            const double allowedOutlierRate = 0.01; // Allow 1% outliers

            foreach (var gender in genders)
            {
                // Set expected height mean and standard deviation in cm
                var (mean, stdDev) = gender switch
                {
                    Gender.Male => (175.3, 7.6),    // Based on global averages
                    Gender.Female => (162.6, 7.6),
                    _ => throw new ArgumentException("Unsupported gender")
                };

                var heights = new List<int>();

                for (int i = 0; i < sampleSize; i++)
                {
                    var constraints = new DatingProfileGenerationConstraints
                    {
                        Gender = gender
                    };

                    var profile = await _generator.GenerateDatingProfileAsync(constraints);
                    heights.Add(profile.Height);
                }

                // Check that most values fall within 3 standard deviations
                int lowerBound = (int)(mean - 3 * stdDev);
                int upperBound = (int)(mean + 3 * stdDev);
                int outliers = heights.Count(h => h < lowerBound || h > upperBound);

                var good = heights.Where(h => h >= lowerBound && h <= upperBound).ToList();
                var bad = heights.Except(good).ToList();

                bool allWithinRange = heights.All(h => h >= lowerBound && h <= upperBound);
                double avgHeight = heights.Average();
                double outlierRate = (double)outliers / sampleSize;

                Assert.IsTrue(outlierRate <= allowedOutlierRate,
           $"Too many {gender} outliers: {outliers}/{sampleSize} outside [{lowerBound}, {upperBound}]");

                Assert.IsTrue(Math.Abs(avgHeight - mean) <= 2.0,
                    $"Mean {gender} height deviates: expected ≈ {mean}, actual = {avgHeight}");
            }
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_RespectsAllowedEthnicities()
        {
            // Arrange
            var constraints = new DatingProfileGenerationConstraints
            {
                AllowedEthnicities = new HashSet<Ethnicity> { Ethnicity.Asian, Ethnicity.Latino }
            };

            // Act
            var profile = await _generator.GenerateDatingProfileAsync(constraints);

            // Assert
            CollectionAssert.Contains(new[] { Ethnicity.Asian, Ethnicity.Latino }, profile.Ethnicity);
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_RespectsHobbyCountRange()
        {
            // Arrange
            var constraints = new DatingProfileGenerationConstraints
            {
                HobbyCount = new HobbyCountRange(3, 5)
            };

            // Act
            var profile = await _generator.GenerateDatingProfileAsync(constraints);

            // Assert
            Assert.IsTrue(profile.Hobbies.Count >= 3 && profile.Hobbies.Count <= 5);
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_RespectsAllowedHobbies()
        {
            // Arrange
            var allowedHobbies = new HashSet<string> { "Sports and fitness", "Music and concerts" };
            var constraints = new DatingProfileGenerationConstraints
            {
                AllowedHobbies = allowedHobbies,
                HobbyCount = new HobbyCountRange(1, 2)
            };

            // Act
            var profile = await _generator.GenerateDatingProfileAsync(constraints);

            // Assert
            foreach (var hobby in profile.Hobbies)
            {
                CollectionAssert.Contains(allowedHobbies.ToList(), hobby);
            }
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_WhenInterestedInFemale_OnlyGeneratesFemaleProfiles()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 20,
                FixedGender = Gender.Female, // Simulating "Interested in Female"
                GenderCounts = null // Let it generate all female
            };

            // Act
            var result = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert
            Assert.AreEqual(20, result.DatingProfiles.Count);
            Assert.IsTrue(result.DatingProfiles.All(p => p.Gender == Gender.Female));
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_WhenInterestedInMale_OnlyGeneratesMaleProfiles()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 20,
                FixedGender = Gender.Male, // Simulating "Interested in Male"
                GenderCounts = null // Let it generate all male
            };

            // Act
            var result = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert
            Assert.AreEqual(20, result.DatingProfiles.Count);
            Assert.IsTrue(result.DatingProfiles.All(p => p.Gender == Gender.Male));
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_WhenGenderUnspecified_GeneratesBothGenders()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 100,
                FixedGender = null, // if unspecified means the user wants to see all genders
                GenderCounts = null // Default distribution
            };

            // Act
            var result = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert
            Assert.AreEqual(100, result.DatingProfiles.Count);
            Assert.IsTrue(result.DatingProfiles.Any(p => p.Gender == Gender.Male));
            Assert.IsTrue(result.DatingProfiles.Any(p => p.Gender == Gender.Female));
        }

        [TestMethod]
        public async Task GenerateDatingProfileSetAsync_WhenInterestedInFemale_RespectsGenderCountsOverride()
        {
            // Arrange
            var constraints = new DatingProfileSetGenerationConstraints
            {
                SetSize = 10,
                FixedGender = Gender.Female, // Primary filter
                GenderCounts = null
            };

            // Act
            var result = await _generator.GenerateDatingProfileSetAsync(constraints);

            // Assert
            Assert.AreEqual(10, result.DatingProfiles.Count);
            Assert.IsTrue(result.DatingProfiles.All(p => p.Gender == Gender.Female));
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_WhenGenderConstraintSet_ReturnsSpecifiedGender()
        {
            // Arrange
            var constraints = new DatingProfileGenerationConstraints
            {
                Gender = Gender.Female
            };

            // Act
            var profile = await _generator.GenerateDatingProfileAsync(constraints);

            // Assert
            Assert.AreEqual(Gender.Female, profile.Gender);
        }

        [TestMethod]
        public async Task GenerateDatingProfileAsync_WhenNoGenderConstraint_ReturnsRandomGender()
        {
            // Arrange
            var constraints = new DatingProfileGenerationConstraints
            {
                Gender = null
            };
            var generatedGenders = new HashSet<Gender>();

            // Act - run multiple times to ensure both genders can be generated
            for (int i = 0; i < 20; i++)
            {
                var profile = await _generator.GenerateDatingProfileAsync(constraints);
                generatedGenders.Add(profile.Gender);
                if (generatedGenders.Count == 2) break; // Exit early if we got both
            }

            // Assert
            Assert.IsTrue(generatedGenders.Contains(Gender.Male));
            Assert.IsTrue(generatedGenders.Contains(Gender.Female));
        }

        [TestMethod]
        public void GetRandomAge_ReturnsValueInRange()
        {
            // Arrange
            var range = new AgeRange(25, 30);

            // Act
            var age = _generator.GetRandomAge(range);

            // Assert
            Assert.IsTrue(age >= 25 && age <= 30);
        }

        [TestMethod]
        public void GetRandomHobbies_RespectsCountRange()
        {
            // Arrange
            var countRange = new HobbyCountRange(2, 4);
            var hobbies = HobbyConstants.AllHobbies;

            // Act
            var result = _generator.GetRandomHobbies(hobbies, countRange);

            // Assert
            Assert.IsTrue(result.Count >= 2 && result.Count <= 4);
        }

        [TestMethod]
        public void GetRandomHobbies_AdjustsInvalidRanges()
        {
            // Arrange
            var countRange = new HobbyCountRange(5, 3); // Invalid range
            var hobbies = new HashSet<string> { "H1", "H2", "H3", "H4" };

            // Act
            var result = _generator.GetRandomHobbies(hobbies, countRange);

            // Assert
            Assert.IsTrue(result.Count >= 3 && result.Count <= 4); // Should adjust to valid range
        }

        [TestMethod]
        public void GetRandomHobbies_HandlesMaxLargerThanAvailable()
        {
            // Arrange
            var countRange = new HobbyCountRange(1, 10);
            var hobbies = new HashSet<string> { "H1", "H2" };

            // Act
            var result = _generator.GetRandomHobbies(hobbies, countRange);

            // Assert
            Assert.IsTrue(result.Count >= 1 && result.Count <= 2); // Max should be adjusted to 2
        }

        [TestMethod]
        public void GetRandomFromSet_ReturnsFromAllowedSet()
        {
            // Arrange
            var allowed = new HashSet<LookingFor> { LookingFor.LONG_TERM_RELATIONSHIP, LookingFor.SHORT_TERM_FUN };

            // Act
            var result = _generator.GetRandomLookingFor(allowed);

            // Assert
            CollectionAssert.Contains(allowed.ToList(), result);
        }

        [TestMethod]
        public void GetRandomFromSet_ReturnsDefaultWhenAllowedEmpty()
        {
            // Arrange
            var allowed = new HashSet<LookingFor>();

            // Act
            var result = _generator.GetRandomLookingFor(allowed);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LookingFor));
        }

        [TestMethod]
        public void GetRandomEnum_ReturnsValidEnumValue()
        {
            // Act
            var result = _generator.GetRandomEnum<Education>();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Education));
        }

        // Mock service for testing
        private class MockProfilePictureService : IProfilePictureService
        {
            public Task<string> GetRandomProfilePictureUrl(Gender gender, Ethnicity ethnicity, int age)
            {
                return Task.FromResult($"https://example.com/{gender}_{ethnicity}_{age}.jpg");
            }
        }
    }
}


using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.Enums;
using SwipeLab.Services.Configuration;
using SwipeLab.Services.Interfaces.Interfaces;

namespace SwipeLab.Services.Services;

public class ProfilePictureService : IProfilePictureService
{
    private readonly ILogger<ProfilePictureService> _logger;
    private readonly BlobContainerClient _blobContainerClient;
    private readonly StorageSharedKeyCredential _storageCredential;

    private readonly int AGE_RANGE = 5;
    private readonly HashSet<string> _usedPictureUrls;

    public ProfilePictureService(ILogger<ProfilePictureService> logger,
        AzureStorageAccountConfiguration azureStorageAccountConfiguration)
    {
        _logger = logger;
        _blobContainerClient = new BlobContainerClient(azureStorageAccountConfiguration.ConnectionString,
            azureStorageAccountConfiguration.ContainerName);
        _storageCredential = new StorageSharedKeyCredential(
            GetValueFromConnectionString(azureStorageAccountConfiguration.ConnectionString, "AccountName"),
            GetValueFromConnectionString(azureStorageAccountConfiguration.ConnectionString, "AccountKey"));

        _usedPictureUrls = new HashSet<string>();
    }

    public async Task<string> GetRandomProfilePictureUrl(Gender gender, Ethnicity ethnicity, int age)
    {
        string baseBlobPrefix = $"{gender.ToString().ToLower()}/{ethnicity.ToString().ToLower()}/";
        var checkedRanges = new HashSet<(int, int)>();
        var random = new Random();

        int currentRangeStep = 0;

        while (true)
        {
            int minAge = Math.Max(18, age - AGE_RANGE * currentRangeStep);
            int maxAge = age + AGE_RANGE * currentRangeStep;

            // Ensure we don't re-check the same range
            if (!checkedRanges.Contains((minAge, maxAge)))
            {
                _logger.LogInformation("Looking for profile pictures in range {minAge}-{maxAge}", minAge, maxAge);
                var blobItems = await GetBlobItemsInAgeRange(baseBlobPrefix, minAge, maxAge);

                // Filter out used pictures
                var unusedBlobs = blobItems
                    .Where(blob => !_usedPictureUrls.Contains(blob.Name))
                    .ToList();

                if (unusedBlobs.Count > 0)
                {
                    var selectedBlob = unusedBlobs[random.Next(unusedBlobs.Count)];
                    _usedPictureUrls.Add(selectedBlob.Name);

                    return GenerateSasUrl(_blobContainerClient.GetBlobClient(selectedBlob.Name));
                }

                // Remember we already checked this range
                checkedRanges.Add((minAge, maxAge));
            }

            // Break if we already looked in all possible ranges (to avoid infinite loop)
            if (minAge <= 18 && maxAge >= 60)
            {
                _logger.LogError($"No *unique* profile pictures found for {gender}/{ethnicity} around age {age}, even after expanding search.");

                return string.Empty;
            }

            currentRangeStep++;
        }
    }

    private async Task<List<BlobItem>> GetBlobItemsInAgeRange(string baseBlobPrefix, int minAge, int maxAge)
    {
        var blobItems = new List<BlobItem>();

        // Check each age in the range
        for (int age = minAge; age <= maxAge; age++)
        {
            string filePrefix = $"{baseBlobPrefix}{age}/";

            await foreach (var blobItem in _blobContainerClient.GetBlobsAsync(prefix: filePrefix))
            {
                if (!blobItem.Name.EndsWith("/"))
                    blobItems.Add(blobItem);
            }
        }

        return blobItems;
    }

    private string GenerateSasUrl(BlobClient blobClient)
    {
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
            ExpiresOn = DateTimeOffset.UtcNow.AddMonths(6)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        return $"{blobClient.Uri}?{sasBuilder.ToSasQueryParameters(_storageCredential)}";
    }

    private static string GetValueFromConnectionString(string connectionString, string key)
    {
        foreach (var part in connectionString.Split(';'))
        {
            if (part.StartsWith($"{key}="))
                return part.Substring(key.Length + 1);
        }

        throw new ArgumentException($"Invalid connection string: {key} not found");
    }
}
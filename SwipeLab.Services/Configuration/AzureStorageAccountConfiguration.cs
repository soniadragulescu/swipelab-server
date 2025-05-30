namespace SwipeLab.Services.Configuration;

public class AzureStorageAccountConfiguration(string connectionString, string containerName)
{
    public string ConnectionString { get; set; } = connectionString;
    public string ContainerName { get; set; } = containerName;
}
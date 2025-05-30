namespace SwipeLab.Feedback.Configuration;

public class GeminiConfiguration(string model, string baseUrl, string apiKey)
{
    public string Model { get; set; } = model;
    public string BaseUrl { get; set; } = baseUrl;
    public string ApiKey { get; set; } = apiKey;
}
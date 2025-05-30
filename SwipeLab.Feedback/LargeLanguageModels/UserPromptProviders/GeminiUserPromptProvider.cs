using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SwipeLab.Domain.Experiment;
using SwipeLab.Feedback.Configuration;

namespace SwipeLab.Feedback.LargeLanguageModels.UserPromptProviders;

public class GeminiUserPromptProvider : IUserPromptProvider
{
    private readonly ILogger<GeminiUserPromptProvider> _logger;
    private readonly ILlmPromptBuilder _llmPromptBuilder;
    private readonly GeminiConfiguration _geminiConfiguration;
    private readonly HttpClient _httpClient;

    public GeminiUserPromptProvider(
        IHttpClientFactory httpClientFactory,
        ILlmPromptBuilder llmPromptBuilder,
        ILogger<GeminiUserPromptProvider> logger,
        GeminiConfiguration geminiConfiguration)
    {
        _logger = logger;
        _llmPromptBuilder = llmPromptBuilder;
        _geminiConfiguration = geminiConfiguration;

        _httpClient = httpClientFactory.CreateClient("Gemini");
        _httpClient.BaseAddress = new Uri(_geminiConfiguration.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _geminiConfiguration.ApiKey);
    }

    public Task<List<string>> GetUserReflectiveQuestionsAsync(ExperimentInteractionData experimentInteractionData)
    {
        var prompt = _llmPromptBuilder.BuildReflectiveQuestionsPrompt(experimentInteractionData);
        _logger.LogInformation("Using prompt for generating reflective questions with Gemini: {Prompt}", prompt);

        return SendGeminiRequestAsync(
            prompt,
            ParseReflectiveQuestionsResponse
        );
    }

    private async Task<T> SendGeminiRequestAsync<T>(
        string prompt,
        Func<string, T> parseResponse)
    {
        try
        {
            var requestBody = CreateRequestBodyWithStringArraySchema(prompt);

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                $"v1beta/models/{_geminiConfiguration.Model}:generateContent", content);

            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API Error: {StatusCode}, {Error}", response.StatusCode, responseJson);
                throw new Exception($"Gemini API Error: {response.StatusCode}, {responseJson}");
            }

            _logger.LogInformation("Prompt response from Gemini: {PromptResponse}", responseJson);

            return parseResponse(responseJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Gemini request");
            throw;
        }
    }

    private List<string> ParseReflectiveQuestionsResponse(string json)
    {
        try
        {
            var root = JsonDocument.Parse(json).RootElement;
            var text = root.GetProperty("candidates")[0]
                           .GetProperty("content")
                           .GetProperty("parts")[0]
                           .GetProperty("text")
                           .GetString();

            if (text == null)
            {
                _logger.LogError("[ParseReflectiveQuestionsResponse] No text returned.");

                return [];
            }

            var questions = JsonSerializer.Deserialize<List<string>>(text);
            if (questions == null)
            {
                _logger.LogError("[ParseReflectiveQuestionsResponse] Failed to parse questions.");

                return [];
            }

            _logger.LogInformation("[ParseReflectiveQuestionsResponse] Parsed {Count} questions.", questions.Count);

            return questions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ParseReflectiveQuestionsResponse] Failed to parse response.");

            return [];
        }
    }


    private object CreateRequestBodyWithStringArraySchema(string prompt)
    {
        return new
        {
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = prompt } }
                }
            },
            generationConfig = new
            {
                response_mime_type = "application/json",
                response_schema = new
                {
                    type = "ARRAY",
                    items = new { type = "STRING" }
                }
            }
        };
    }
}
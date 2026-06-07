using System.Text;
using System.Text.Json;

namespace ArchAItect.API.Services;

public class ArchitectureService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ArchitectureService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateAsync(string requirement)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
var url =
$"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";
        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $@"
You are a software architect AI.

Return ONLY valid JSON (no markdown).

STRICT FORMAT:
{{
  ""frontend"": ""string"",
  ""backend"": ""string"",
  ""database"": ""string"",
  ""architecture"": ""string"",
  ""services"": [""service1"", ""service2""]
}}

IMPORTANT:
- services must ALWAYS be array of strings
- no objects inside services

Requirement:
{requirement}
"
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(body);

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        var result = await response.Content.ReadAsStringAsync();

        Console.WriteLine(result);

        // ❌ API FAILED CASE
        if (!response.IsSuccessStatusCode)
        {
            return @"{
                ""frontend"": ""API Error"",
                ""backend"": ""Gemini Failed"",
                ""database"": ""-"",
                ""architecture"": ""Check API Key / Quota"",
                ""services"": []
            }";
        }

        try
        {
            using var doc = JsonDocument.Parse(result);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrWhiteSpace(text))
            {
                return "{}";
            }

            // REMOVE markdown garbage
            text = text.Replace("```json", "")
                       .Replace("```", "")
                       .Trim();

            return text;
        }
        catch
        {
            return @"{
                ""frontend"": ""Parse Error"",
                ""backend"": ""Invalid Response"",
                ""database"": ""-"",
                ""architecture"": ""JSON parsing failed"",
                ""services"": []
            }";
        }
    }
}
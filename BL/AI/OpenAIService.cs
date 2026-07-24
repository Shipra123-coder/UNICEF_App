using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BL.AI
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> AskAI(string question)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var request = new
            {
                model = "gpt-4.1-mini",

                instructions = @"
                  You are an AI assistant for the UN Monitoring Dashboard.
                  
                  Your job is to:
                  1. Understand the user's question.
                  2. Extract intent and parameters.
                  3. Return ONLY valid JSON.
                  4. Never return explanations.
                  
                  JSON Format:
                  {
                    ""intent"": """",
                    ""department"": """",
                    ""department1"": """",
                    ""department2"": """",
                    ""district"": """",
                    ""year"": null,
                    ""status"": """",
                    ""chartType"": """"
                  }",
                input = question
            };
            //var request = new
            //{
            //    model = "gpt-4.1-mini",
            //    input = question
            //};

            var json = JsonSerializer.Serialize(request);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/responses",
                content);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(responseBody);
            }

            //response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}

using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MoneyMagic.GraphQL.Responses;
using OpenAI.Chat;

namespace MoneyMagic.GraphQL.Queries
{
    [QueryType]
    public class CoinAnalysisQueries
    {
        public async Task<CoinAnalysisResponse> GetCoinAnalysisOpenApi(string coinName, [Service]ILogger<CoinAnalysisQueries> logger, [Service]IConfiguration configuration)
        {
            try
            {
                logger.LogInformation("Fetching coin analysis for {CoinName} using openapi", coinName);
                var key = configuration.GetValue<string>("OpenApiKey");
                ChatClient client = new(model: "gpt-4o", key);
                var promptPath = "D:\\Projects\\dotnet\\StreamNoteSln\\src\\GraphQL\\MoneyMagic.GraphQL\\Prompts\\CoinAnalysis.txt";
                var promptContents = System.IO.File.ReadAllText(promptPath);
                var coinNameInPrompt = promptContents.Replace("{{COIN_NAME}}", coinName);
                ChatCompletion completion = await client.CompleteChatAsync(coinNameInPrompt);

                return new CoinAnalysisResponse
                {
                    FinishReason = Enum.GetName<ChatFinishReason>(completion.FinishReason)!,
                    PromptResponse = completion.Content[0].Text

                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching coin analysis for {CoinName}", coinName);
                throw;
            }
           
        }

        //do same for gemini
        public async Task<CoinAnalysisResponse> GetCoinAnalysisGemini(string coinName, [Service]ILogger<CoinAnalysisQueries> logger, [Service]IConfiguration configuration)
        {
            try
            {
                logger.LogInformation("Fetching coin analysis for {CoinName} using Gemini", coinName);
                var key = configuration.GetValue<string>("GeminiApiKey");
                var secret = configuration.GetValue<string>("GeminiApiSecret");
                var promptPath = "D:\\Projects\\dotnet\\StreamNoteSln\\src\\GraphQL\\MoneyMagic.GraphQL\\Prompts\\CoinAnalysis.txt";
                var promptContents = System.IO.File.ReadAllText(promptPath);
                var coinNameInPrompt = promptContents.Replace("{{COIN_NAME}}", coinName);

                var client = new Client(apiKey: key);
                //var models = await client.Models.ListAsync();
                var response = await client.Models.GenerateContentAsync(
                                model: "models/gemini-pro-latest",
                                contents: coinNameInPrompt);

                return new CoinAnalysisResponse
                {
                    FinishReason = Enum.GetName<FinishReason>(response.Candidates[0].FinishReason.Value),
                    PromptResponse = response.Candidates[0].Content.ToString()
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching coin analysis for {CoinName} using Gemini", coinName);
                throw;
            }

        }
    }
}

using Culinary_Bot_API.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Culinary_Bot_API.Services
{
    public class RecipeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RecipeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Spoonacular:ApiKey"];
        }

        public async Task<List<Recipe>> SearchRecipesAsync(string query)
        {
            var response = await _httpClient.GetStringAsync($"https://api.spoonacular.com/recipes/complexSearch?query={query}&addRecipeInformation=true&apiKey={_apiKey}");
            var result = JsonConvert.DeserializeObject<RecipeSearchResult>(response);
            return result.Results;
        }

        public async Task<Recipe> GetRandomRecipeAsync()
        {
            var response = await _httpClient.GetStringAsync($"https://api.spoonacular.com/recipes/random?apiKey={_apiKey}");
            var result = JsonConvert.DeserializeObject<RandomRecipeResult>(response);
            return result.Recipes[0];
        }

        public async Task<string> GetRandomFoodJokeAsync()
        {
            var response = await _httpClient.GetStringAsync($"https://api.spoonacular.com/food/jokes/random?apiKey={_apiKey}");
            var joke = JsonConvert.DeserializeObject<FoodJoke>(response);
            return joke.Text;
        }

        public async Task<string> GetRandomFoodFactAsync()
        {
            var response = await _httpClient.GetStringAsync($"https://api.spoonacular.com/food/trivia/random?apiKey={_apiKey}");
            var fact = JsonConvert.DeserializeObject<FoodFact>(response);
            return fact.Text;
        }

        public async Task<string> AnalyzeRecipeAsync(string recipeContent)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { title = recipeContent }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://api.spoonacular.com/recipes/analyze?apiKey={_apiKey}", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }

    public class RecipeSearchResult
    {
        public List<Recipe> Results { get; set; }
    }

    public class RandomRecipeResult
    {
        public List<Recipe> Recipes { get; set; }
    }

    public class FoodJoke
    {
        public string Text { get; set; }
    }

    public class FoodFact
    {
        public string Text { get; set; }
    }
}

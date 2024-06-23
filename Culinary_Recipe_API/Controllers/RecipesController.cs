using Culinary_Bot_API.Models;
using Culinary_Bot_API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Culinary_Bot_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly RecipeService _recipeService;

        public RecipesController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Recipe>>> SearchRecipes([FromQuery] string query)
        {
            var recipes = await _recipeService.SearchRecipesAsync(query);
            return Ok(recipes);
        }

        [HttpGet("random")]
        public async Task<ActionResult<Recipe>> GetRandomRecipe()
        {
            var recipe = await _recipeService.GetRandomRecipeAsync();
            return Ok(recipe);
        }

        [HttpGet("joke")]
        public async Task<ActionResult<string>> GetRandomFoodJoke()
        {
            var joke = await _recipeService.GetRandomFoodJokeAsync();
            return Ok(joke);
        }

        [HttpGet("fact")]
        public async Task<ActionResult<string>> GetRandomFoodFact()
        {
            var fact = await _recipeService.GetRandomFoodFactAsync();
            return Ok(fact);
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeRecipe([FromBody] RecipeContent recipeContent)
        {
            if (recipeContent == null || string.IsNullOrEmpty(recipeContent.Content))
            {
                return BadRequest("Recipe content cannot be null or empty.");
            }

            var analysisResult = await _recipeService.AnalyzeRecipeAsync(recipeContent.Content);
            return Ok(analysisResult);
        }
    }
public class RecipeContent
    {
        public string Content { get; set; }
    }
}

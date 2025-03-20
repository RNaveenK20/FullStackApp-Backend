using Backend.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipescrudContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public RecipeController(RecipescrudContext context)
        {
            _context = context;

            // Ensure the upload directory exists
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        // ADD NEW RECIPE (With Image Upload)
        [HttpPost("add")]
        public async Task<IActionResult> AddRecipe([FromForm] Recipe recipe, IFormFile imageFile)
        {
            if (recipe == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                // Generate a unique file name for the image
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(_uploadPath, uniqueFileName);

                // Save the image to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Store relative path in the database
                recipe.ImageUrl = "/uploads/" + uniqueFileName;
            }

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Recipe added successfully!" });
        }

        // GET ALL RECIPES
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _context.Recipes
                .Select(r => new
                {
                    r.RecipeId,
                    r.RecipeName,
                    r.Category,
                    r.Description,
                    r.ImageUrl  // Include ImageUrl in response
                }).ToListAsync();

            return Ok(recipes);
        }

        // GET RECIPE BY ID
        [HttpGet("{id}")]
        public IActionResult GetRecipeById(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == id);
            if (recipe == null)
            {
                return NotFound("Recipe not found");
            }
            return Ok(recipe);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRecipe(int id, [FromBody] Recipe updatedRecipe)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.RecipeName = updatedRecipe.RecipeName;
            recipe.Category = updatedRecipe.Category;
            recipe.Description = updatedRecipe.Description;
            recipe.Ingredients = updatedRecipe.Ingredients;
            recipe.Instructions = updatedRecipe.Instructions;

            _context.SaveChanges();
            return Ok(recipe);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound(new { message = "Recipe not found" });
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Recipe deleted successfully" });
        }

    }
}
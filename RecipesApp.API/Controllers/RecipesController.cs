using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecipesApp.API.Data;
using RecipesApp.API.Dtos;
using RecipesApp.API.Helpers;
using RecipesApp.API.Models;

namespace RecipesApp.API.Controllers
{
    [Authorize]
    [Route("/api/users/{userId}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        private Cloudinary _cloudinary;

        public RecipesController(IRecipesRepository repository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _repository = repository;
            _mapper = mapper;
           
           Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet] // Pobieranie wartości
        public async Task<IActionResult> GetRecipes([FromQuery]RecipeParams recipeParams)
        {
            
            var recipes = await _repository.GetRecipes(recipeParams);

            var recipesToReturn = _mapper.Map<IEnumerable<RecipeForListDto>>(recipes);

            Response.AddPagination(recipes.CurrentPage, 
            recipes.PageSize, 
            recipes.TotalCount, 
            recipes.TotalPages);

            return Ok(recipesToReturn);
        }

        [Route("api/recipes/{id}")]
        [HttpGet("{id}")] // Pobieranie wartości
        public async Task<IActionResult> GetRecipe(int id)
        {
            var recipe = await _repository.GetRecipe(id);

            var recipeToReturn = _mapper.Map<RecipeForDetailDto>(recipe);

            return Ok(recipeToReturn);
        }

        // http://localhost:5000/api/users/{userId}/recipes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int userId, int id)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var recipeFromRepo = await _repository.GetRecipe(id);

            recipeFromRepo.UserId = userId;

            _repository.Delete(recipeFromRepo);

            if (await _repository.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the recipe");
        }



        [HttpPut("{id}")] // Aktualizacja wartości
        // http://localhost:5000/api/users/{userId}/recipes/{id}
        public async Task<IActionResult> UpdateRecipe(int userId, int id, RecipeForUpdateDto recipeForUpdateDto)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            // tylko zalogowany użytkownik może edytować swój profil

            var recipeFromRepo = await _repository.GetRecipe(id);

            _mapper.Map(recipeForUpdateDto, recipeFromRepo);

            if (await _repository.SaveAll())
                return NoContent();

            throw new Exception($"Updating recipe {id} failed on save");

        }
        // http://localhost:5000/api/users/{userId}/recipes/{id}
         [HttpPost("{recipeId}/photos")]
        public async Task<IActionResult> AddPhotoForRecipe(int userId, int recipeId,
            [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var photoFromRepo = await _repository.GetRecipe(recipeId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<RecipePhoto>(photoForCreationDto);

            if (!photoFromRepo.RecipePhotos.Any(u => u.IsMain))
                photo.IsMain = true;

            photoFromRepo.RecipePhotos.Add(photo);

            if (await _repository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetRecipePhoto", new { recipeId = recipeId, id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }


        [HttpDelete("{recipeId}/photos/{id}")]
        public async Task<IActionResult> DeletePhoto(int recipeId, int id)
        {
           

            var recipe = await _repository.GetRecipe(recipeId);

            if (!recipe.RecipePhotos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repository.GetRecipePhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repository.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                _repository.Delete(photoFromRepo);
            }

            if (await _repository.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }



        [HttpPost("{recipeId}/photos/{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int recipeId, int id)
        {
            

            var recipe = await _repository.GetRecipe(recipeId);

            if (!recipe.RecipePhotos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repository.GetRecipePhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repository.GetMainPhotoForRecipe(recipeId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repository.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var users = await _repository.GetCategories();
            return Ok(users);
        }

    }
}

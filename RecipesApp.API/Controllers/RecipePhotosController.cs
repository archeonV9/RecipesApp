using System.Linq;
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
    [Route("api/[controller]")]
    [ApiController]
    public class RecipePhotosController : ControllerBase
    {
        
        private readonly IRecipesRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public RecipePhotosController(IRecipesRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }


            [HttpGet("{id}", Name = "GetRecipePhoto")]
            public async Task<IActionResult> GetRecipePhoto(int id)
        {
            var photoFromRepo = await _repo.GetRecipePhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
            
        }


        // [HttpPost]
        // public async Task<IActionResult> AddPhotoForRecipe(int recipeId,
        //     [FromForm]PhotoForCreationDto photoForCreationDto)
        // {
            
        //     var photoFromRepo = await _repo.GetRecipe(recipeId);

        //     var file = photoForCreationDto.File;

        //     var uploadResult = new ImageUploadResult();

        //     if (file.Length > 0)
        //     {
        //         using (var stream = file.OpenReadStream())
        //         {
        //             var uploadParams = new ImageUploadParams()
        //             {
        //                 File = new FileDescription(file.Name, stream),
        //                 Transformation = new Transformation()
        //                     .Width(500).Height(500).Crop("fill").Gravity("face")
        //             };

        //             uploadResult = _cloudinary.Upload(uploadParams);
        //         }
        //     }

        //     photoForCreationDto.Url = uploadResult.Uri.ToString();
        //     photoForCreationDto.PublicId = uploadResult.PublicId;

        //     var photo = _mapper.Map<RecipePhoto>(photoForCreationDto);

        //     if (!photoFromRepo.RecipePhotos.Any(u => u.IsMain))
        //         photo.IsMain = true;

        //     photoFromRepo.RecipePhotos.Add(photo);

        //     if (await _repo.SaveAll())
        //     {
        //         var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
        //         return CreatedAtRoute("GetRecipePhoto", new {recipeId = recipeId, id = photo.Id}, photoToReturn);
        //     }

        //     return BadRequest("Could not add the photo");
        // }
    }
}
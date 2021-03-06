using System.Linq;
using AutoMapper;
using RecipesApp.API.Dtos;
using RecipesApp.API.Dtos.Message;
using RecipesApp.API.Dtos.Recipe;
using RecipesApp.API.Models;

namespace RecipesApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
                .MapFrom(src => src.UserPhotos
                    .FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt
                .MapFrom(src => src.DateOfBirth
                    .CalculateAge()));

           CreateMap<User, UserForDetailDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
                .MapFrom(src => src.UserPhotos
                    .FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt
                .MapFrom(src => src.DateOfBirth
                    .CalculateAge()));
            
            

           CreateMap<UserPhoto, UserPhotoForDetailDto>();


           CreateMap<UserForUpdateDto, User>();  

           CreateMap<UserPhoto, PhotoForReturnDto>();

           CreateMap<PhotoForCreationDto, UserPhoto>();


           //_________________RECIPE_________________________//


           CreateMap<Recipe, RecipeForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
                .MapFrom(src => src.RecipePhotos
                    .FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Category , opt => opt
            .MapFrom(src => src.Categories.Name));
           //DETAIL
           CreateMap<Recipe, RecipeForDetailDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt
                .MapFrom(src => src.RecipePhotos
                    .FirstOrDefault(p => p.IsMain).Url));

           CreateMap<RecipePhoto, RecipePhotoForDetailDto>();  

           // CLOUDINARY //
           CreateMap<RecipePhoto, PhotoForReturnDto>(); 
           CreateMap<PhotoForCreationDto, RecipePhoto>();
           // ..........//


           CreateMap<UserForRegisterDto, User>();

           
           //CREATE
           CreateMap<RecipeForCreateDto, Recipe>();
           //UPDATE
           CreateMap<RecipeForUpdateDto, Recipe>(); 

           CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.UserPhotos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.UserPhotos.FirstOrDefault(p => p.IsMain).Url));






          

           






              
            


           

           
           
        }
    }   
}
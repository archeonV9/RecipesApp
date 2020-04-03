using System;
using System.Collections.Generic;
using RecipesApp.API.Models;

namespace RecipesApp.API.Dtos
{
    public class UserForDetailDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created {get; set;}
        public DateTime LastActive {get; set;}
        public string Introduction { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        //Aby wyświetlić main photo
        public ICollection<UserPhotoForDetailDto> UserPhotos {get; set;}
        public ICollection<RecipeForListDto> Recipes {get; set;}
        public ICollection<FavouriteRecipe> FavRecipes {get; set;}
        public ICollection<Like> Likers {get; set;}
        public ICollection<Like> Likees {get; set;}
    }
}
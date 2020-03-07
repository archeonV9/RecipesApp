using Microsoft.EntityFrameworkCore;
using RecipesApp.API.Models;

namespace RecipesApp.API.Controllers.Models.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        
            public DbSet<Value> Values {get; set;}
            public DbSet<User> Users {get; set;}
            public DbSet<UserPhoto> UserPhotos {get; set;}
            public DbSet<Recipe> Recipes {get; set;}
            public DbSet<RecipePhoto> RecipePhotos {get; set;}
            public DbSet<FavouriteRecipe> FavouriteRecipes {get; set;}

            public DbSet<Like> Likes { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<FavouriteRecipe>()
            .HasKey(k => new {k.UserId, k.RecipeId});
         
            builder.Entity<Like>()
            .HasKey(k => new {k.LikerId, k.LikeeId});

            builder.Entity<Like>()
            .HasOne(u => u.Likee)
            .WithMany(u => u.Likers)
            .HasForeignKey(u => u.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
            .HasOne(u => u.Liker)
            .WithMany(u => u.Likees)
            .HasForeignKey(u => u.LikerId)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
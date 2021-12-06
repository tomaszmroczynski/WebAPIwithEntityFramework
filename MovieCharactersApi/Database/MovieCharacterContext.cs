using Microsoft.EntityFrameworkCore;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Database
{
    public class MovieCharacterContext : DbContext
    {
        public MovieCharacterContext(DbContextOptions options) : base(options) { }

        protected MovieCharacterContext() { }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Franchise> Franchises { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Character>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Movie>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Franchise>().Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<Movie>().HasMany(x => x.CharacterList);
            builder.Entity<Character>().HasMany(x => x.MovieList);
            builder.Entity<Franchise>().HasMany(x => x.MovieList);
            
        }

    }
}
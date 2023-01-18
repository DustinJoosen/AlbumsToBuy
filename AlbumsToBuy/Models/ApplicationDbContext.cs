using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Album> Albums{ get; set; }
		public DbSet<AlbumOrder> AlbumOlders{ get; set; }
		public DbSet<ShoppingListItem> ShoppingListItems{ get; set; }
		public DbSet<Order> Orders{ get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Track> Tracks { get; set; }
		public DbSet<User> Users{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Album>().HasData(new List<Album>()
			{
				new Album
				{
					Id = 1,
					Name = "Testing Album 1",
					Creator = "Testing Creator 1",
					Price = 1.28m,
					Stock = 42,
					ReleaseDate = DateTime.Today,
					CoverImage = "NotFound.png",
					Type = AlbumType.LP
				},
				new Album
				{
					Id = 2,
					Name = "Testing Album 2",
					Creator = "Testing Creator 2",
					Price = 1.28m,
					Stock = 42,
					ReleaseDate = DateTime.Today,
					CoverImage = "NotFound.png",
					Type = AlbumType.LP
				}
			});

			modelBuilder.Entity<Track>().HasData(new List<Track>()
			{
				new Track
				{
					Id = 1,
					AlbumId = 1,
					Name = "Testing Album 1 track 1",
					Duration = 184
				},
				new Track
				{
					Id = 2,
					AlbumId = 1,
					Name = "Testing Album 1 track 2",
					Duration = 187
				},
				new Track
				{
					Id = 3,
					AlbumId = 1,
					Name = "Testing Album 1 track 3",
					Duration = 168
				},
				new Track
				{
					Id = 4,
					AlbumId = 2,
					Name = "Testing Album 2 track 1",
					Duration = 205
				},
				new Track
				{
					Id = 5,
					AlbumId = 2,
					Name = "Testing Album 2 track 2",
					Duration = 192
				},
			});
        }
    }
}

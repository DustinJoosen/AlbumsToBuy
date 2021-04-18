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
	}
}

using back_end.Entities;
using Microsoft.EntityFrameworkCore;

namespace back_end.Data
{
    public class TripContext:DbContext
	{
		public TripContext(DbContextOptions options) :base(options)
		{

		}

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}


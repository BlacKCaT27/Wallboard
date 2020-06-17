using Bcss.Wallboard.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bcss.Wallboard.Api.Data.EfCore.Contexts
{
    public class SlideContext : DbContext
    {
        public DbSet<Slide> Slides { get; set; }

        public SlideContext(DbContextOptions<SlideContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Slide>().ToTable("Slide");
        }
    }
}
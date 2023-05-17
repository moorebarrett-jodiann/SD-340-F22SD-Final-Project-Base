using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SD_340_W22SD_Final_Project_Group6.Models;

namespace SD_340_W22SD_Final_Project_Group6.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Ticket>()
				.HasMany(t => t.TicketWatchers)
				.WithOne(tw => tw.Ticket)
				.HasForeignKey(tw => tw.TicketId)
				.OnDelete(DeleteBehavior.NoAction); ;

			modelBuilder.Entity<ApplicationUser>()
				.HasMany(au => au.TicketWatching)
				.WithOne(tw => tw.Watcher)
				.HasForeignKey(tw => tw.WatcherId)
				.OnDelete(DeleteBehavior.NoAction); ;
		}

		public DbSet<SD_340_W22SD_Final_Project_Group6.Models.Ticket> Tickets { get; set; }
        public DbSet<SD_340_W22SD_Final_Project_Group6.Models.Project> Projects { get; set; }
        public DbSet<SD_340_W22SD_Final_Project_Group6.Models.Comment> Comments { get; set; }
        public DbSet<SD_340_W22SD_Final_Project_Group6.Models.UserProject> UserProjects { get; set; }
        public DbSet<SD_340_W22SD_Final_Project_Group6.Models.TicketWatcher> TicketWatchers { get; set; }

    }
}
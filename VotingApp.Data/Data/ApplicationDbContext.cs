using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VotingApp.Data.Models;

namespace VotingApp.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Party> Parties { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
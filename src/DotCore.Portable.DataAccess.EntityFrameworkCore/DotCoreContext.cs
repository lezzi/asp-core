using DotCore.Portable.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotCore.Portable.DataAccess.EntityFrameworkCore
{
    public class DotCoreContext : IdentityDbContext<User>
    {
        #region Properties, Indexers

        public DbSet<Question> Questions => Set<Question>();

        public DbSet<Answer> Answers => Set<Answer>();

        #endregion

        #region Constructors

        public DotCoreContext()
        {
        }

        public DotCoreContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region Event Handlers

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Question>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>()
                .HasOne(x => x.AcceptedAnswer)
                .WithOne()
                .HasForeignKey<Question>(x => x.AcceptedAnswerId);

            modelBuilder.Entity<Question>()
                .HasMany(x => x.Answers)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Answer>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        #endregion
    }
}
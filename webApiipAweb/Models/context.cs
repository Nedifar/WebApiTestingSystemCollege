using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class context : IdentityDbContext<
        Child, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public context(DbContextOptions<context> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Child>(b =>
            {
                b.HasMany(e => e.Claims)
               .WithOne()
               .HasForeignKey(uc => uc.UserId)
               .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();
                b.HasMany(e => e.UserRoles)
               .WithOne(e => e.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<AnswearOnTask> AnswearOnTasks { get; set; }
        public DbSet<ThingPackExecution> ThingPackExecutions { get; set; }
        public DbSet<ThingExecution> ThingExecutions { get; set; }
        public DbSet<ThingPack> ThingPacks { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Thing> Things { get; set; }
        public DbSet<Achivment> Achivments { get; set; }
        public DbSet<AchivmentExecution> AchivmentExecutions { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ChapterExecution> ChapterExecutions { get; set; }
        public DbSet<LevelStuding> LevelStudings { get; set; }
        public DbSet<LevelStudingExecution> LevelStudingExecutions { get; set; }
        public DbSet<SubjectExecution> SubjectExecutions { get; set; }
        public DbSet<TaskWithOpenAnsw> TaskWithOpenAnsws { get; set; }
        public DbSet<TaskWithClosedAnsw> TaskWithClosedAnsw { get; set; }
        public DbSet<TestTask> TestTasks { get; set; }
        public DbSet<TestTaskExecution> TestTaskExecutions { get; set; }
        public DbSet<TryingTestTask> TryingTestTasks { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<TaskWithOpenAnswsExecution> TaskWithOpenAnswsExecutions { get; set; }
        public DbSet<TaskWithClosedAnswsExecution> TaskWithClosedAnswsExecutions { get; set; }
        public DbSet<Appeal> Appeals { get; set; }
        public DbSet<TypeAppeal> TypeAppeals { get; set; }
        public DbSet<TestPack> TestPacks { get; set; }
        public DbSet<TestPackExecution> TestPackExecutions { get; set; }
        public DbSet<SessionProgress> SessionProgresses { get; set; }
        public DbSet<SessionChapterExecution> SessionChapterExecutions { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<TheorySession> TheorySessions { get; set; }
    }
}

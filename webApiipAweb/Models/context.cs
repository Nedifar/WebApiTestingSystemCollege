using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class context : IdentityDbContext<Child>
    {
        public context(DbContextOptions<context> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<AnswearOnTask> AnswearOnTasks { get; set; }
        public DbSet<ThingPackExecution> ThingPackExecutions { get; set; }
        public DbSet<ThingExecution> ThingExecutions{ get; set; }
        public DbSet<ThingPack> ThingPacks{ get; set; }
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
    }
}

//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.Models;
using SkillsHub.Domain.BaseModels;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Diagnostics.Metrics;

namespace SkillsHub.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ExternalConnection> ExternalConnections { get; set; }
    public DbSet<UserDaySchedule> DaySchedules { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<LessonType> LessonTypes { get; set; }
    public DbSet<LessonActivityType> LessonActivityTypes { get; set; }

    public DbSet<LessonStudent> LessonStudents { get; set; }


    public DbSet<Cource> Cources { get; set; }

    public DbSet<EnglishLevel> EnglishLevels { get; set; }
    public DbSet<CourceName> CourceNames { get; set; }

    public DbSet<LessonTask> LessonTasks { get; set; }
    public DbSet<EmailMessage> EmailMessages { get; set; }
    public DbSet<ScheduleDay> ScheduleDays { get; set; }
    public DbSet<FinanceElement> Finances { get; set; }

    public DbSet<WorkingDay> WorkingDays { get; set; }

    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        //builder.Entity<Lesson>().HasOne(a => a.LessonTask).WithOne(a => a.Lesson).HasForeignKey<LessonTask>(c => c.LessonId);

        //builder.Entity<LessonStudent>().HasNoKey();
        //builder.Entity<GroupStudent>().HasNoKey();
        //builder.Entity<Group>().HasMany(x => x.ArrivedStudents).WithOne(x => x.Group).OnDelete(DeleteBehavior.SetNull);
        builder.Entity<Lesson>().HasMany(x => x.ArrivedStudents).WithOne(x => x.Lesson).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<ApplicationUser>().HasOne(x => x.UserStudent).WithOne(x => x.ApplicationUser).HasForeignKey<Student>(x=>x.ApplicationUserId);
        builder.Entity<ApplicationUser>().HasOne(x => x.UserTeacher).WithOne(x => x.ApplicationUser).HasForeignKey<Teacher>(x => x.ApplicationUserId);

        //builder.Entity<Teacher>().HasMany(x => x.Groups).WithOne(x => x.Teacher).OnDelete(DeleteBehavior.SetNull);
        //builder.Entity<Teacher>().HasMany(x=>x.Groups).WithOne(x=>x.Teacher).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Student>().HasMany(x => x.Groups).WithMany(x => x.GroupStudents);


        base.OnModelCreating(builder);
    }


}
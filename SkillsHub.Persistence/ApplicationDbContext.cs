//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.Models;
using SkillsHub.Domain.BaseModels;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Diagnostics.Metrics;
using SkillsHub.Domain.Models.NotInUse;

namespace SkillsHub.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ExternalConnection> ExternalConnections { get; set; }
    public DbSet<WorkingDay> DaySchedules { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<LessonType> LessonTypes { get; set; }
    public DbSet<LessonActivityType> LessonActivityTypes { get; set; }

    public DbSet<LessonStudent> LessonStudents { get; set; }


    //public DbSet<Cource> Cources { get; set; }

    //public DbSet<EnglishLevel> EnglishLevels { get; set; }
    public DbSet<CourseName> CourseNames { get; set; }

    public DbSet<LessonTask> LessonTasks { get; set; }

    public DbSet<EmailMessage> EmailMessages { get; set; }
    //public DbSet<ScheduleDay> ScheduleDays { get; set; }
    public DbSet<FinanceElement> Finances { get; set; }

    public DbSet<WorkingDay> WorkingDays { get; set; }
    public DbSet<NotificationMessage> NotificationMessages { get; set; }

    public DbSet<CourseNameTeacher> CourseNameTeachers { get; set; }
    public DbSet<CourseNameStudent> CourseNameStudents { get; set; }

    public DbSet<RequestLesson> RequestLessons { get; set; }

    public DbSet<GroupStudent> GroupStudents { get; set; }

    public DbSet<NotificationUser> NotificationUsers { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();


        builder.Entity<ApplicationUser>().HasOne(x=>x.UserTeacher).WithOne(x => x.ApplicationUser).HasForeignKey<Teacher>(x => x.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ApplicationUser>().HasOne(x => x.UserStudent).WithOne(x => x.ApplicationUser).HasForeignKey<Student>(x => x.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Teacher>().HasMany(x => x.Groups).WithOne(x => x.Teacher).OnDelete(DeleteBehavior.SetNull);
        //builder.Entity<Student>().HasMany(x => x.Groups).WithMany(x => x.GroupStudents).OnDelete(DeleteBehavior.SetNull);
        //builder.Entity<Student>().HasMany(x => x.Groups).WithMany(x => x.GroupStudents);



        builder.Entity<WorkingDay>().HasOne(x => x.Group).WithMany(x => x.DaySchedules).OnDelete(DeleteBehavior.NoAction);
        //builder.Entity<WorkingDay>().HasOne(x => x.Student).WithMany(x => x.WorkingDays).OnDelete(DeleteBehavior.NoAction);
        //builder.Entity<WorkingDay>().HasOne(x => x.Teacher).WithMany(x => x.WorkingDays).OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Group>().HasOne(x => x.Teacher).WithMany(x => x.Groups).OnDelete(DeleteBehavior.SetNull);



        builder.Entity<Lesson>().HasMany(x => x.ArrivedStudents).WithOne(x => x.Lesson).OnDelete(DeleteBehavior.SetNull);


        //builder.Entity<EnglishLevel>().HasMany(x=>x.Users).WithOne(x=>x.EnglishLevel).HasForeignKey(x=>x.EnglishLevelId);



        builder.Entity<CourseNameTeacher>()
       .HasKey(ct => new { ct.CourseNameId, ct.TeacherId });

        builder.Entity<CourseNameTeacher>()
            .HasOne(ct => ct.CourseName)
            .WithMany(c => c.Teachers)
            .HasForeignKey(ct => ct.CourseNameId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for CourceNameTeacher

        builder.Entity<CourseNameTeacher>()
            .HasOne(ct => ct.Teacher)
            .WithMany(t => t.PossibleCources)
            .HasForeignKey(ct => ct.TeacherId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for CourceNameTeacher

        builder.Entity<CourseNameStudent>()
       .HasKey(ct => new { ct.CourseNameId, ct.StudentId });

        builder.Entity<CourseNameStudent>()
            .HasOne(ct => ct.CourseName)
            .WithMany(c => c.Students)
            .HasForeignKey(ct => ct.CourseNameId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for CourceNameTeacher

        builder.Entity<CourseNameStudent>()
            .HasOne(ct => ct.Student)
            .WithMany(t => t.PossibleCources)
            .HasForeignKey(ct => ct.StudentId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete for CourceNameTeacher


        builder.Entity<LessonStudent>()
            .HasOne(x => x.Student)
            .WithMany(x => x.Lessons)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LessonStudent>()
            .HasOne(x => x.Lesson)
            .WithMany(x => x.ArrivedStudents)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<GroupStudent>()
            .HasOne(x => x.Student)
            .WithMany(x => x.Groups)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<GroupStudent>()
            .HasOne(x => x.Group)
            .WithMany(x => x.GroupStudents)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<NotificationUser>()
            .HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<NotificationUser>()
            .HasOne(x => x.NotificationMessage)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);




        base.OnModelCreating(builder);
    }


}
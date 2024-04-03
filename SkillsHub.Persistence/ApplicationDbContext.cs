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
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;

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

    //public DbSet<LessonActivityType> LessonActivityTypes { get; set; }

    public DbSet<LessonStudent> LessonStudents { get; set; }
    public DbSet<LessonTask> LessonTasks { get; set; }

    public DbSet<EmailMessage> EmailMessages { get; set; }
    //public DbSet<ScheduleDay> ScheduleDays { get; set; }
    public DbSet<FinanceElement> Finances { get; set; }

    public DbSet<WorkingDay> WorkingDays { get; set; }
    public DbSet<NotificationMessage> NotificationMessages { get; set; }

    public DbSet<PossibleCourseTeacher> PossibleCourseTeacher { get; set; }
    public DbSet<PreferenceCourseStudent> PreferenceCourseStudent { get; set; }
    public DbSet<RequestLesson> RequestLessons { get; set; }
    public DbSet<GroupStudent> GroupStudents { get; set; }
    public DbSet<NotificationUser> NotificationUsers { get; set; }
    public DbSet<PaymentCategory> PaymentCategories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<AgeType> AgeTypes { get; set; }
    public DbSet<GroupTeacher> GroupTeachers { get; set; }
    public DbSet<GroupType> GroupTypes { get; set; }

    public DbSet<Subject> Subjects { get; set; }
    public DbSet<LocationType> LocationTypes { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();

        builder.Entity<ApplicationUser>().HasOne(x=>x.UserTeacher).WithOne(x => x.ApplicationUser).HasForeignKey<Teacher>(x => x.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ApplicationUser>().HasOne(x => x.UserStudent).WithOne(x => x.ApplicationUser).HasForeignKey<Student>(x => x.ApplicationUserId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WorkingDay>().HasOne(x => x.Group).WithMany(x => x.DaySchedules).OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Lesson>().HasMany(x => x.ArrivedStudents).WithOne(x => x.Lesson).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Course>().HasOne(x => x.Subject).WithMany(x => x.Courses).HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.NoAction);        
        #region Many-to-many

             // Cascade delete for CourceNameTeacher
        builder.Entity<PossibleCourseTeacher>().HasKey(ct => new { ct.CourseId, ct.TeacherId , ct.LocationTypeId});
        builder.Entity<PossibleCourseTeacher>().HasOne(ct => ct.Course).WithMany(c => c.PossibleCourseTeachers).HasForeignKey(ct => ct.CourseId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PossibleCourseTeacher>().HasOne(ct => ct.Teacher).WithMany(t => t.PossibleCources).HasForeignKey(ct => ct.TeacherId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PossibleCourseTeacher>().HasOne(ct => ct.LocationType).WithMany(t => t.PossibleCourseTeachers).HasForeignKey(ct => ct.LocationTypeId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PreferenceCourseStudent>().HasKey(ct => new { ct.CourseId, ct.StudentId, ct.LocationTypeId });
        builder.Entity<PreferenceCourseStudent>().HasOne(ct => ct.Course).WithMany(c => c.PreferenceCourseStudents).HasForeignKey(ct => ct.CourseId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PreferenceCourseStudent>().HasOne(ct => ct.Student).WithMany(t => t.PossibleCources).HasForeignKey(ct => ct.StudentId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PreferenceCourseStudent>().HasOne(ct => ct.LocationType).WithMany(t => t.PreferenceCourseStudents).HasForeignKey(ct => ct.LocationTypeId).OnDelete(DeleteBehavior.Cascade);


        //При удалении Teacher, Group удалится и GroupTeacher
        builder.Entity<GroupTeacher>().HasKey(ct => new { ct.GroupId, ct.TeacherId });
        builder.Entity<GroupTeacher>().HasOne(x => x.Teacher).WithMany(x => x.GroupTeachers).HasForeignKey(x=>x.TeacherId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<GroupTeacher>().HasOne(x => x.Group).WithMany(x => x.GroupTeachers).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);



        builder.Entity<LessonStudent>().HasOne(x => x.Student).WithMany(x => x.Lessons).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<LessonStudent>().HasOne(x => x.Lesson).WithMany(x => x.ArrivedStudents).HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<GroupStudent>().HasOne(x => x.Student).WithMany(x => x.Groups).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Cascade);

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



        #endregion

        base.OnModelCreating(builder);
    }


}
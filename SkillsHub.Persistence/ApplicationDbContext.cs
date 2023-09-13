//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.DTO;
using SkillsHub.Domain.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SkillsHub.Persistence;

public class ApplicationDbContext : IdentityDbContext<SkillsHub.Domain.BaseModels.ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<StudentDTO> Students { get; set; }
    public DbSet<TeacherDTO> Teachers { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonType> LessonTypes { get; set; }


}
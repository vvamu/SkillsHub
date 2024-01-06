using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class CourseName : BaseModels.BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public int MinimumAge { get; set; }
    public int MaximumAge { get; set; }

    public List<CourseNameStudent> Students { get; set; }
    public List<CourseNameTeacher> Teachers { get; set; }

}


using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class CourseAgeType
{
    public Guid Id { get; set; }
    public AgeType? AgeType { get; set; }
    public Guid AgeTypeId { get; set; }
    public Course? Course { get; set; }
    public Guid? CourseId { get; set; }


}
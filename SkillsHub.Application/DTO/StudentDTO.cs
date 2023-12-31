﻿using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.DTO;

public class StudentDTO : ApplicationUser
{
    public Guid UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public List<Lesson> StudentLessons { get; set; }
    public List<Lesson> TeacherLessons { get; set; }

    public string? ParentPhone { get; set; }
    public string? ParentName { get; set; }

    public string? EnglishLevel { get; set; }
    public string IsRecorded { get; set; }
    public int CountPayedLessons { get; set; }
    public int VisitedLessons { get; set; }


    public List<Group>? Groups { get; set; }


    public List<CourseName>? PossibleCources { get; set; }
    public List<WorkingDay>? WorkingDays { get; set; }


}
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

public class SalaryService : ISalaryService
{
    private readonly ApplicationDbContext _context;
    private readonly IGroupService _groupService;

    public SalaryService(ApplicationDbContext context, IGroupService groupService)
    {
        _context = context;
        _groupService = groupService;
    }

    public async Task<decimal> GetStudentSalaryAsync(Student student, bool withNotCompletedLessons = false, bool byMonth = false)
    {
        decimal res = 0;
        if (student.Groups == null || student.Groups.Count() == 0) return res;

        List<Group> groups = new List<Group>();

        foreach (var gr in student.Groups.Select(x=>x.Group))
        {
            groups.Add(await _groupService.GetAsync(gr.Id));
        }

        _context.LessonStudents.ToList();

        
        foreach (var group in groups)
        {
            decimal pricePerLesson = 0;
            if (group.PaymentCategory == null || group.Lessons == null || group.Lessons.Count() == 0) continue;
            pricePerLesson = group.PaymentCategory.StudentPrice;
            int countPaidLessons = 0;
            foreach (var lesson in group.Lessons.Where(x =>x.ArrivedStudents != null && x.ArrivedStudents.Select(x => x.Student.Id).Contains(student.Id)))
            {
                if (byMonth && lesson.StartTime.Month != DateTime.Now.Month) continue;

                if (withNotCompletedLessons || lesson.IsСompleted) countPaidLessons += 1;
            }
            res += pricePerLesson * countPaidLessons;

        }
        //foreach(var lesson in _context.LessonStudents.Where(x=>x.Student.Id == student.Id).Select(x=>x.Lesson)

        return res;

    }

    public async Task<decimal> GetTeacherSalaryAsync(Teacher teacher, bool withNotCompletedLessons = false, bool byMonth = false)
    {
        decimal res = 0;
        if (teacher.GroupTeachers == null || teacher.GroupTeachers.Count() == 0) return res;

        List<Group> groups = new List<Group>();
        foreach (var gr in teacher.GroupTeachers)
        {
            //var g = _context.Groups
            //.Include(x => x.Lessons).ThenInclude(x => x.Teacher).Where(x=>x.Lessons != null).Where(x=>x.Lessons.Select(x=>x.Teacher.Id) == teacher.Id);

            groups.Add(await _groupService.GetAsync(gr.Id));
        }

        _context.Teachers.ToList();

        foreach (var group in groups)
        {
            decimal pricePerLesson = 0;
            if (group.PaymentCategory == null || group.Lessons == null || group.Lessons.Count() == 0) continue;
            pricePerLesson = group.PaymentCategory.TeacherPrice;
            int countPaidLessons = 0;
            foreach (var lesson in group.Lessons.Where(x=> x.Teacher != null && x.Teacher.Id == teacher.Id))
            {
                //var les = await _context.Lessons.Include(x=>x.Teacher).FirstOrDefaultAsync(x=>x.Id == lesson.Id);
                //if (les.Teacher != null && les.Teacher.Id == teacher.Id) continue;
                if (byMonth && lesson.StartTime.Month != DateTime.Now.Month) continue;

                if (withNotCompletedLessons || lesson.IsСompleted)
                    countPaidLessons++;
            }
            res += pricePerLesson * countPaidLessons;

        }
        return res;
    }
}

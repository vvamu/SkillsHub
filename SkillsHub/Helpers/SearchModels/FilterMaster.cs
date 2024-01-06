using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using System.Linq;

namespace SkillsHub.Helpers.SearchModels;
public static class FilterMaster
{
    public static async Task<IQueryable<Student>> GetAllStudents(IQueryable<Student> items, StudentFilterModel filters, StudentOrderModel orders)
    {

        if (filters != null)
        {
            if (filters.ApplicationUserId != Guid.Empty)
                items = items.Where(x => x.Id == filters.ApplicationUserId);
            if (!string.IsNullOrEmpty(filters.ParentName))
                items = items.Where(x => x.ParentName.Contains(filters.ParentName));
            if (!string.IsNullOrEmpty(filters.ParentPhone))
                items = items.Where(x => x.ParentPhone.Contains(filters.ParentPhone));
            if (filters.MinCountPayedLessons != 0)
                items = items.Where(x=>x.CountPayedLessons != null).Where(x => x.CountPayedLessons >= filters.MinCountPayedLessons);
            if (filters.MinCountGroups != 0)
                items = items.Where(x => x.Groups != null).Where(x => x.Groups.Count >= filters.MinCountGroups);
            if (filters.GroupId != Guid.Empty)
                items = items.Where(x => x.Groups != null).Where(x => x.Groups.Select(x=>x.Id).Contains(filters.GroupId));
            if (!string.IsNullOrEmpty(filters.WorkingDay))           
                items = items.Where(x => x.WorkingDays != null)
                    .Where(x => x.WorkingDays.Contains(filters.WorkingDay));
            if (!string.IsNullOrEmpty(filters.PossibleCourse))
                items = items.Where(x => x.PossibleCources != null)
                    .Where(x => x.PossibleCources.Select(x => x.CourseName.Name).Contains(filters.PossibleCourse));
            if(filters.IsDeleted != -100)
                items = items.Where(x=>x.IsDeleted == Convert.ToBoolean(filters.IsDeleted));
            if (filters.MinDateCreated != null)
                items = items.Where(x => x.DateCreated >= filters.MinDateCreated);

        }
        if(orders !=  null)
        {
            if (orders.CountPayedLessons != -100)
            {
                if (orders.CountPayedLessons >= 0)
                    items.OrderBy(x => x.CountPayedLessons);
                else
                    items.OrderByDescending(x => x.CountPayedLessons);
            }
            /*
            if(orders.Groups != null)
            {
                if (orders.Groups.Count >= 0)
                    items.Where(x=>x.Groups!=null).OrderBy(x => x.Groups.Count);
                else
                    items.Where(x => x.Groups != null).OrderByDescending(x => x.Groups.Count);
            }
            

            if (orders.CountCources != -100)
            {
                if (orders.CountCources >= 0)
                    items.Where(x => x.PossibleCources != null).OrderBy(x => x.PossibleCources.Count);
                else
                    items.Where(x => x.PossibleCources != null).OrderByDescending(x => x.PossibleCources.Count);
            }
            */
            /*
            if (orders.DateCreated != null)
            {
                if (orders.DateCreated >= DateTime.Now)
                    items.OrderBy(x => x.DateCreated);
                else
                    items.OrderByDescending(x => x.DateCreated);
            }
            */

        }

        return items;
    }

    public static async Task<IQueryable<Teacher>> GetAllTeachers(IQueryable<Teacher> items, TeacherFilterModel filters, TeacherOrderModel orders)
    {
        if (filters != null)
        {
            if (filters.ApplicationUserId != Guid.Empty)
                items = items.Where(x => x.Id == filters.ApplicationUserId);
            /*
            if (filters.MinSalary != 0)
                items = items.Where(x => x.Salary >= filters.MinSalary);
            if (filters.MaxSalary != 0)
                items = items.Where(x => x.Salary <= filters.MaxSalary);
            */
            if (!string.IsNullOrEmpty(filters.PossibleCourse))
                items = items.Where(x => x.PossibleCources != null)
                             .Where(x => x.PossibleCources.Select(c => c.CourseName.Name).Contains(filters.PossibleCourse));
            if (filters.GroupId != Guid.Empty)
                items = items.Where(x => x.Groups != null)
                             .Where(x => x.Groups.Select(g => g.Id).Contains(filters.GroupId));
            if (!string.IsNullOrEmpty(filters.WorkingDay))
                items = items.Where(x => x.WorkingDays != null)
                             .Where(x => x.WorkingDays.Contains(filters.WorkingDay));
            if (filters.IsDeleted != -100)
                items = items.Where(x => x.IsDeleted == Convert.ToBoolean(filters.IsDeleted));
        }

        return items;
    }

    public static async Task<IQueryable<Lesson>> GetAllLessons(IQueryable<Lesson> items, LessonFilterModel filters, LessonOrderModel orders)
    {
        if (filters != null)
        {
            if (filters.TeacherId != Guid.Empty)
                items = items.Where(x => x.Group.Teacher.Id == filters.TeacherId);
            if(filters.Topic != "" || String.IsNullOrEmpty(filters.Topic))
            {
                items = items.Where(x => x.Topic.Contains(filters.Topic));
            }
            /*
            if (filters.MinSalary != 0)
                items = items.Where(x => x.Salary >= filters.MinSalary);
            if (filters.MaxSalary != 0)
                items = items.Where(x => x.Salary <= filters.MaxSalary);
            */

          
        }
        if(orders != null)
        {
            if (orders.DateCreated != -100)
            {
                if (orders.CountPayedLessons >= 0)
                    items.OrderBy(x => x.DateCreated);
                else
                    items.OrderByDescending(x => x.DateCreated);
            }
        }

        return items;
    }
}

/*
 *         
 *         var p = HttpContext.Request.QueryString.Value ?? "";
        var result = ParseQueryString(p);
     static Dictionary<string, string> ParseQueryString(string url)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        int questionMarkIndex = url.IndexOf('?');
        if (questionMarkIndex != -1)
        {
            string queryString = url.Substring(questionMarkIndex + 1);
            string[] pairs = queryString.Split('&');

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    string value = keyValue[1];
                    result[key] = value;
                }
            }
        }

        return result;
    }
*/
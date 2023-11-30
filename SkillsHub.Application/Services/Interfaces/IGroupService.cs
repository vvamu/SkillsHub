using SkillsHub.Application.PageViewModel;
using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Interfaces;

public interface IGroupService
{
    public IQueryable<Group> GetAll();
    public Task<Group> GetAsync(Guid id);
    public Task<Group> CreateAsync(Group item);
    public Task<Group> AddStudentsToGroupAsync(Guid groupId, List<Guid> studentsId);
    public Task<Group> AddTeacherToGroupAsync(Guid groupId, Guid teacherId);


}

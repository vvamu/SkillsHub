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
    public Task<Group> CreateAsync(Group item);

}

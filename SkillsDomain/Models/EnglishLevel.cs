using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class EnglishLevel : BaseModels.BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }

    //public List<ApplicationUser>? Users { get; set; }
   

}


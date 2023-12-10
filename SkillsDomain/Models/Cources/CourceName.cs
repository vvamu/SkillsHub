using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models;

public class CourceName : BaseModels.BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public int MinimumAge { get; set; }
    public int MaximumAge { get; set; }

	public DateTime DateCreated { get; set; }
}


using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Domain.Models
{
    public class FinanceElement : BaseEntity
    {
        public string Name { get; set; }
        public FinanceElement? Parent { get; set; }
        public Guid? ParentId { get; set; }
        public decimal? Budget { get; set; }
    }
}

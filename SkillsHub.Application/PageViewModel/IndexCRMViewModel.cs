using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.PageViewModel;

public class IndexCRMViewModel
{
    public int CountClasses { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalStudents { get; set; }
    public int TotalUsers { get; set; }
    
    public int CountMails { get; set; }
    public int CountUnVerifiedUsers { get; set; }

}

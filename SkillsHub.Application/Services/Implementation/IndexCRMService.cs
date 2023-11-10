using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.PageViewModel;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Implementation;

public class IndexCRMService : IIndexCRMService
{
    private readonly ApplicationDbContext _context;

    public IndexCRMService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IndexCRMViewModel> IndexAdmin()
    {
        var viewModel = new IndexCRMViewModel()
        {
            TotalTeachers = _context.Teachers.Count(),
            TotalStudents = _context.Students.Count(),
            TotalUsers = _context.Users.Count(),
            CountClasses = _context.Teachers.Include(x => x.Lessons).Count(),
            CountMails = _context.EmailMessages.Count(),
        };
        return viewModel;

    }
}

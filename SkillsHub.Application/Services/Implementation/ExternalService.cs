using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Implementation;

public class ExternalService : IExternalService
{
    private readonly ApplicationDbContext _context;

    public ExternalService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<EmailMessage> SaveMessage(EmailMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        await _context.EmailMessages.AddAsync(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<IQueryable<EmailMessage>> GetMessages()
    {
        return _context.EmailMessages.OrderBy(x => x.Date);
    }


}

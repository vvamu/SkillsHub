using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Interfaces;

public interface IExternalService
{
    public Task<EmailMessage> SaveMessage(EmailMessage message);
    public Task<IQueryable<EmailMessage>> GetMessages();
}

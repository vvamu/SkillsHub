using SkillsHub.Application.PageViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Interfaces;

public interface IIndexCRMService
{
    public Task<IndexCRMViewModel> IndexAdmin();
}

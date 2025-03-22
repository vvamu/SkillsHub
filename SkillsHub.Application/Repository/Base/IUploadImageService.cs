using SkillsHub.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Repository.Base;

public interface IUploadImageService<T> where T : IUploadImageModel, new()
{
    public Task<string> UploadImage(T item);

}

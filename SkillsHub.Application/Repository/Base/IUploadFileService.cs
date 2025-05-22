using Microsoft.AspNetCore.Http;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Repository.Base;

public interface IUploadFileService<T> where T : IUploadImageModel, new()
{
    //public Task<string> UploadImage(T item);
    //public Task<string> UploadAsync(T item);
    //public Task<byte[]> DownloadAsync(string imageName);
    public Task<string> UploadFileAsync (T item);

}


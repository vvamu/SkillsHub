using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Application.Repository.Base;

public interface IUploadIconService<T> where T : IUploadImageModel, new()
{
    //public Task<string> UploadImage(T item);
    //public Task<string> UploadAsync(T item);
    //public Task<byte[]> DownloadAsync(string imageName);
    public Task<string> UploadIconAsync(T item);

}


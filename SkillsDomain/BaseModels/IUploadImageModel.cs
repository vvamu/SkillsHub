using Microsoft.AspNetCore.Http;

namespace SkillsHub.Domain.BaseModels;

public interface IUploadImageModel
{
    public string PathToImage { get; set; }

    public List<IFormFile>? ImagesFiles { get; set; }

}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.BaseModels;

public interface IUploadImageModel
{
    public string PathToImage { get; set; }

    [NotMapped]
    public List<IFormFile>? ImagesFiles { get; set; }
}

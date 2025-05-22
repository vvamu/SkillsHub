//using CG.Web.MegaApiClient;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using SkillsHub.Domain.Models;
//using SkillsHub.Persistence;

//namespace SkillsHub.Application.Helpers;

//public class DatabaseFileUploader<T> : FileUploader where T : Course
//{
//    private readonly ApplicationDbContext _context;
//    private readonly T _obj;
//    private  DbSet<Course> _contextModel { get; }


//    public DatabaseFileUploader(ApplicationDbContext context, T obj)
//    {
//        _context = context;
//        _obj = obj;
//        _contextModel = _context.Courses;

//    }
//    public override async Task<byte[]> GetAsync(string fileName)
//    {
//        throw new NotImplementedException();
//    }

//    protected override async Task CheckValueIsNew(IFormFile fileToUpload, string? oldFilePath)
//    {
//        var oldFileLength = _obj.ImageBytes?.Length;
//        if (oldFileLength == 0 || fileToUpload.Length == 0) return;

//        if (fileToUpload.Length == oldFileLength) throw new Exception("Uploaded file have equals size with already uploaded");

//    }

//    protected override async Task<object> GetRootPath(object localPathFolders)
//    {
//        return "";
//    }

//    protected override async Task<string> UploadFile(IFormFile fileToUpload, string fileName, object? path)
//    {
        
//    }
//}


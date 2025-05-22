using CG.Web.MegaApiClient;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestSharp.Extensions;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Repository.Base;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators.LessonTypeModels;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using System.IO;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class CourseService :  AbstractLessonTypeLogModelService<Course>, ICourseService
{
    private static FileUploader _fileUploader;
    private static FileUploader FileUploader { get { return _fileUploader == null ? _fileUploader = new MegaFileUploader() : _fileUploader; } }

    protected override DbSet<Course> _contextModel => _context.Courses;

    protected override IValidator<Course> _validator => new CourseValidator();

    protected override IQueryable<Course>? _fullInclude => _context.Courses;

    public CourseService(ApplicationDbContext context, ILessonTypeService lessonTypeService) : base(context, lessonTypeService) { }

    public override async Task<Course> GetLastValueAsync(Guid? itemId, bool withParents = false, bool touchFullInclude = true)
    {
        var res = await base.GetLastValueAsync(itemId, withParents, touchFullInclude);
        //var iconPath = res.PathToIcon; 
        //if (!string.IsNullOrEmpty(iconPath) && iconPath.Contains("mega"))
        //    res.IconBytes = await FileUploader.GetAsync(iconPath);
        //var imagePath = res.PathToImage;
        //if (!string.IsNullOrEmpty(imagePath) && imagePath.Contains("mega")) 
        //    res.ImageBytes = await FileUploader.GetAsync(imagePath);
        return res;
    }

    protected override void SetPropertyId(LessonType item, Guid value)
    {
        item.CourseId = value;
       
    }

    public override async Task<Course> UpdateAsync(Course item)
    {
        item.DescriptionOnMainPage = item.DescriptionOnMainPage?.Replace("/r", "").Replace("\r", "");
        if (item.IsDeleted)
        {
            item.IsVisibleOnMainPage = false;
            item.OrderOnMainPage = 0;
        }
        var res = await base.UpdateAsync(item);

        return res;

    }

    public override async Task Validate(Course oldValue, Course newItem)
    {
        await base.Validate(oldValue,newItem);
        if (newItem.IsVisibleOnMainPage && string.IsNullOrEmpty(newItem.IdentityString)) throw new Exception("Course can`t be visible on page if identity not defined");
        if (string.IsNullOrEmpty(newItem.IdentityString)) return;
        var allItems = GetItems(onlyCurrent: true);
        var existWithEqualIdenityString = await allItems.Where(x=> x.IdentityString == newItem.IdentityString).Select(x=>x.Id).ToListAsync();
        var children = GetAllChildren(newItem.Id).Select(x=>x.Id).ToList();
        if(!children.Contains(oldValue.Id)) children.Add(oldValue.Id);
        var res = existWithEqualIdenityString.Except(children).ToList();
        if (res.Count() > 0) throw new Exception("Идентификатор страницы должен быть уникальным");

        var existWithEqualOrderNum = await allItems.Where(x => x.OrderOnMainPage == newItem.OrderOnMainPage).ToListAsync();
        var res2 = existWithEqualOrderNum.Select(x => x.Id).Except(children);
        if (res2.Count() > 0 && existWithEqualOrderNum.Where(x=>res2.Contains(x.Id) && x.OrderOnMainPage !=0).Count() > 0) throw new Exception("Order num already in use. Try select next");



    }

    //public async Task<string> UploadFileAsync(Course item)
    //{
    //    var itemDb = await _contextModel.FindAsync(item.Id);
    //    var filePath = await UploadImageFileAsync(item, item.ImagesFiles, itemDb.PathToImage, "course");
    //    //itemDb.PathToImage = filePath;
    //    _contextModel.Update(itemDb);
    //    await _context.SaveChangesAsync();
    //    return filePath;

    //}



    public async Task<string> UploadIconAsync(Course item)
    {
        var itemDb = await _contextModel.FindAsync(item.Id);
        if (itemDb == null) throw new Exception("Model can`t be recognized");

        if (string.IsNullOrEmpty(itemDb.IdentityString)) throw new Exception("Model Identity for main page not set");

        var file = item.IconsFiles?.FirstOrDefault();
        var fileName = itemDb.IdentityString;

        //var filePath = await UploadImageFileAsync(item,item.IconsFiles, itemDb.PathToIcon,"icon_course");
        //var filePath = await FileUploader.UploadAsync(file, fileName, new List<string>() { item.GetType().Name, "icon" }, itemDb.PathToIcon);
        //itemDb.PathToIcon = filePath;
        //await CheckCanUploadImageAsync(file,fileName,itemDb.IconBytes);

        using (var stream = file.OpenReadStream())
        {
            var fileBytes = stream.ReadAsBytes();
            itemDb.IconBytes = fileBytes;
            _contextModel.Update(itemDb);
            await _context.SaveChangesAsync();
        }

        return Convert.ToBase64String(itemDb.IconBytes);
    }

    public async Task<string> UploadImageAsync(Course item)
    {

        var itemDb = await _contextModel.FindAsync(item.Id);
        if (itemDb == null) throw new Exception("Model can`t be recognized");

        if (string.IsNullOrEmpty(itemDb.IdentityString)) throw new Exception("Model Identity for main page not set");

        var file = item.ImagesFiles?.FirstOrDefault();
        var fileName = "";// itemDb.IdentityString;

        var imageBytes = await UploadImageAsync(file, item.Id, itemDb.ImageBytes, "");

        return Convert.ToBase64String(imageBytes);

    }

    public async Task<byte[]?> UploadImageAsync(IFormFile fileToUpload, Guid itemId, byte[]? oldImageBytes,string? uploadFileName)
    {
        await CheckCanUploadImageAsync(fileToUpload, oldImageBytes, uploadFileName);
        return await UploadImageAsync(fileToUpload, itemId);
    }

    //Need to override
    private async Task<byte[]?> UploadImageAsync(IFormFile fileToUpload,Guid itemId)
    {
        var itemDb = await _contextModel.FindAsync(itemId);
        if (itemDb == null) return null;

        using (var stream = fileToUpload.OpenReadStream())
        {
            var fileBytes = stream.ReadAsBytes();
            itemDb.ImageBytes = fileBytes;
            _contextModel.Update(itemDb);
            await _context.SaveChangesAsync();
            return fileBytes;
        }

    }


    private async Task CheckCanUploadImageAsync(IFormFile fileToUpload, byte[]? oldImageBytes, string? fileName)
    {
        if (fileToUpload == null) throw new Exception("Image not passed to server");
        var fileExtension = Path.GetExtension(fileToUpload?.FileName);
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        if (!allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase))) throw new BadImageFormatException();


        if (fileToUpload == null || fileToUpload.Length <= 0) throw new Exception("Before save you need to update image or image not set or file not correct");
        if (fileToUpload.Length > 30000000) throw new Exception("File weight too large. Compress image and try load later");

        await CheckValueIsNew(fileToUpload, oldImageBytes);
        //fileName += fileExtension;
    }
    private async Task CheckValueIsNew(IFormFile fileToUpload, byte[]? oldImageBytes)
    {
        if (oldImageBytes?.Length == 0 || oldImageBytes?.Length == null) return;
        if (fileToUpload.Length == oldImageBytes?.Length) throw new Exception("Uploaded file have equals size with already uploaded");

    }

    //    //CHANGE
    //    //string fileName = itemDb.IdentityString + fileExtension;



    //    //string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
    //    //var localPath = parentDirectory + "\\wwwroot\\images\\";
    //    //string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    //    //if (environment == "Development")
    //    //{
    //    //    localPath = parentDirectory + "\\SkillsHub\\wwwroot\\images\\";
    //    //}
    //    //var localPath = new List<string> { item.GetType().Name };  
    //    //if (!string.IsNullOrEmpty(prename)) localPath.Add(prename);


    //    //var fileNameDb = Path.GetFileName(pathToImageDb);
    //    //fileNameDb = localPath + fileNameDb;
    //    //var imageDbLenght = Path.Exists(fileNameDb) ? new System.IO.FileInfo(fileNameDb).Length : 0;
    //    //if (imageDbLenght != 0 && image.Length == imageDbLenght) throw new Exception("Uploaded file have equals size with already uploaded");

    //    //if (!Path.Exists(localPath)) Directory.CreateDirectory(localPath);

    //    //using (var fileStream = new FileStream(filePath, FileMode.Create))
    //    //{
    //    //    await image.CopyToAsync(fileStream);
    //    //}
    //    //if (!string.IsNullOrEmpty(prename))
    //    //    filePath =  "/images/" + prename + "/" + fileName;
    //    //else
    //    //    filePath = "/images/" + fileName;
    //    //return filePath;
    //}


}

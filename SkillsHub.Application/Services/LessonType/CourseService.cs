using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
public class CourseService : AbstractLessonTypeLogModelService<Course>, IUploadImageService<Course>, IUploadIconService<Course>
{
    protected override DbSet<Course> _contextModel => _context.Courses;

    protected override IValidator<Course> _validator => new CourseValidator();

    protected override IQueryable<Course>? _fullInclude => _context.Courses;

    public CourseService(ApplicationDbContext context,ILessonTypeService lessonTypeService) : base(context, lessonTypeService) { }
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

        var existWithEqualOrderNum = await allItems.Where(x => x.OrderOnMainPage == newItem.OrderOnMainPage).Select(x => x.Id).ToListAsync();
        var res2 = existWithEqualOrderNum.Except(children);
        if (res2.Count() > 0) throw new Exception("Order num already in use. Try select next");



    }

    public async Task<string> UploadImage(Course item)
    {
        var itemDb = await _contextModel.FindAsync(item.Id);
        var filePath = await UploadImageFileAsync(item, item.ImagesFiles, itemDb.PathToImage,"course");
        itemDb.PathToImage = filePath;
        //pizza.ImageMime = fileExtension.Replace(".", "");
        _contextModel.Update(itemDb);
        await _context.SaveChangesAsync();
        return filePath;

    }

    public async Task<string> UploadIcon(Course item)
    {
        var itemDb = await _contextModel.FindAsync(item.Id);
        var filePath = await UploadImageFileAsync(item,item.IconsFiles, itemDb.PathToIcon,"icon_course");
        itemDb.PathToIcon = filePath;
        //pizza.ImageMime = fileExtension.Replace(".", "");
        _contextModel.Update(itemDb);
        await _context.SaveChangesAsync();
        return filePath;
    }

    private async Task<string> UploadImageFileAsync(Course item, List<IFormFile> images, string? pathToImageDb, string prename = "")
    {
        if (images == null ) throw new Exception("Before save you need to update image");
        if (images.Count() < 1) throw new Exception("Image not set");
        if (Guid.Empty == item.Id) throw new Exception("Model can`t be recognized");
        var itemDb = await _contextModel.FindAsync(item.Id);
        if (itemDb == null) throw new Exception("Model not found in database");
        if (string.IsNullOrEmpty(itemDb.IdentityString)) throw new Exception("Model Identity for main page not set");

        //foreach (var image in item.ImagesFiles)

        var image = images.FirstOrDefault();
        if (image == null || image.Length <= 0) throw new Exception("File not correct");
        if (image.Length > 30000000) throw new Exception("File weight too large. Compress image and try load later");
        

        var fileExtension = Path.GetExtension(image.FileName);
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        if (!allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase))) throw new BadImageFormatException();

        string parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var localPath = parentDirectory + "\\wwwroot\\images\\";
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment == "Development")
        {
            localPath = parentDirectory + "\\SkillsHub\\wwwroot\\images\\";
        }
        
        string fileName = itemDb.IdentityString + fileExtension;
        if (!string.IsNullOrEmpty(prename)) localPath = localPath + prename + "\\";
        string filePath = localPath  + fileName;

        var fileNameDb = Path.GetFileName(pathToImageDb);
        fileNameDb = localPath + fileNameDb;
        var imageDbLenght = Path.Exists(fileNameDb) ? new System.IO.FileInfo(fileNameDb).Length : 0;
        if (imageDbLenght != 0 && image.Length == imageDbLenght) throw new Exception("Uploaded file have equals size with already uploaded");

        if (!Path.Exists(localPath)) Directory.CreateDirectory(localPath);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        if (!string.IsNullOrEmpty(prename))
            filePath =  "/images/" + prename + "/" + fileName;
        else
            filePath = "/images/" + fileName;
        return filePath;
    }


}

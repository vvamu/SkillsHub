using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestSharp.Extensions;
using SkillsHub.Domain.Models;
using System.Linq.Expressions;
namespace SkillsHub.Application.Helpers;

public abstract class FileUploader 
{
    public virtual  async Task<string> UploadAsync(IFormFile fileToUpload, string fileName, List<string>? localPathFolders, string? oldFilePath)
    {
        if (fileToUpload == null) throw new Exception("Image not passed to server");
        var fileExtension = Path.GetExtension(fileToUpload?.FileName);
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        if (!allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase))) throw new BadImageFormatException();


        if (fileToUpload == null || fileToUpload.Length <= 0) throw new Exception("Before save you need to update image or image not set or file not correct");
        if (fileToUpload.Length > 30000000) throw new Exception("File weight too large. Compress image and try load later");
        
        await CheckValueIsNew(fileToUpload, oldFilePath);
        fileName += fileExtension;
        var root = await GetRootPath(localPathFolders);
        return await UploadFile(fileToUpload, fileName, root);

    }
    public abstract Task<byte[]> GetAsync(string fileName);

    protected abstract Task CheckValueIsNew(IFormFile fileToUpload, string? oldFilePath);
    protected abstract Task<string> UploadFile(IFormFile fileToUpload, string fileName, Object? path);
    protected abstract Task<Object> GetRootPath(Object localPathFolders);


   

}

public static class FileUploaderHelper
{

    public static async Task<byte[]?> UploadImageAsync(IFormFile fileToUpload, Guid itemId, byte[]? oldImageBytes, string? uploadFileName)
    {
        await CheckCanUploadFileAsync(fileToUpload, oldImageBytes, uploadFileName);
        return new byte[1];
        //return await UploadFileAsync(fileToUpload, itemId);
    }

    //Need to override
    //public static async Task<byte[]?> UploadFileAsync(IFormFile fileToUpload, Guid itemId) { return new byte[1]; } //extend


    private static async Task CheckCanUploadFileAsync(IFormFile fileToUpload, byte[]? oldImageBytes, string? fileName)
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
    private static async Task CheckValueIsNew(IFormFile fileToUpload, byte[]? oldImageBytes)
    {
        if (oldImageBytes?.Length == 0 || oldImageBytes?.Length == null) return;
        if (fileToUpload.Length == oldImageBytes?.Length) throw new Exception("Uploaded file have equals size with already uploaded");

    }

    
}


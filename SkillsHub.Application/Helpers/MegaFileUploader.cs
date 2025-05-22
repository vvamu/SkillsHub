using CG.Web.MegaApiClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SkillsHub.Application.Repository.Base;
using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Application.Helpers;
public class MegaFileUploader  : FileUploader
{
    private readonly static string _login;
    private readonly static string _password;
    private readonly static MegaApiClient _client;
    static MegaFileUploader()
    {
        _login = "6183866@gmail.com";//configuration["MegaService:MegaLogin"];
        _password = "2002y1f1l1z1";//configuration["MegaService:MegaPassword"];
        _client = new MegaApiClient();
        _client.Login(_login, _password);
        //IEnumerable<INode> nodes = _client.GetNodes();
    }
    
    public override async Task<byte[]> GetAsync(string fileName)
    {
        Uri fileLink = new Uri(fileName);
        INode node = _client.GetNodeFromLink(fileLink);

        var stream = await _client.DownloadAsync(node);
        var bytes = new byte[stream.Length];

        stream.Read(bytes);

        return bytes;
    }

    protected override async Task CheckValueIsNew(IFormFile fileToUpload, string oldFilePath)
    {
        if (string.IsNullOrEmpty(oldFilePath) || !(oldFilePath is Uri)) return;
        
        INode node = await _client.GetNodeFromLinkAsync(new Uri(oldFilePath));
        var stream = await _client.DownloadAsync(node);
        var bytes = new byte[stream.Length];
        if (stream.Length != 0 && fileToUpload.Length == stream.Length) throw new Exception("Uploaded file have equals size with already uploaded");
        
    }
    protected override async Task<Object> GetRootPath(Object localPathFolders)
    {
        IEnumerable<INode> nodes = await _client.GetNodesAsync();
        INode root = nodes.Single(x => x.Name == "skills_hub");

        var localPathList = localPathFolders as List<string>;
        foreach (var folder in localPathList)
        {
            var newRoot = nodes.Where(x => x.Type == NodeType.Directory).FirstOrDefault(x => x.ParentId == root.Id && x.Name == folder);
            if (newRoot == null) { root = await _client.CreateFolderAsync(folder, root); continue; }
            root = newRoot;
        }
        return root;
    }
    protected override async Task<string> UploadFile(IFormFile fileToUpload, string fileName , Object? path)
    {
        var root = path as INode;
        INode file = await _client.UploadAsync(fileToUpload.OpenReadStream(), fileName, root);
        Uri downloadLink = _client.GetDownloadLink(file);
        var resPath = downloadLink.OriginalString;
        return resPath;
    }

}


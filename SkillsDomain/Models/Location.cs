using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SkillsHub.Domain.Models;

public class Location : BaseEntity
{
    public Location? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsOffline { get; set; }
    [NotMapped]
    public string DisplayName
    {
        get 
        {
            var result = "";
            if (IsOffline) result = "Offline -";
            else result += "Online - ";
            return result + Description;
        }
    }
    public string Description { get; set; }
}
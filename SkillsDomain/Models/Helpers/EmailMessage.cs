using SkillsHub.Domain.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SkillsHub.Domain.Models;

public class EmailMessage : BaseEntity
{
    [Required]
    public string Name { get; set; }
    [Required]
    [Phone]
    public string? Phone { get; set; }

    [Required]
    public string? Data { get; set; }

    public string Date { get; set; } = System.DateTime.Now.ToString();

    [EmailAddress]
    public string? Email { get; set; } = "-";

    public DateTime DateCreated { get; set; }



}

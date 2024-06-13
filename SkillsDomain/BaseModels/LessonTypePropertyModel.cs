using Newtonsoft.Json.Linq;
using SkillsHub.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillsHub.Domain.BaseModels;

public abstract class LessonTypePropertyModel<T> : LogModel<T>
{
    public List<LessonType>? LessonTypes { get; set; }

}

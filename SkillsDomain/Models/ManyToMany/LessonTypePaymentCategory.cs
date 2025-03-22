using SkillsHub.Domain.BaseModels;

namespace SkillsHub.Domain.Models;

public class LessonTypePaymentCategory : BaseEntity
{
    public PaymentCategory PaymentCategory { get; set; }
    public Guid? PaymentCategoryId { get; set; }
    public LessonType LessonType { get; set; }
    public Guid LessonTypeId { get; set; }
    public DateTime DateAdd { get; set; }

}
namespace SkillsDomain;

public class Lesson
{
    public DateTime Date { get; set; }

    public Teacher Teacher { get; set; }
    public List<Student> ArrivedStudent { get; set; }
    public bool IsСompleted { get; set; }
    public LessonType Type { get; set; }

}

using Moq;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Tests;

[TestFixture]
public class LessonManagerTests
{
    private Mock<ILessonService> _lessonService;

    [SetUp]
    public void Setup()
    {
        // Initialize the LessonManager instance
        //lessonManager = new LessonManager();
        _lessonService = new Mock<ILessonService>();

    }

    [Test]
    public void GetExistingLessonsFromGroup_NoExistingLessons_ReturnsEmptyList()
    {
        // Arrange
        int groupId = 1;

        // Act
        //List<Lesson> existingLessons = lessonManager.GetExistingLessonsFromGroup(groupId);
        

        // Assert
        //Assert.IsEmpty(existingLessons);
    }

    [Test]
    public void GetExistingLessonsFromGroup_ExistingLessons_ReturnsCorrectList()
    {
        // Arrange
        int groupId = 1;


        // Act
        //var lesson1 = new Lesson() { GroupId = 1 }
        List<Lesson> existingLessons = new List<Lesson>();//_lessonService.Create(groupId);

        // Assert
        Assert.AreEqual(3, existingLessons.Count);

        // Additional assertions based on your mock data
        Assert.AreEqual(DateTime.Parse("2023-01-01 09:00"), existingLessons[0].StartTime);
        Assert.AreEqual(DateTime.Parse("2023-01-01 10:00"), existingLessons[0].EndTime);

        Assert.AreEqual(DateTime.Parse("2023-01-01 14:00"), existingLessons[1].StartTime);
        Assert.AreEqual(DateTime.Parse("2023-01-01 15:00"), existingLessons[1].EndTime);

        Assert.AreEqual(DateTime.Parse("2023-01-01 17:00"), existingLessons[2].StartTime);
        Assert.AreEqual(DateTime.Parse("2023-01-01 18:00"), existingLessons[2].EndTime);
    }
}

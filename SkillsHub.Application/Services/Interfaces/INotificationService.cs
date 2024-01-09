using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface INotificationService
{
	public Task<NotificationMessage> СreateToEditLesson(Lesson lastLessonValue, Lesson? lesson, List<ApplicationUser>? usersToSend, int answer = 1);
    public Task<NotificationMessage> СreateToUpdateCountLessonsInGroup(Group group, int previousCountLessons, int currentCountLessons, List<ApplicationUser>? usersToSend);


}

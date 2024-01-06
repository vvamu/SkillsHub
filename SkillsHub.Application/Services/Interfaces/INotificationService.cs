using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface INotificationService
{
	public Task<NotificationMessage> CreateToLesson(Lesson lastLessonValue, Lesson? lesson, List<ApplicationUser>? usersToSend, int answer = 1);

}

using Microsoft.AspNetCore.Mvc;
using SkillsHub.Application.DTO;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;

namespace SkillsHub.Application.Services.Interfaces;

public interface IRequestService
{
    public Task<List<RequestLesson>> GetAll();
    public Task<RequestLesson> Get(Guid id);
    public Task<RequestLesson> Create(Guid lessonId, string requestMessage, Lesson? newLesson = null);

    public Task<RequestLesson> ApplyLessonRequest(RequestLesson item, int answer);
    public Task<RequestLesson> ApplyLessonDeleteRequest(RequestLesson item, int answer);

    public Task DeletePreviousRequests(RequestLesson item);
    public Task DeletePreviousRequests(Lesson item);




}

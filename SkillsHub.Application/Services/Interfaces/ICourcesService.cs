using Microsoft.EntityFrameworkCore;
using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsHub.Application.Services.Interfaces;

public interface ICourcesService
{
    public  Task InitCources();

    public IQueryable<LessonType> GetAllLessonType(); //Offline / Online
    public IQueryable<LessonActivityType> GetAllLessonActivityType(); //Отработка Пробное Обычное
    public IQueryable<CourceName> GetAllCourcesNames(); //"English for adults", "English for children", "English for organizations"
    public IQueryable<Cource> GetAllCources();

    public IQueryable<EnglishLevel> GetAllEnglishLevels();


    public Task<CourceName> CreateCourceName(CourceName item);
	public Task<LessonType> CreateLessonType(LessonType item);
}

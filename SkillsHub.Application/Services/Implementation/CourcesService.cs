using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.DTO;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Validators;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Domain.Models.NotInUse;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Services.Implementation;

//For Admin Panel
public class CourcesService : ICourcesService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CourcesService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task InitCources()
    {
        EnglishLevelsInit();
        CourceNamesInit();
        LessonTypesInit();
        LessonActivityTypeInit();

        //AdultCourcesInit();
        //ChildrenCources();

        await _context.SaveChangesAsync();
    }
    public IQueryable<LessonType> GetAllLessonType() => _context.LessonTypes; //Offline / Online
    public IQueryable<LessonActivityType> GetAllLessonActivityType() => _context.LessonActivityTypes; //Отработка Пробное Обычное
    public IQueryable<CourseName> GetAllCourcesNames() => _context.CourseNames; //"English for adults", "English for children", "English for organizations"
    //public IQueryable<EnglishLevel> GetAllEnglishLevels() => _context.EnglishLevels;

    /*
    public async Task<Group> AddTeacherToGroup(Guid userId, Guid groupIf) // == create Group
    {
        var group = _context
    }
    public async Task<Group> CreateCourceToUser(Guid userId,Group group) // == create Group
    {
        //var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        Student st;
        var user = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == userId);
        if(user == null)
            st = await _context.Students.FirstOrDefaultAsync(x => x.Id == userId);

    }
    */


    public async Task<LessonType> CreateLessonType(LessonType item)
    {
		if (item == null) throw new Exception("Not correct data for group");
		var userRegisterValidator = new LessonTypeValidator();
		var validationResult = await userRegisterValidator.ValidateAsync(item);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors;
			var errorsString = string.Concat(errors);
			throw new Exception(errorsString);
		}

		if (_context.LessonTypes.FirstOrDefault(x => x.Name == item.Name) != null) throw new Exception("Lesson type with such name already exist");
        item.DateCreated = DateTime.Now;
        await _context.LessonTypes.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
	}


	public async Task<CourseName> CreateCourceName(CourseName item)
	{
		if (item == null) throw new Exception("Not correct data for group");
		var userRegisterValidator = new CourceNameValidator();
		var validationResult = await userRegisterValidator.ValidateAsync(item);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors;
			var errorsString = string.Concat(errors);
			throw new Exception(errorsString);
		}

        if (_context.CourseNames.FirstOrDefault(x => x.Name == item.Name) != null)
        {
            var it = _context.CourseNames.FirstOrDefault(x => x.Name == item.Name);

			throw new Exception("Cource name type with such name already exist");
        }
		item.DateCreated = DateTime.Now;
		await _context.CourseNames.AddAsync(item);
		await _context.SaveChangesAsync();
		return item;
	}

	#region Init - LessonTypes
	public void LessonTypesInit()
    {
        if (_context.LessonTypes.FirstOrDefault(x => x.Name == "Group English") != null) return;

        _context.LessonTypes.Add(new LessonType() { Name = "Group English", StudentPrice = 22.5M, TeacherPrice=100, LessonTimeInMinutes = 70, MinumumLessonsToPay = 8});
        _context.LessonTypes.Add(new LessonType() { Name = "Individual English", StudentPrice = 40, TeacherPrice = 100 , LessonTimeInMinutes = 60 , MinumumLessonsToPay = 1});
		_context.LessonTypes.Add(new LessonType() { Name = "Group Programming", StudentPrice = 30, TeacherPrice = 100 , LessonTimeInMinutes = 90, MinumumLessonsToPay = 4 });
    
		_context.SaveChanges();
    }
    public void LessonActivityTypeInit()
    {
        if (_context.LessonActivityTypes.FirstOrDefault(x => x.Name == "Отработка") != null) return;

        _context.LessonActivityTypes.Add(new LessonActivityType() { Name = "Отработка" , StudentPrice = 0, TeacherPrice = 10});
        _context.LessonActivityTypes.Add(new LessonActivityType() { Name = "Пробное", StudentPrice = 0, TeacherPrice = 10 });
        _context.LessonActivityTypes.Add(new LessonActivityType() { Name = "Обычное", StudentPrice = 0, TeacherPrice = 10 });
        _context.SaveChanges();

    }
    public void EnglishLevelsInit()
    {
        var englishLevels = new List<string>() { "A1.1", "A1.2", "A2", "B1", "B2", "C1", "C2" };
        //if (_context.EnglishLevels.FirstOrDefault(x => x.Name == englishLevels[0]) != null) return;

        foreach(var item in englishLevels)
        {
           // _context.EnglishLevels.Add(new EnglishLevel() { Name = item });
        }
        _context.SaveChanges();

    }
    public void CourceNamesInit()
    {
		var courseNames = new List<CourseName>()
        {
	        new CourseName() { Name = "English for adults", MinimumAge = 16, MaximumAge = 100 },
	        new CourseName() { Name = "English for children", MinimumAge = 4, MaximumAge = 16 },
	        new CourseName() { Name = "English for organizations", MinimumAge = 16, MaximumAge = 100 },
	        new CourseName() { Name = "English express", MinimumAge = 16, MaximumAge = 100 },
	        new CourseName() { Name = "Speaking club", MinimumAge = 16, MaximumAge = 100 },
	        new CourseName() { Name = "Programming Scratch", MinimumAge = 6, MaximumAge = 14 },
	        new CourseName() { Name = "Programming Python", MinimumAge = 10, MaximumAge = 14 },
	        new CourseName() { Name = "Programming Java", MinimumAge = 12, MaximumAge = 16 }
        };

		foreach (var courseName in courseNames)
		{
            if (_context.CourseNames.FirstOrDefault(x => x.Name == courseName.Name) != null) return;
			
			_context.CourseNames.Add(courseName);
			
		}

		_context.SaveChanges();
    }

    #endregion

    #region Adult Children Cources Init Not in use

    public void AdultCourcesInit()
    {
        var adult = _context.CourseNames.FirstOrDefault(x => x.Name == "English for adults");
        //if (_context.Cources.Include(x=>x.Name).FirstOrDefault(x => x.Name == adult) != null) return;

        /*
        foreach (var englishLevel in _context.EnglishLevels)
        {
            /*
            var item = new Cource()
            {
                SubscriptionType = "Term",
                Name = adult,
                EnglishLevel = englishLevel,

                LessonsPerWeek = 2,
                CountMonth = 5, //correct
                MaxArrivedStudents = 8,
                MinimumAge = 16
            };

            _context.Cources.Add(item);
            //item.LessonType = "Offline";
            _context.Cources.Add(item);
            */
        //}

        _context.SaveChanges();
    }

    public void ChildrenCources()
    {
        //var 

        //var children71 = new Cource()
        //{
        //    SubscriptionType = "Termless",
        //    Name = "English for children",
        //    EnglishLevel = "A1.1",
        //    LessonType = "Offline",
        //    LessonTimeInMinutes = 45,
        //    LessonsPerWeek = 2,
        //    MaxArrivedStudents = 1,
        //    MinimumAge = 4,
        //    TeacherPrice = 0,
        //    StudentPrice = 0
        //};

        //var children72 = new Cource()
        //{
        //    SubscriptionType = "Termless",
        //    Name = "English for children",
        //    EnglishLevel = "A1.1",
        //    LessonType = "Online",
        //    LessonTimeInMinutes = 45,
        //    LessonsPerWeek = 2,
        //    MaxArrivedStudents = 1,
        //    MinimumAge = 4,
        //    TeacherPrice = 0,
        //    StudentPrice = 0
        //};

        //var englishChildrenLevels7 = new List<string> { "A1.1", "A1.2", "A2" }; //7-10
        //foreach(var englishLevel in englishChildrenLevels7)
        //{
        //    for (int i = 60; i <= 70; i += 10)
        //    {
        //        var lessonType = "Offline";
        //        if (i == 70) lessonType = "Online";

        //        var item = new Cource()
        //        {
        //            SubscriptionType = "Term",
        //            Name = "English for children",
        //            EnglishLevel = englishLevel,
        //            LessonType = lessonType,
        //            LessonTimeInMinutes = 70,
        //            LessonsPerWeek = 2,
        //            CountMonth = 5, //correct
        //            MaxArrivedStudents = 8,
        //            MinimumAge = 7,
        //            TeacherPrice = 0,
        //            StudentPrice = 0
        //        };
        //        childrenCources.Add(item);
        //    }
        //}

        //var englishChildrenLevels11 = new List<string> { "A1.1", "A1.2", "A2", "B1" }; //11-15

        //foreach (var englishLevel in englishChildrenLevels11)
        //{
        //    for (int i = 60; i <= 70; i += 10)
        //    {
        //        var lessonType = "Offline";
        //        if (i == 70) lessonType = "Online";

        //        var item = new Cource()
        //        {
        //            SubscriptionType = "Term",
        //            Name = "English for children",
        //            EnglishLevel = englishLevel,
        //            LessonType = lessonType,
        //            LessonTimeInMinutes = 70,
        //            LessonsPerWeek = 2,
        //            CountMonth = 5, //correct
        //            MaxArrivedStudents = 8,
        //            MinimumAge = 7,
        //            TeacherPrice = 0,
        //            StudentPrice = 0
        //        };
        //        childrenCources.Add(item);
        //    }
        //}

        //childrenCources.Add(children71);
        //childrenCources.Add(children72);
        //return childrenCources;
    }

    #endregion


}

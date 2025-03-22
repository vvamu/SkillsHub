using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Implementation.User;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;

namespace SkillsHub.Tests.UnitTests;


[TestFixture]
public class UserServiceTest
{
    private UserService _mockUserService;
    private ApplicationDbContext _mockContext;

    [SetUp]
    public void Setup()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddAutoMapper(typeof(MappingProfile).Assembly);
                services.AddTransient<IUserService, UserService>();
                //services.AddTransient<IExternalService, ExternalService>();
                //services.AddTransient<ICourcesService, CourcesService>();
                services.AddScoped<IGroupService, GroupService>();
                //services.AddTransient<IRequestService, RequestService>();
                services.AddTransient<INotificationService, NotificationService>();
                services.AddScoped<ILessonService, LessonService>();
                //services.AddScoped<ISalaryService, SalaryService>();
                services.AddScoped<IBaseUserInfoService, BaseUserInfoService>();
                services.AddScoped<IApplicationUserBaseUserInfoService, ApplicationUserBaseUserInfoService>();


                services.AddScoped<ILessonTypeService, LessonTypeService>();
                services.AddScoped<IAbstractLogModel<PaymentCategory>, PaymentCategoryService>();
                services.AddScoped<IAbstractLogModel<AgeType>, AgeTypeService>();
                services.AddScoped<IAbstractLogModel<GroupType>, GroupTypeService>();
                services.AddScoped<IAbstractLogModel<Course>, CourseService>();
                services.AddScoped<IAbstractLogModel<Location>, LocationService>();
                //services.AddTransient<IAbstractLogModel<LessonTypeStudent>, LessonTypeStudentService>();
                //services.AddTransient<IAbstractLogModel<LessonTypeTeacher>, LessonTypeTeacherService>();
                //services.AddTransient<IUserRoleModelService<Student>, StudentService>();
                //services.AddTransient<IUserRoleModelService<Teacher>, TeacherService>();
            })
        .Build();



        var newUser = new Application.DTO.UserCreateDTO();
        IQueryable<ApplicationUser> _items = new List<ApplicationUser>().AsQueryable();

        //_mockUserService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_items);


    }

    [Test]
    public async Task GetUserdByIdAsync_returns_user()
    {


        //var users =await _mockUserService.Object.GetAllAsync();
        //var usersDb = await _mockContext.ApplicationUsers.ToListAsync();
        //var u = users.ToList();
        //Assert.AreEqual(u.Count, usersDb.Count);
    }
    [Test]
    public async Task CreateUserAsync_returns_user()
    {

        //var newUser = new Application.DTO.UserCreateDTO();
        //IQueryable<ApplicationUser> _items = new List<ApplicationUser>().AsQueryable();
        //_mockUserService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_items);
        //_mockUserService.Setup(repo => repo.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ApplicationUser());
        //_mockUserService.Setup(repo => repo.CreateUserAsync(newUser)).ReturnsAsync(new ApplicationUser());

        //var user = await _mockUserService.Object.CreateUserAsync(newUser);
        //var users = await _mockUserService.Object.GetAllAsync();
        //var usersDb = await _mockContext.ApplicationUsers.ToListAsync();
        //var u = users.ToList();
        //Assert.AreEqual(u, usersDb);

        //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        //    .EnableSensitiveDataLogging()
        //    .EnableSensitiveDataLogging()
        //    .Options;
        //_mockContext = new ApplicationDbContext(options);
        //var mockSignInManager = new Mock<SignInManager<ApplicationUser>>();
        //var mockMapper = new Mock<IMapper>();
        //var mockUserManager = new Mock<UserManager<ApplicationUser>>();
        //var mockBaseUserInfoService = new Mock<IBaseUserInfoService>(); //
        //_mockUserService = new UserService(mockSignInManager.Object, mockUserManager.Object, _mockContext, mockMapper.Object, mockBaseUserInfoService.Object); //();
        //var _fixture = new Fixture();
        //_fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        //_fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        //var newUser = new Application.DTO.UserCreateDTO(); // UserCreateDTO without setting the Name property
        //mockBaseUserInfoService.Setup(x => x.CreateAsync(It.IsAny<BaseUserInfo>())).ReturnsAsync(new BaseUserInfo());
        ////_mockUserService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<ApplicationUser>().AsQueryable());
        ////_mockUserService.Setup(repo => repo.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ApplicationUser());
        ////_mockUserService.Setup(repo => repo.CreateUserAsync(It.IsAny<UserCreateDTO>())).ReturnsAsync(new ApplicationUser());


        //var userCreated = await _mockUserService.CreateUserAsync(newUser);

        //Assert.IsNotNull(userCreated);

    }



    //mockRepo.Setup(repo => repo.CreateUserAsync(new Application.DTO.UserCreateDTO()).Callback<ApplicationUser>(new ApplicationUser()));
    //mockRepo.Setup(repo => repo.UpdateCatalogItem(It.IsAny<CatalogItem>()))
    //    .Callback<CatalogItem>(i =>
    //    {
    //        var item = _items.FirstOrDefault(catalogItem => catalogItem.Id == i.Id);
    //        if (item != null)
    //        {
    //            item.Name = i.Name;
    //            item.Description = i.Description;
    //            item.Price = i.Price;
    //        }
    //    });
    //mockRepo.Setup(repo => repo.DeleteCatalogItem(It.IsAny<string>()))
    //    .Callback<string>(id => _items.RemoveAll(i => i.Id == id));
    //_controller = new CatalogController(mockRepo.Object);


    //[Test]
    //public async Task CreateTeacherAsync_ShouldCreateTeacher()
    //{
    //    // Arrange
    //    var userId = Guid.NewGuid();
    //    var teacher = new Teacher { /* Set teacher properties */ };

    //    // Act
    //    //var createdTeacher = await _userService.CreateTeacherAsync(userId, teacher);

    //    // Assert
    //    //Assert.IsNotNull(createdTeacher);
    //    // Add more assertions based on your business logic
    //}
    //[SetUp]
    //public void Setup(ICourseService userService, ApplicationDbContext context)
    //{
    //    //_userService = new UserService(mockSignInManager, mockUserManager, mockContext, mockMapper);
    //}
    // Add more tests for other methods in UserService
}

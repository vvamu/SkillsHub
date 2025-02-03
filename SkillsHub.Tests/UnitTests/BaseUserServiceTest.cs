using NUnit.Framework;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
using SkillsHub.Persistence;
using Moq;
using SkillsHub.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SkillsHub.Application.Helpers;
using ICourseService = SkillsHub.Application.Helpers.AbstractLessonTypeLogModelService<SkillsHub.Domain.Models.Course>;
using SkillsHub.Domain.BaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SkillsHub.Application.DTO;
using AutoFixture;

namespace SkillsHub.Tests.UnitTests;

[TestFixture]
public class BaseUserInfoServiceTests
{
    private Mock<ApplicationDbContext> _mockContext;
    private Mock<UserManager<ApplicationUser>> _mockUserManager;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _mockMapper = new Mock<IMapper>();
    }

    [Test]
    public async Task CreateAsync_WhenValidItemProvided_ShouldReturnCreatedBaseUserInfo()
    {
        // Arrange
        var service = new BaseUserInfoService(_mockContext.Object, _mockUserManager.Object, _mockMapper.Object);

        var item = new BaseUserInfo
        {
            // Initialize properties of the BaseUserInfo object as needed for the test
        };

        //_mockContext.Setup(c => c.BaseUserInfo.FindAsync(It.IsAny<Guid>())).ReturnsAsync((BaseUserInfo)null);
        //_mockContext.Setup(c => c.BaseUserInfo.AddAsync(It.IsAny<BaseUserInfo>())).ReturnsAsync(new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseUserInfo>());
        //_mockContext.Setup(c => c.BaseUserInfo.Where(x => x.Parent == null).ToListAsync()).ReturnsAsync(new List<BaseUserInfo>());
        //_mockContext.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var createdUser = await service.CreateAsync(item);

        // Assert
        Assert.IsNotNull(createdUser);
        // Add more specific assertions as needed based on the behavior of CreateAsync method
    }
}


//[TestFixture]
//public class BaseUserServiceTest
//{
//    private  UserService _mockUserService;
//    private  ApplicationDbContext _mockContext;

//    [SetUp]
//    public void Setup()
//    {


//    }

//    [Test]
//    public async Task GetUserdByIdAsync_returns_user()
//    {


//        //var users =await _mockUserService.Object.GetAllAsync();
//        //var usersDb = await _mockContext.ApplicationUsers.ToListAsync();
//        //var u = users.ToList();
//        //Assert.AreEqual(u.Count, usersDb.Count);
//    }
//    [Test]
//    public async Task CreateUserAsync_returns_user()
//    {



//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase(Guid.NewGuid().ToString())
//            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
//            .EnableSensitiveDataLogging()
//            .EnableSensitiveDataLogging()
//            .Options;
//        _mockContext = new ApplicationDbContext(options);
//        var mockSignInManager = new Mock<SignInManager<ApplicationUser>>();
//        var mockMapper = new Mock<IMapper>();
//        var mockUserManager = new Mock<UserManager<ApplicationUser>>();
//        var mockBaseUserInfoService = new BaseUserInfoService(_mockContext,mockUserManager.Object,mockMapper.Object); //


//        var item = await mockBaseUserInfoService.CreateAsync(new BaseUserInfo());
//        var items = mockBaseUserInfoService.GetAll().ToList();

//        //_mockUserService.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<ApplicationUser>().AsQueryable());
//        //_mockUserService.Setup(repo => repo.GetUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ApplicationUser());
//        //_mockUserService.Setup(repo => repo.CreateUserAsync(It.IsAny<UserCreateDTO>())).ReturnsAsync(new ApplicationUser());


//        //var userCreated = await _mockUserService.CreateUserAsync(newUser);

//        Assert.IsNotNull(1);

//    }



//    //mockRepo.Setup(repo => repo.CreateUserAsync(new Application.DTO.UserCreateDTO()).Callback<ApplicationUser>(new ApplicationUser()));
//    //mockRepo.Setup(repo => repo.UpdateCatalogItem(It.IsAny<CatalogItem>()))
//    //    .Callback<CatalogItem>(i =>
//    //    {
//    //        var item = _items.FirstOrDefault(catalogItem => catalogItem.Id == i.Id);
//    //        if (item != null)
//    //        {
//    //            item.Name = i.Name;
//    //            item.Description = i.Description;
//    //            item.Price = i.Price;
//    //        }
//    //    });
//    //mockRepo.Setup(repo => repo.DeleteCatalogItem(It.IsAny<string>()))
//    //    .Callback<string>(id => _items.RemoveAll(i => i.Id == id));
//    //_controller = new CatalogController(mockRepo.Object);


//    //[Test]
//    //public async Task CreateTeacherAsync_ShouldCreateTeacher()
//    //{
//    //    // Arrange
//    //    var userId = Guid.NewGuid();
//    //    var teacher = new Teacher { /* Set teacher properties */ };

//    //    // Act
//    //    //var createdTeacher = await _userService.CreateTeacherAsync(userId, teacher);

//    //    // Assert
//    //    //Assert.IsNotNull(createdTeacher);
//    //    // Add more assertions based on your business logic
//    //}
//    //[SetUp]
//    //public void Setup(ICourseService userService, ApplicationDbContext context)
//    //{
//    //    //_userService = new UserService(mockSignInManager, mockUserManager, mockContext, mockMapper);
//    //}
//    // Add more tests for other methods in UserService
//}

using NUnit.Framework;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsHub.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;

        [SetUp]
        public void Setup(IUserService userService)
        {
            // Set up your service with mock dependencies if necessary
            
            //_userService = new UserService(mockSignInManager, mockUserManager, mockContext, mockMapper);
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var user = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(userId, user.Id);
        }

        

        [Test]
        public async Task CreateTeacherAsync_ShouldCreateTeacher()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var teacher = new Teacher { /* Set teacher properties */ };

            // Act
            //var createdTeacher = await _userService.CreateTeacherAsync(userId, teacher);

            // Assert
            //Assert.IsNotNull(createdTeacher);
            // Add more assertions based on your business logic
        }

        // Add more tests for other methods in UserService
    }
}

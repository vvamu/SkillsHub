global using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.Models;
namespace SkillsHub.Tests;

//[SetUpFixture]
//public class UnitTestSetupFixture
//{

       
    
//    //[SetUp]
//    //public void Setup()
//    //{
//    //    Console.WriteLine("===============");
//    //    Console.WriteLine("=====START=====");
//    //    Console.WriteLine("===============");


//    //}

//    //[TearDown]
//    //public void TearDown()
//    //{
//    //    Console.WriteLine("===============");
//    //    Console.WriteLine("=====BYE!======");
//    //    Console.WriteLine("===============");
//    //}
//}
//
//

public class Program
{
    public static async Task<int> Main(string[] args)
    {

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddAutoMapper(typeof(MappingProfile).Assembly);
                services.AddTransient<IUserService, UserService>();
                services.AddTransient<IExternalService, ExternalService>();
                //services.AddTransient<ICourcesService, CourcesService>();
                services.AddScoped<IGroupService, GroupService>();
                services.AddTransient<IRequestService, RequestService>();
                services.AddTransient<INotificationService, NotificationService>();
                services.AddScoped<ILessonService, LessonService>();
                services.AddScoped<ISalaryService, SalaryService>();
                services.AddScoped<IBaseUserInfoService, BaseUserInfoService>();
                services.AddScoped<IApplicationUserBaseUserInfoService, ApplicationUserBaseUserInfoService>();
                services.AddScoped<ILessonTypeService, LessonTypeService>();
                services.AddScoped<IAbstractLogModel<PaymentCategory>, PaymentCategoryService>();
                services.AddScoped<IAbstractLogModel<AgeType>, AgeTypeService>();
                services.AddScoped<IAbstractLogModel<GroupType>, GroupTypeService>();
                services.AddScoped<IAbstractLogModel<Course>, CourseService>();
                services.AddScoped<IAbstractLogModel<Location>, LocationService>();
                services.AddTransient<IAbstractLogModel<LessonTypeStudent>, LessonTypeStudentService>();
                services.AddTransient<IAbstractLogModel<LessonTypeTeacher>, LessonTypeTeacherService>();
                services.AddTransient<IUserRoleModelService<Student>, StudentService>();
                services.AddTransient<IUserRoleModelService<Teacher>, TeacherService>();
            })
        .Build();

        host.Run();

        return 1;
           
    }
}
global using SkillsHub.Domain.Models;

using EmailProvider;
using EmailProvider.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Options;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Helpers;
using SkillsHub.Persistence;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using Viber.Bot.NetCore.Middleware;

namespace SkillsHub;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews().
            AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = ".AdventureWorks.Session";
            options.IdleTimeout = TimeSpan.FromDays(10);
            options.Cookie.IsEssential = true;
        });


        var services = builder.Services;
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        #region Options pattern
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

        builder.Services.AddSingleton(configuration);
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.OptionName));
        services.Configure<ConnectionStringsOptions>(configuration.GetSection(ConnectionStringsOptions.OptionName));
        services.Configure<MailOptions>(configuration.GetSection(MailOptions.OptionName));

        #endregion

        #region DbConfig

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(
        options =>
        {
            options.UseSqlServer(connectionString,options => options.EnableRetryOnFailure().CommandTimeout(60));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging();
        },
        ServiceLifetime.Scoped);

        #endregion

        #region Authentication
        
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
       
        
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        });

        #endregion

        #region Roles
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Teacher", builderPolicy =>
            {
                builderPolicy.RequireClaim(ClaimTypes.Role, "Teacher");
            });
            options.AddPolicy("Student", builderPolicy =>
            {
                builderPolicy.RequireClaim(ClaimTypes.Role, "Student");
            });

            options.AddPolicy("Admin", builderPolicy =>
            {
                builderPolicy.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Teacher") ||
                                               x.User.HasClaim(ClaimTypes.Role, "Admin"));
            });


        });

        #endregion

        #region Own Services
        
        builder.Services.AddTransient<EmailProvider.Interfaces.IMailService, MailService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IExternalService, ExternalService>();
        //builder.Services.AddTransient<ICourcesService, CourcesService>();
        builder.Services.AddScoped<IGroupService, GroupService>();
        builder.Services.AddTransient<IRequestService, RequestService>();
        builder.Services.AddTransient<INotificationService, NotificationService>();
        builder.Services.AddScoped<ILessonService, LessonService>();
        builder.Services.AddScoped<ISalaryService, SalaryService>();
        builder.Services.AddScoped<IBaseUserInfoService, BaseUserInfoService>();
        builder.Services.AddScoped<IApplicationUserBaseUserInfoService, ApplicationUserBaseUserInfoService> ();


        builder.Services.AddScoped<ILessonTypeService, LessonTypeService>();
        builder.Services.AddScoped<IAbstractLogModel<PaymentCategory>, PaymentCategoryService>();
        builder.Services.AddScoped<IAbstractLogModel<AgeType>, AgeTypeService>();
        builder.Services.AddScoped<IAbstractLogModel<GroupType>, GroupTypeService>();
        builder.Services.AddScoped<IAbstractLogModel<Course>, CourseService>();
        builder.Services.AddScoped<IAbstractLogModel<Location>, LocationService>();
        builder.Services.AddTransient<IAbstractLogModel<LessonTypeStudent>, LessonTypeStudentService>();
        builder.Services.AddTransient<IAbstractLogModel<LessonTypeTeacher>, LessonTypeTeacherService>();
        builder.Services.AddTransient<IUserRoleModelService<Student>, StudentService>();
        builder.Services.AddTransient<IUserRoleModelService<Teacher>, TeacherService>();



        #endregion

        services.AddControllers()
        .AddRazorRuntimeCompilation();

        builder.Services.AddCors();
        services.AddHostedService<RepeatingService>();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Home/Error");\
            app.UseDeveloperExceptionPage();
            app.UseHsts();
        }
        if (app.Environment.IsProduction())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseCors(opt =>
        {
            opt.AllowAnyMethod();
            opt.AllowAnyHeader();
            opt.AllowAnyOrigin();
        });
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        /*
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var statusCode = context.Response.StatusCode;

                if (statusCode == 404)
                {
                    context.Response.Redirect("/Account/SignIn");
                    return;
                }

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/html";

                // Перенаправление на страницу Home/Error
                context.Response.Redirect("/Home/Error");
            });
        });*/
        app.UseStatusCodePagesWithReExecute("/Account/SignIn", "?statusCode={0}");
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession(); // use this before .UseEndpoints or .MapControllerRoute

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        app.MapControllerRoute(
        name: "ThanksRoute",
        pattern: "{controller=Home}/{action=Thanks}",
        defaults: new { controller = "Home", action = "Thanks" }
        );
        app.MapControllerRoute(name: "thanks",
                pattern: "thanks/",
                defaults: new { controller = "Home", action = "Thanks" });





        #region Roles
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var roles = roleManager.Roles.ToList();
            string[] roleNames = { "Admin", "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }
        #endregion


        app.Run();

    }

    public void ConfigureServices(IServiceCollection services)
    {

    }
}
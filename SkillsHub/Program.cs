global using SkillsHub.Domain.Models;

using EmailProvider;
using EmailProvider.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Options;
using SkillsHub.Application.Repository.Base;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Implementation.User;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Application.Services.Repository;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Helpers;
using SkillsHub.Persistence;
using System.Net;
using System.Security.Claims;

namespace SkillsHub;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews().
            AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        //FOR SESSION
        //builder.Services.AddDistributedMemoryCache();
        //builder.Services.AddSession(options =>
        //{
        //    options.IdleTimeout = TimeSpan.FromDays(30); // ���������� ����� ������� ������ �� 30 ����
        //});
        //builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
        //.AddCookie(options =>
        //{
        //    options.Cookie.IsEssential = true; // Make the cookie essential for preserving user authentication
        //    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Set the expiration time of the cookie
        //    options.SlidingExpiration = true; // Extend the expiration time with each request
        //});

        builder.Services.AddDistributedMemoryCache();

        var environment = builder.Services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();
        builder.Services.AddDataProtection().SetApplicationName($"my-app-{environment.EnvironmentName}")
                    .PersistKeysToFileSystem(new DirectoryInfo($@"{environment.ContentRootPath}\keys"));

        //builder.Services.AddSession(options =>
        //{
        //    options.Cookie.Name = "skills.Session";
        //    options.IdleTimeout = TimeSpan.FromDays(10);
        //    options.Cookie.IsEssential = true;
        //});
        builder.Services.AddSession();
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ".AspNetCore.Identity.Application";
            options.ExpireTimeSpan = TimeSpan.FromDays(10); // ��������� ����� �������� ���� �� 10 ����
            options.SlidingExpiration = true; // ��������� ����������� ��������� ����� ��������
           
                                              
        });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ".AdventureWorks.Session";
            options.ExpireTimeSpan = TimeSpan.FromDays(10); // ��������� ����� �������� ���� �� 10 ����
            options.SlidingExpiration = true; // ��������� ����������� ��������� ����� ��������
                                              // ������ ��������� ��� ���� .AdventureWorks.Session
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
        //var connectionString = "Server=localhost;Port=3306;Database=skills;Uid=root;Pwd=nadezhda123;";//builder.Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(
        options =>
        {
            options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure().CommandTimeout(60));
            
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging();
        },
        ServiceLifetime.Scoped);

        #endregion

        #region Authentication

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 3;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
        });

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

            //options.AddPolicy("Programmer", builderPolicy =>
            //{
            //    builderPolicy.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Teacher") ||
            //                                   x.User.HasClaim(ClaimTypes.Role, "Student") ||
            //                                   x.User.HasClaim(ClaimTypes.Role, "Admin") ||
            //                                   x.User.HasClaim(ClaimTypes.Role, "Programmer"));
            //});


        });

        #endregion

        #region Own Services

        //builder.Services.AddViberBotApi(opt =>
        //{
        //    opt.Token = "5192d0382ea7e2e9-d35f8cb4f9a26339-3a3429d4a23b0c5f";
        //    opt.Webhook = "https://localhost:7150/Viber/Get";
        //});

        builder.Services.AddTransient<EmailProvider.Interfaces.IMailService, MailService>();  
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IAbstractLogModelService<BaseUserInfo>, BaseUserInfoRepository>();
        builder.Services.AddTransient<IUserService, UserService>();

		builder.Services.AddTransient<IAbstractLogModelService<Teacher>, TeacherService>();
		builder.Services.AddTransient<IAbstractLogModelService<Student>, StudentService>();


		//builder.Services.AddTransient<IExternalService, ExternalService>();
		//builder.Services.AddTransient<ICourcesService, CourcesService>();
		builder.Services.AddScoped<IGroupService, GroupService>();
        //builder.Services.AddTransient<IRequestService, RequestService>();
        builder.Services.AddTransient<INotificationService, NotificationService>();
        builder.Services.AddScoped<ILessonService, LessonService>();
        //builder.Services.AddScoped<ISalaryService, SalaryService>();
        
        //builder.Services.AddScoped<IApplicationUserBaseUserInfoService, ApplicationUserBaseUserInfoService>();


        builder.Services.AddScoped<ILessonTypeService, LessonTypeService>();
        builder.Services.AddScoped<IAbstractLogModelService<PaymentCategory>, PaymentCategoryService>();
        builder.Services.AddScoped<IAbstractLogModelService<AgeType>, AgeTypeService>();
        builder.Services.AddScoped<IAbstractLogModelService<GroupType>, GroupTypeService>();
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IAbstractLogModelService<Location>, LocationService>();


        //builder.Services.AddTransient<IUserRoleModelService<Student>, StudentService>();
        //builder.Services.AddTransient<IUserRoleModelService<Teacher>, TeacherService>();



        #endregion

        services.AddControllers()
        .AddRazorRuntimeCompilation();

        builder.Services.AddCors();
        services.AddHostedService<RepeatingService>();

        var app = builder.Build();
        //FOR SESSION

        //app.UseCookiePolicy(new CookiePolicyOptions
        //{
        //    MinimumSameSitePolicy = SameSiteMode.None,
        //});

        //app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();
        }
        app.UseCors(opt =>
        {
            opt.AllowAnyMethod();
            opt.AllowAnyHeader();
            opt.AllowAnyOrigin();
        });
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        if (app.Environment.IsProduction())
        {
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
                    context.Response.Redirect("/Home/Error");
                });
            });
        }



        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseCookiePolicy(new CookiePolicyOptions
        //{
        //    MinimumSameSitePolicy = SameSiteMode.None,
        //    Secure = CookieSecurePolicy.Always,
        //    HttpOnly = HttpOnlyPolicy.None,
        //    ExpireTimeSpan = TimeSpan.FromDays(10), // ��������� ����� �������� ���� �� 10 ����
        //    SlidingExpiration = true // ��������� ����������� ��������� ����� ��������
        //});
        //��������� ������� ������������� ���, ������� ��������� ������������ ���.
        app.UseSession(); // use this before .UseEndpoints or .MapControllerRoute //���������� �������� ������������� � �������� ������ ������.
        
        app.UseEndpoints(endpoints =>
		{
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "ThanksRoute",
                pattern: "{controller=Home}/{action=Thanks}",
                defaults: new { controller = "Home", action = "Thanks" });


            endpoints.MapGet("/CRM/InstitutionSetting", context =>
			{
				context.Response.Redirect("/Home/InstitutionSetting");
				return Task.CompletedTask;
			});

            endpoints.MapControllerRoute(
                name: "thanks",
                pattern: "thanks/",
                defaults: new { controller = "Home", action = "Thanks" });

        });


		#region Roles
		using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var roles = roleManager.Roles.ToList();
            string[] roleNames = { "Admin", "Teacher", "Student" };//, "Developer"};
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
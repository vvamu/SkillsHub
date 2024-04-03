global using SkillsHub.Domain.Models;

using EmailProvider;
using EmailProvider.Options;
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



        //FOR SESSION
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options => {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
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
            options.UseSqlServer(connectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging();
        },
        ServiceLifetime.Transient);

        #endregion

        #region Authentication JWT
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


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

        //builder.Services.AddViberBotApi(opt =>
        //{
        //    opt.Token = "5192d0382ea7e2e9-d35f8cb4f9a26339-3a3429d4a23b0c5f";
        //    opt.Webhook = "https://localhost:7150/Viber/Get";
        //});

        builder.Services.AddTransient<EmailProvider.Interfaces.IMailService, MailService>();
        builder.Services.AddTransient<IUserService,UserService>();
        builder.Services.AddTransient<IExternalService,ExternalService>();
        //builder.Services.AddTransient<ICourcesService, CourcesService>();
        builder.Services.AddTransient<IGroupService,GroupService>();
        builder.Services.AddTransient<IRequestService, RequestService>();
        builder.Services.AddTransient<INotificationService,NotificationService>();
        builder.Services.AddScoped<ILessonService, LessonService>();
        builder.Services.AddScoped<ISalaryService, SalaryService>();

        builder.Services.AddScoped<ILessonTypeService, LessonTypeService>();
        builder.Services.AddScoped<IPaymentCategoryService, PaymentCategoryService>();

        #endregion

        services.AddControllers()
        .AddRazorRuntimeCompilation();


        builder.Services.AddCors();
        services.AddHostedService<RepeatingService>();

        var app = builder.Build();
        //FOR SESSION
        // use this before .UseEndpoints or .MapControllerRoute
        app.UseSession();

        //app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Home/Error");\
            app.UseDeveloperExceptionPage();

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

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
            string[] roleNames = { "Admin", "Teacher" ,"Student"};
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
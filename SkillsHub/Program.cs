global using SkillsHub.Domain.Models;

using EmailProvider;
using EmailProvider.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Options;
using SkillsHub.Application.Services.Implementation;
using SkillsHub.Application.Services.Interfaces;
using SkillsHub.Domain.BaseModels;
using SkillsHub.Helpers;
using SkillsHub.Persistence;
using System.Security.Claims;
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
        });

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
        builder.Services.AddTransient<IUserPresentationService, UserPresentationService>();
        builder.Services.AddTransient<IExternalService,ExternalService>();
        builder.Services.AddTransient<IIndexCRMService, IndexCRMService>();
        builder.Services.AddTransient<ICourcesService, CourcesService>();
        builder.Services.AddTransient<IGroupService,GroupService>();
        #endregion

        services.AddControllers(options =>
        {
            options.Conventions.Add(new AuthorizedControllerConvention());
        });


        builder.Services.AddCors();
        var app = builder.Build();
        //app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
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
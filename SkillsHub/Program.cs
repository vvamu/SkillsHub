global using SkillsHub.Domain.Models;

using EmailProvider;
using EmailProvider.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillsHub.Application.Helpers;
using SkillsHub.Application.Options;
using System.Security.Claims;
using Viber.Bot.NetCore.Middleware;

namespace SkillsHub;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

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

        var connectionString = builder.Configuration.GetConnectionString("Sqlite");
        services.AddDbContext<SkillsHub.Persistence.ApplicationDbContext>(
        options =>
        {
            options.UseSqlite(connectionString);
        });

        #endregion


        #region Own Services

        //builder.Services.AddViberBotApi(opt =>
        //{
        //    opt.Token = "5192d0382ea7e2e9-d35f8cb4f9a26339-3a3429d4a23b0c5f";
        //    opt.Webhook = "https://localhost:7150/Viber/Get";
        //});

        builder.Services.AddTransient<EmailProvider.Interfaces.IMailService, MailService>();

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



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");




        app.Run();

    }

    public void ConfigureServices(IServiceCollection services)
    {



    }
}
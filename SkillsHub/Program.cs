using EmailProvider;
using EmailProvider.Options;
using Microsoft.AspNetCore.Identity;
using Viber.Bot.NetCore.Middleware;

namespace SkillsHub;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();


        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();


        #region Services

        //builder.Services.AddViberBotApi(opt =>
        //{
        //    opt.Token = "5192d0382ea7e2e9-d35f8cb4f9a26339-3a3429d4a23b0c5f";
        //    opt.Webhook = "https://localhost:7150/Viber/Get";
        //});

        builder.Services.AddTransient<EmailProvider.Interfaces.IMailService, MailService>();

        #endregion

        builder.Services.AddSingleton(configuration);
        builder.Services.Configure<MailOptions>(configuration.GetSection(MailOptions.OptionName));



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


        #region Roles
        //using (var scope = app.Services.CreateScope())
        //{
        //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var roles = roleManager.Roles.ToList();
        //    string[] roleNames = { "Admin", "User" };
        //    foreach (var roleName in roleNames)
        //    {
        //        if (!await roleManager.RoleExistsAsync(roleName))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }
        //}
        #endregion

        app.Run();

    }
}
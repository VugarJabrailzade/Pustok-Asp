using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Services;
using Pustok.Services.Abstract;
using FluentValidation;
using Pustok.Database.DomainModels;
using FluentValidation.AspNetCore;
using Pustok.Validation;
using Pustok.ViewModels.Product;
using Pustok.Services.Concretes;
using Pustok.Extensions;
using Pustok.Hubs;

namespace Pustok;

public class Program
{
    public static void Main(string[] args)
    {
        //DatabaseService databaseService = new DatabaseService();
        //databaseService.InitializeTables();
        //databaseService.Dispose();

        //string myHtml = "\"<div class=\\\"w3-example\\\">\\r\\n<h3>Example</h3>\\r\\n<div class=\\\"w3-white w3-padding notranslate\\\">\\r\\n<form action=\\\"/action_page.php\\\" target=\\\"_blank\\\">\\r\\n  <label for=\\\"fname\\\">First name:</label><br>\\r\\n  <input type=\\\"text\\\" id=\\\"fname\\\" name=\\\"fname\\\" value=\\\"John\\\"><br>\\r\\n  <label for=\\\"lname\\\">Last name:</label><br>\\r\\n  <input type=\\\"text\\\" id=\\\"lname\\\" name=\\\"lname\\\" value=\\\"Doe\\\"><br><br>\\r\\n  <input type=\\\"submit\\\" value=\\\"Submit\\\">\\r\\n</form> \\r\\n</div>\\r\\n<a class=\\\"w3-btn w3-margin-top w3-margin-bottom\\\" href=\\\"tryit.asp?filename=tryhtml_form_submit\\\" target=\\\"_blank\\\">Try it Yourself »</a>\\r\\n</div>\"";

        //boiler-plate 
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);


        //Middleware pipeline

        var app = builder.Build();

        ConfigureMiddleWareServices(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddConrollers();

        builder.Services.AddAuth();

        builder.Services.AddCustomService();    

        builder.Services.AddEmailConfiguration(builder.Configuration);
        builder.Services.AddSignalR();
    }


    private static void ConfigureMiddleWareServices(WebApplication app)
    {
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllerRoute("default", "{controller=Home}/{action=Index}");

        app.MapHub<AlertHub>("alerthub");

    }


}
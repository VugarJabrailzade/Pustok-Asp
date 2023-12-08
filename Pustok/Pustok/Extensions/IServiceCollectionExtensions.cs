using Microsoft.Extensions.DependencyInjection;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Services.Abstract;
using Pustok.Services.Concretes;
using Pustok.Services;
using System.Security.Cryptography;
using FluentValidation.AspNetCore;
using Pustok.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace Pustok.Extensions;

public static class IServiceCollectionExtensions
{

    public static void AddConrollers(this IServiceCollection services)
    {
        services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation();

    }

    
    public static void AddCustomService(this IServiceCollection serviceCollection)
    {

        serviceCollection
            .AddHttpContextAccessor()
            .AddScoped<IBasketService, BasketService>()
            .AddScoped<IEmailSender, EmailSender>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IEmployeeService, EmployeeServiceImp>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IFileService, FileService>()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryAddResponseViewModel>())
            .AddDbContext<PustokDbContext>(o =>
            {
                o.UseNpgsql(DatabaseConstants.CONNECTION_STRING);
            });

    }

    public static void AddEmailConfiguration( this IServiceCollection services, IConfiguration configuration)
    {
        var emailConfiguration = configuration.GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();

        services.AddSingleton(emailConfiguration);
    }



    public static void AddAuth(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddAuthentication("Cookie").
            AddCookie("Cookie", o =>
            {
                o.LoginPath = "/auth/login";
                o.LogoutPath = "/home/index";
                o.AccessDeniedPath = "/home/index";
            });
    }


}

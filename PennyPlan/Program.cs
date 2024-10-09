using Microsoft.AspNetCore.Authentication.Cookies;
using PennyPlan.Repositories;

namespace PennyPlan
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Add custom repositories
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IBillRepository, BillRepository>();
            builder.Services.AddTransient<ITransactionsRepository, TransactionsRepository>();
            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

            // Configure cookie-based authentication without Entity Framework
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/login"; // Specify login path
                    options.LogoutPath = "/account/logout"; // Specify logout path
                });

            // Add Authorization service
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable Authentication and Authorization
            app.UseAuthentication(); // Enable cookie authentication
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}



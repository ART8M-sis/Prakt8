using Classwork_17_10_2026.Data;
using Classwork_17_10_2026.Mapping;
using Classwork_17_10_2026.Services;
using FileAvatar.Logging;
using FileAvatar.Middleware;
using FileAvatar.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Classwork_17_10_2026;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers(option =>
        {
            option.Filters.Add<ValidationFilterAttribute>();
        });

        // Services
        builder.Services.AddSingleton<IFileRequestLogger, FileRequestLogger>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFileService, FileService>();

        builder.Services.AddAutoMapper(typeof(Program).Assembly);


        // FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
        // AddFluentValidationAutoValidation() — ВИДАЛЕНО (правильно!)

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRequestLogging();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
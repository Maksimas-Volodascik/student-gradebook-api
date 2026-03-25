
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using StackExchange.Redis;
using StudentGradebookApi.Data;
using StudentGradebookApi.Mappings;
using StudentGradebookApi.Middleware;
using StudentGradebookApi.Repositories.ClassesRepository;
using StudentGradebookApi.Repositories.ClassSubjectsRepository;
using StudentGradebookApi.Repositories.EnrollmentsRepository;
using StudentGradebookApi.Repositories.GradesRepository;
using StudentGradebookApi.Repositories.Main;
using StudentGradebookApi.Repositories.StudentsRepository;
using StudentGradebookApi.Repositories.SubjectsRepository;
using StudentGradebookApi.Repositories.TeachersRepository;
using StudentGradebookApi.Repositories.UsersRepository;
using StudentGradebookApi.Services.ClassesServices;
using StudentGradebookApi.Services.EnrollmentsServices;
using StudentGradebookApi.Services.GradesServices;
using StudentGradebookApi.Services.StudentServices;
using StudentGradebookApi.Services.SubjectClassServices;
using StudentGradebookApi.Services.SubjectsService;
using StudentGradebookApi.Services.TeacherServices;
using StudentGradebookApi.Services.UserServices;
using System.Diagnostics;
using System.Text;

namespace StudentGradebookApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            //API Versioning
            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddMvc()
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

            //Lowercase API calls
            builder.Services.AddRouting(opt =>
            {
                opt.LowercaseUrls = true;
            });

            // Database
            builder.Services.AddDbContext<SchoolDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repository and Services
            builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
            builder.Services.AddScoped<ITeachersRepository, TeachersRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IClassesRepository, ClassesRepository>();
            builder.Services.AddScoped<ISubjectsRepository, SubjectsRepository>();
            builder.Services.AddScoped<IClassSubjectsRepository, ClassSubjectsRepository>();
            builder.Services.AddScoped<IEnrollmentsRepository, EnrollmentsRepository>();
            builder.Services.AddScoped<IGradesRepository,  GradesRepository>();

            builder.Services.AddScoped<IGradesServices, GradesServices>();
            builder.Services.AddScoped<IEnrollmentServices,  EnrollmentServices>();
            builder.Services.AddScoped<IClassSubjectsService, ClassSubjectsService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IClassesServices, ClassesServices>();
            builder.Services.AddScoped<ISubjectsService, SubjectsService>();
            
            //Store Logs in File
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            ////Redis cache
            //builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
            //builder.Services.AddHttpClient();

            builder.Services.AddCustomRateLimiting(); //Rate Limit
            
            builder.Services.AddAutoMapper(cfg => { }, typeof(StudentProfile).Assembly); //AutoMapper

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") //frontend URL
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                        ValidAudience = builder.Configuration["AppSettings:Audience"],                        
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))               
                    };
                });
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseCors("AllowFrontend");

            app.UseHttpsRedirection();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseRateLimiter();

            app.MapControllers().RequireRateLimiting("userFixed");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

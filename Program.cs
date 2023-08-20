using ClassroomPlus;
using ClassroomPlus.Data;
using ClassroomPlus.Middlewares;
using ClassroomPlus.Repositories;
using ClassroomPlus.Repositories.Interfaces;
using ClassroomPlus.Services;
using ClassroomPlus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
});

var connectionString = builder.Configuration.GetConnectionString("mysqlserver");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddDbContext<SQLServerContext>(
    options => options.UseMySql(connectionString, serverVersion)
); 

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IClassroomEnrollmentRepository, ClassroomEnrollmentRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IClassroomEnrollmentService, ClassroomEnrollmentService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder
            .WithOrigins("Your frontend url")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(options => 
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication().AddJwtBearer(j =>
j.TokenValidationParameters = new TokenValidationParameters
{
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               builder.Configuration.GetSection("AppSettings:Token").Value!)),
    ValidAudience = "aud",
    ValidIssuer = "https://localhost:7296"
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireId", policy =>
        policy.RequireClaim("Id"));
});


var app = builder.Build();

app.MapHub<NotificationHub>("/notification");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
         Path.Combine(builder.Environment.ContentRootPath, "images")),
    RequestPath = "/images"
});

app.UseCors("MyCorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.AddGlobalErrorHandler();

app.Run();

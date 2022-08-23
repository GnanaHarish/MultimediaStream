using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MultimediaStream.Models;
using MultimediaStream.Services;

var builder = WebApplication.CreateBuilder(args);

//IConfiguration configuration = app.Configuration;
//IWebHostEnvironment environment = app.Environment;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddTransient<IProductRepo, ProductRepo>();
builder.Services.AddTransient<MultimediaStream.Interface.IStreamFileUploadService, StreamFileUploadLocalService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<AppDbContext>(option => option.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(configuration =>
{

}).AddEntityFrameworkStores<AppDbContext>();


//builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "SupportFiles")),
    RequestPath = "/SupportFiles",
    EnableDefaultFiles = true
});

app.UseAuthorization();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();

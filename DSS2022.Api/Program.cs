using AutoMapper;
using DSS2002.Api.Helpers;
using DSS2022.Business;
using DSS2022.Business.Implementation;
using System.Reflection;
using DSS2022.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICollectionService, CollectionService>();

builder.Services.AddDbContext<DSS2022.Data.DSS2022DataContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DSS2022"),
                    b => b.MigrationsAssembly("DSS2022.Api")));



builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

//app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();



app.Run();

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Turismo.Domain.Entities.Entidades;
using Microsoft.EntityFrameworkCore;
using Turismo.Infraestructure.EFDataContext.Context;
using System.Security.Cryptography;
using Turismo.Presentation.WebServices;
/*
var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddDbContext<DBContext>(opt =>
{
    opt.UseSqlServer(Configuration.GetConnectionString("DevConnection"));
});


builder.Services.AddIdentity<Usuario, IdentityRole>()
        .AddEntityFrameworkStores<DBContext>()
        .AddDefaultTokenProviders();


var builderUser =  builder.Services.AddIdentityCore<Usuario>();
builderUser = new IdentityBuilder(builderUser.UserType , builder.Services);
builderUser.AddRoles<IdentityRole>();
builderUser.AddEntityFrameworkStores<DBContext>();
builderUser.AddSignInManager<SignInManager<Usuario>>();



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();*/



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
startup.Configure(app, app.Environment);
app.Run();

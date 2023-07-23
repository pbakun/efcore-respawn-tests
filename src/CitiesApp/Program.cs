using Microsoft.EntityFrameworkCore;
using System.Reflection;
using CitiesApp.Infrastructure;
using CitiesApp.Infrastructure.Database;
using CitiesApp.Infrastructure.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var presentationAssembly = typeof(CitiesApp.Presentation.AssemblyReference).Assembly;
builder.Services.AddControllers().AddApplicationPart(presentationAssembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string postgresConnString = builder.Configuration.GetConnectionString("postgresql") 
    ?? throw new MissingApplicationConfigurationException(nameof(postgresConnString));

builder.Services.AddInfrastructure(postgresConnString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using(IServiceScope scope = app.Services.CreateScope())
{
    ApplicationStartup.Initialize(scope);
}


app.Run();

public partial class Program { }
using Api.Data;
using Api.Mapping;
using AutoMapper;
using Core.Data;
using Core.Models;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<WidgetContext>();

builder.Services.AddScoped<IMapper>(x => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<WidgetMappingProfile>())));
builder.Services.AddScoped<IWidgetCleanupService, WidgetCleanupService>();
builder.Services.AddScoped<IRepository<Widget>, WidgetRepo>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();

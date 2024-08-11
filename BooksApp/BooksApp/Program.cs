using AutoMapper;
using BooksApp;
using BooksApp.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connString = builder.Configuration.GetConnectionString("BookStoreDB");
builder.Services.AddDbContext<BookAppStoreContext>(option =>
{
    option.UseSqlServer(connString);
});

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Host.UseSerilog((context, lc) =>
lc.WriteTo.Console().ReadFrom.Configuration(context.Configuration));
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll", b => b.AllowAnyMethod().
                                    AllowAnyHeader().
                                    AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

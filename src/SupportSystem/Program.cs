using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SupportSystem.Data;
using SupportSystem.DTOs;
using SupportSystem.DTOs.Validators;
using SupportSystem.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SupportSystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<ITicketsService, TicketsService>();
builder.Services.AddScoped<IValidator<NewTicketDTO>, NewTicketDTOValidator>();
builder.Services.AddScoped<IValidator<NewCommentDTO>, NewCommentDTOValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

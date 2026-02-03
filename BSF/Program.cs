using Infrastructure.Context;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<BSFContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

// Seed data
if (app.Environment.IsDevelopment())
{
    UserSeedData.UserSeed(app.Services);
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

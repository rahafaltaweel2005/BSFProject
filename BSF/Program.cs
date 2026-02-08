using System.Text;
using Application.Repositories;
using Application.Sarvices.AuthService;
using Application.Services.Service;
using Application.Services.ClientUserService;
using Application.Services.CurrentUserService;
using Application.Services.LookupService;
using Application.Services.ServiceProviderServices;
using Infrastructure.Context;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Servicesss.CurrentUserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Application.Services.OrderService;
using Application.Services.NotificationService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<BSFContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"])),

        };
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BSF API",
        Version = "v1"
    });


    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put **_ONLY_** your JWT Bearer token hera",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityReq = new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { } }
    };
    c.AddSecurityRequirement(securityReq);
});
builder.Services.AddHttpContextAccessor();//
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IAuthService),typeof(AuthService));
builder.Services.AddScoped(typeof(ICurrentUserService),typeof(CurrentUserService));
builder.Services.AddScoped(typeof(IServiceProviderService),typeof(ServiceProviderService));
builder.Services.AddScoped(typeof(ILookupService), typeof(LookupService));
builder.Services.AddScoped(typeof(IClientUserService), typeof(ClientUserService));
builder.Services.AddScoped(typeof(IServicesService), typeof(ServicesService));
builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
builder.Services.AddScoped(typeof(INotificationService), typeof(NotificationService));








var app = builder.Build();

// Seed data

    UserSeedData.UserSeed(app.Services);


app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

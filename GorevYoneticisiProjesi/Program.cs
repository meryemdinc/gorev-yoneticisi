using GorevYoneticisiProjesi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;    
using System.Text;
using GorevYoneticisiProjesi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); //defaultConnection appsettings.json'den okunuyor.

//  Authentication(kullanıcı girişi),JWT ayarları
var jwtSettings = builder.Configuration.GetSection("Jwt");
//appsettings.json dosyasından jwt ayarlarını alıyoruz,token ile ilgili bilgiler burada.JWT token'ını dogrulama kısmı
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
            ClockSkew = TimeSpan.FromMinutes(2)
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var auth = ctx.Request.Headers["Authorization"].ToString();
                Console.WriteLine($"[JWT] OnMessageReceived Authorization header: '{auth}'");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"[JWT] OnAuthenticationFailed: {ctx.Exception?.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine($"[JWT] OnTokenValidated: {ctx.Principal?.Identity?.Name ?? "no-name-claim"}");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

//API için contreller sınıflarının kullanılacağı bildiriliyor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Görev Yöneticisi API", Version = "v1" });

   
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token} biçiminde JWT girin"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//servisler her istek geldiğinde hazır olacak şekilde projeye ekleniyor.
builder.Services.AddScoped <IUserService, UserService>();
builder.Services.AddScoped <ITaskReportService, TaskReportService>();

// uygulamanın çalıştırılması
//app.Build() ile  uygulama oluşturuluyor.Swagger arayüzü açılıyor.
var app = builder.Build(); 


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//gelen isteğin kimlik doğrulaması yapılıyor
app.UseAuthentication();  
//yetki kontrolü yapılıyor
app.UseAuthorization();
//API 'deki tüm controllerlar aktif ediliyor
app.MapControllers();

app.Run();

//Controller = kapı,Service = işi yapan,controller yönlendiriyor,Service iş yapıyor.
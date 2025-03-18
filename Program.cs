
// con el administrador de paquetes nuguet si queremos crear una migracion tenemos que ejecutar :
// Add-Migration NombreDeLaMigracion
// Y PARA QUE SE REFLEJE EN LA BASE DE DATOS en la misma consola del administrador de paquetes luego de ejecutar Add-Migration Nombre
// ejecutamos : Update-database
// y con eso todo deberia funcionar muy bien
// con el administrador de paquetes nuguet si queremos remover la ultima migracion debemos ejecutar Remove-Migration


// con el administrador de paquetes nuguet si queremos remover la ultima migracion debemos ejecutar Remove-Migration
using MiBlog.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;

using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using MiBlog.Servicios;

var builder = WebApplication.CreateBuilder(args);

var CadenaDeConexion = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbBlogContext>(options => {
    options.UseSqlServer(CadenaDeConexion);
});

//agregamos automaper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddScoped<JWTService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// configuracion de Jwt 
// si no esta configurado va saltar este error
var key = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key no está configurada");
// si esta configurado va proceder aca
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();// Jwt
app.UseAuthorization();

app.MapControllers();

app.Run();

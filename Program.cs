
// con el administrador de paquetes nuguet si queremos crear una migracion tenemos que ejecutar :
// Add-Migration NombreDeLaMigracion
// Y PARA QUE SE REFLEJE EN LA BASE DE DATOS en la misma consola del administrador de paquetes luego de ejecutar Add-Migration Nombre
// ejecutamos : Update-database
// y con eso todo deberia funcionar muy bien
// con el administrador de paquetes nuguet si queremos remover la ultima migracion debemos ejecutar Remove-Migration


// con el administrador de paquetes nuguet si queremos remover la ultima migracion debemos ejecutar Remove-Migration
using MiBlog.AppDbContext;
using Microsoft.EntityFrameworkCore;

using System;

var builder = WebApplication.CreateBuilder(args);

var CadenaDeConexion = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbBlogContext>(options => {
    options.UseSqlServer(CadenaDeConexion);
});

//agregamos automaper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
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

app.Run();

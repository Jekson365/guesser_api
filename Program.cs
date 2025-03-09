using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Repositories;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.WithOrigins(Env.GetString("CORS"))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseNpgsql(
        $"Server={Env.GetString("SERVER_NAME")};PORT={Env.GetInt("PORT")};Database={Env.GetString("DB_NAME")};User Id={Env.GetString("USER")};Password={Env.GetString("PASSWORD")}"
    );
});

builder.Services.AddScoped<IImageInterface, ImageRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

app.MapControllers();

app.Run();

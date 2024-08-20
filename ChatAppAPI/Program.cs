using ChatAppAPI;
using ChatDBLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorFront", policy =>
    {
        policy.WithOrigins("https://localhost:7153")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});
builder.Services.AddSingleton<LoggedUsers>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("BlazorFront");

app.MapHub<Mainhub>("/mainhub");

app.MapGet("/", (HttpContext cntx, LoggedUsers users) =>
{
    int newuserid = 0;
    users.users.TryAdd(newuserid, new UserContext { Id = newuserid++ });
    return Results.Ok();
});

app.MapGet("/users", (HttpContext http) =>
{
    UserDA userDA = new UserDA("Server=127.0.0.1; Port=5432; Database=public; User Id=postgres; Password=heroO726;");
    var users = userDA.GetUsers();

    http.Response.StatusCode = 200;
    return users;
})
.WithDescription("Get all users from db");

app.Run();

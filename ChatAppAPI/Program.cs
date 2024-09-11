using ChatAppAPI;
using ChatDBLibrary;
using ChatDBLibrary.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
//checking
// Load configuration
var configuration = builder.Configuration;

// Register ApplicationDbContext with DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Register other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorFront", policy =>
    {
        policy.WithOrigins("https://localhost:5041")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

// Register UserDA with DI
builder.Services.AddScoped<UserDA>(provider =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new UserDA(connectionString);
});

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

// Apply CORS policy
app.UseCors("BlazorFront");

app.UseAuthorization();

// Map controllers
app.MapControllers();

// Map other endpoints
app.MapGet("/users", async (HttpContext http) =>
{
    var currentUserIdStr = http.Request.Query["currentUserId"];
    if (!int.TryParse(currentUserIdStr, out int currentUserId))
    {
        return Results.BadRequest("Invalid or missing currentUserId");
    }

    try
    {
        var userDA = http.RequestServices.GetRequiredService<UserDA>();
        var users = userDA.GetUsers(currentUserId);
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching users: {ex.Message}");
        return Results.Problem($"Error fetching users: {ex.Message}", statusCode: 500);
    }
})
.WithDescription("Get users who have interacted with the current user");

app.MapGet("/messages", async (HttpContext http) =>
{
    var senderIdStr = http.Request.Query["sender_id"];
    var receiverIdStr = http.Request.Query["receiver_id"];

    if (!int.TryParse(senderIdStr, out int senderId) || !int.TryParse(receiverIdStr, out int receiverId))
    {
        return Results.BadRequest("Invalid or missing sender_id or receiver_id");
    }

    try
    {
        var userDA = http.RequestServices.GetRequiredService<UserDA>();
        var messages = userDA.GetMessages(senderId, receiverId);
        return Results.Ok(messages);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching messages: {ex.Message}");
        return Results.Problem($"Error fetching messages: {ex.Message}", statusCode: 500);
    }
})
.WithDescription("Get messages between two users from db");

app.MapPost("/addmessages", async (HttpContext http) =>
{
    var userDA = http.RequestServices.GetRequiredService<UserDA>();

    var message = await http.Request.ReadFromJsonAsync<Messages>();

    if (message == null || message.Sender_id <= 0 || message.Receiver_id <= 0 || string.IsNullOrWhiteSpace(message.Message))
    {
        http.Response.StatusCode = 400; // Bad Request
        return "Invalid message data";
    }

    try
    {
        userDA.AddMessage(message);
        http.Response.StatusCode = 201;
        return "Message added successfully";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error adding message: {ex.Message}");
        http.Response.StatusCode = 500;
        return "Error adding message";
    }
})
.WithDescription("Add a new message to the db");

app.Run();

using MongoDB.Driver;
using Microsoft.Extensions.Options;
using backend.Models;
using backend.Services;
using backend.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var myAllowSpecificOrigins = "AllowNextJsApp";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                          policy.WithOrigins("http://170.81.43.121:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDbConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        connectionString = settings.ConnectionString;
    }

    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddControllers();

builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IArticleService, ArticleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();
app.Run();
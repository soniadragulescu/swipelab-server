using NewRelic.LogEnrichers.Serilog;
using Serilog;
using SwipeLab.Data.Postgres.Configuration;
using SwipeLab.Feedback.Configuration;
using SwipeLab.Helpers;
using SwipeLab.Middleware;
using SwipeLab.Services.Configuration;
using SwipeLab.Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwipeLabDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

// Add logging
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithNewRelicLogsInContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
    .WriteTo.NewRelicLogs()
    .CreateLogger();

builder.Host.UseSerilog();

var geminiConfiguration = builder.Configuration
    .GetSection("Gemini")
    .Get<GeminiConfiguration>();

if (geminiConfiguration != null)
{
    builder.Services.AddSingleton(geminiConfiguration);
}

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddSwipeLabRepositories();
builder.Services.AddServices();
builder.Services.AddScoped<ContextLoggingMiddleware>();

var connectionString = builder.Configuration.GetValue<string>("AzureStorage:ConnectionString");
var containerName = builder.Configuration.GetValue<string>("AzureStorage:ContainerName");

if (!string.IsNullOrWhiteSpace(connectionString) && !string.IsNullOrWhiteSpace(containerName))
{
    builder.Services.AddSingleton(new AzureStorageAccountConfiguration(connectionString, containerName));
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

try
{
    app.Services.RunMigrations();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error during migrations.");
}

app.UseMiddleware<ContextLoggingMiddleware>();
app.UseMiddleware<RequestBodyLoggerMiddleware>();

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
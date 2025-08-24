using CSharpBankingApp.Services;
using CSharpBankingApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Bank service as singleton
builder.Services.AddSingleton<BankService>();

var app = builder.Build();

// Configure to listen on all interfaces
app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:5000");
app.Urls.Add("https://0.0.0.0:5001");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Commented out for easier local development

// Enable CORS
app.UseCors("AllowAll");

// Serve static files (HTML, CSS, JS)
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Serve the main HTML file for the root route
app.MapFallbackToFile("index.html");

app.Run();
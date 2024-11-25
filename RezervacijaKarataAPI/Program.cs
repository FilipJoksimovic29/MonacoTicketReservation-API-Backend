using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RezervacijaKarata.DataAccess;
using RezervacijaKarata.Services;
using RezervacijaKarata.Services.Interfaces; // Uklju?ite namespace gde se nalaze vaši servisi


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rezervacija Karata API", Version = "v1" });
});

// Configure DbContext with SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRaceDayService, RaceDayService>();
builder.Services.AddScoped<ISeatingZoneService, SeatingZoneService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        builder => builder.WithOrigins("http://localhost:3000") // Replace with the actual URL of your frontend app
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); // Allow credentials if your frontend needs to send credentials like cookies or auth headers
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rezervacija Karata API v1"));
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowWebApp");

app.UseAuthorization();

app.MapControllers();

app.Run();

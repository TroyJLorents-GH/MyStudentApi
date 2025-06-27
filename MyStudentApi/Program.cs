using Microsoft.EntityFrameworkCore;
using MyStudentApi.Data;

var builder = WebApplication.CreateBuilder(args);

//  services to the container.
builder.Services.AddControllers();

// Add CORS policy to requests from the React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://icy-moss-0ce7c881e.2.azurestaticapps.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register EF Core and configure your database connection.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Swagger services.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


// Use the CORS policy.
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

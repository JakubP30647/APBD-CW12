using APBD_CW12.Data;
using APBD_CW12.Data;
using APBD_CW12.Services;
using APBD_CW12.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi



builder.Services.AddControllers();



builder.Services.AddDbContext<ApbdCw12Context>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<ITripsService, TripsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
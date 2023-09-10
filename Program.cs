using Microsoft.EntityFrameworkCore;
using TwitterAPI.Infrastructure.Persistence;
using TwitterAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================================================================
var psqlDbSettings = builder.Configuration.GetSection(nameof(PostgresDbSettings)).Get<PostgresDbSettings>();
builder.Services.AddSingleton(new PostgresTwitterRepository(psqlDbSettings.ConnectionString));
builder.Services.AddDbContext<PostgresTwitterRepository>();
// =============================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

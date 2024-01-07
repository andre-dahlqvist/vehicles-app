using Microsoft.EntityFrameworkCore;
using VehiclesApi.Data;
using VehiclesApi.Services;

var builder = WebApplication.CreateBuilder(args);
var AllowCorsOnLocalhost = "_allowCorsOnLocalhost";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowCorsOnLocalhost,
		builder =>
		{
			builder.WithOrigins("http://localhost:4200")
					.AllowAnyHeader()
					.AllowAnyMethod();
		});
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVehiclesService, VehiclesService>();
builder.Services.AddScoped<IBrandsService, BrandsService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddDbContext<VehiclesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(AllowCorsOnLocalhost);

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
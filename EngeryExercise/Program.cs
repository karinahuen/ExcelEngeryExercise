using Microsoft.EntityFrameworkCore;
using EngeryExercise.Data;

var builder = WebApplication.CreateBuilder(args);


//set microsoft SQL Server database
builder.Services.AddDbContext<EnergyExerciseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EnergyExerciseContext") ?? throw new InvalidOperationException("Connection string 'EnergyExerciseContext' not found.")));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

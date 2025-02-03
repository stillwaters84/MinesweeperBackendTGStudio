using Microsoft.EntityFrameworkCore;
using StudioTGMinesweeperService.Interfaces;
using StudioTGMinesweeperService.Repositories;
using StudioTGMinesweeperService.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://minesweeper-test.studiotg.ru");
                          policy.AllowAnyHeader();
                      });
});

// Add services to the container.
builder.Services.AddDbContext<StudioTGMinesweeperService.Contexts.MinesweeperContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<INewGameRepository, NewGameRepository>();
builder.Services.AddScoped<INewGameRequestService, NewGameRequestService>();

builder.Services.AddScoped<IGameTurnRepository, GameTurnRepository>();
builder.Services.AddScoped<IGameTurnRequestService, GameTurnRequestService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

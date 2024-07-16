using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using PostgresDb;
using Service2.Repositories;
using static Common.Extensions.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMySerilog("http://127.0.0.1:4317", Service2Name);

builder.Services.AddMyOpenTelemetry("http://127.0.0.1:4317", Service2Name);

builder.Services.AddScoped<IUserRepository, UserRepository>();

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("LocalPostgres");

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var serviceProvider = builder.Services.BuildServiceProvider();
var dbContext = serviceProvider.GetRequiredService<MyContext>();

dbContext.Database.Migrate();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTraceId();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunWithLogging();

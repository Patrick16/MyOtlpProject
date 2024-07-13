using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using PostgresDb;
using Serilog;
using static Common.Extensions.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMySerilog("http://127.0.0.1:4317", Service2Name);

builder.Services.AddMyOpenTelemetry(Service2Name);


var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("LocalPostgres");
builder.Services.AddDbContext<MyContext>(builder =>
{
    Log.Information("Start Migration");
    builder.UseNpgsql(connectionString);
    using var context = new MyContext(builder.Options);
    context.Database.EnsureCreated();
    context.Database.Migrate();    
    Log.Information("End Migration");
});


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

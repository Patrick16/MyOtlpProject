using Common.Extensions;
using OpenTelemetry.Resources;
using static Common.Extensions.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ResourceBuilder resource = ResourceBuilder.CreateDefault().AddService(Service2Name);

builder.Services.AddMySerilog(resource);

builder.Services.AddMyOpenTelemetry(Service2Name);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RunWithLogging();

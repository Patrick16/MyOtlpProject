using Common.Extensions;
using OpenTelemetry.Resources;
using static Common.Extensions.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("myapp2",
        x => {
            x.BaseAddress = new Uri("http://localhost:8082");
        });

ResourceBuilder resource = ResourceBuilder.CreateDefault().AddService(Service1Name);

builder.Services.AddMySerilog(resource);

builder.Services.AddMyOpenTelemetry(Service1Name);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.RunWithLogging();



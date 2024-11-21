using Common.Extensions;
using static Common.Extensions.Constants;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("myapp2",
        x => {
            x.BaseAddress = new Uri("http://localhost:8082");
        });


builder.Services.AddMySerilog("http://127.0.0.1:4317", Service1Name);

builder.Services.AddMyOpenTelemetry("http://127.0.0.1:4317", Service1Name);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTraceId();

app.UseAuthorization();

app.MapControllers();

app.RunWithLogging();



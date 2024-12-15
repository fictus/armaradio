using arma_historycompiler;
using arma_historycompiler.Data;
using arma_historycompiler.Services;

var builder = Host.CreateApplicationBuilder(args);
IHostEnvironment hostEnvironment = builder.Environment;
IConfiguration hostContext = builder.Configuration;

builder.Services.AddSingleton(hostEnvironment);
builder.Services.AddSingleton(hostContext);
builder.Services.AddTransient<IDapperHelper, DapperHelper>();
builder.Services.AddTransient<IArmaHistoryService, ArmaHistoryService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

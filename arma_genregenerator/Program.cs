using arma_genregenerator;
using arma_genregenerator.Data;
using arma_genregenerator.Services;

var builder = Host.CreateApplicationBuilder(args);
IHostEnvironment hostEnvironment = builder.Environment;
IConfiguration hostContext = builder.Configuration;

builder.Services.AddSingleton(hostEnvironment);
builder.Services.AddSingleton(hostContext);
builder.Services.AddTransient<IDapperHelper, DapperHelper>();
builder.Services.AddTransient<IArmaGenresService, ArmaGenresService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

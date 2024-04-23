using arma_miner;
using arma_miner.Data;
using arma_miner.Operations;
using arma_miner.Service;

var builder = Host.CreateApplicationBuilder(args);
IHostEnvironment hostEnvironment = builder.Environment;
IConfiguration hostContext = builder.Configuration;

builder.Services.AddSingleton(hostEnvironment);
builder.Services.AddSingleton(hostContext);
builder.Services.AddTransient<IDapperHelper, DapperHelper>();
builder.Services.AddTransient<IArmaMinerService, ArmaMinerService>();
builder.Services.AddTransient<IArmaArtistsOperations, ArmaArtistsOperations>();
builder.Services.AddTransient<IArmaAlbumsOperations, ArmaAlbumsOperations>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

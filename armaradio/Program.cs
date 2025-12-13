using armaradio.ArmaSmtp;
using armaradio.Data;
using armaradio.Operations;
using armaradio.Repositories;
using armaradio.Tools;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Runtime.InteropServices;
using YoutubeDLSharp;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment hostEnvironment = builder.Environment;

if (OperatingSystem.IsLinux())
{
    builder.Host.UseSystemd();
}

//string url = $"https://spotalike.com/en";

//var browserFetcher = new BrowserFetcher();
//browserFetcher.Browser = SupportedBrowser.Firefox;
//browserFetcher.DownloadAsync().Wait();

//IBrowser _browser = Puppeteer.LaunchAsync(new LaunchOptions
//{
//    Headless = true,
//    Browser = SupportedBrowser.Firefox
//}).Result;

//IPage _page = _browser.NewPageAsync().Result;
//_page.GoToAsync(url);

//builder.Services.AddSingleton<IPage>(_page);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(hostEnvironment);
builder.Services.AddScoped<SignInManager<IdentityUser>, SignInManager<IdentityUser>>();
builder.Services.AddScoped<UserManager<IdentityUser>, UserManager<IdentityUser>>();
builder.Services.AddTransient<IDapperHelper, DapperHelper>();
builder.Services.AddTransient<IMusicRepo, MusicRepo>();
builder.Services.AddTransient<IArmaWebRequest, ArmaWebRequest>();
builder.Services.AddTransient<IArmaAuth, ArmaAuth>();
builder.Services.AddTransient<IArmaEmail, ArmaEmail>();
builder.Services.AddTransient<ITeraboxUploader, TeraboxUploader>();
builder.Services.AddScoped<ArmaUserOperation>();

builder.Services.AddSingleton<YoutubeDL>(sp =>
{
    bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    //if (isLinux)
    //{
    //    Process.Start("chmod", $"777 /var/www/armaradio/wwwroot/AudioFiles").WaitForExit();
    //}

    return new YoutubeDL
    {
        YoutubeDLPath = (isLinux ? "/home/fictus/yt-dlp-env/bin/yt-dlp" : "C:\\YTDL\\yt-dlp.exe"), //"/usr/local/bin/yt-dlp-wrapper"   "/home/fictus/.local/bin/yt-dlp"  "/usr/local/bin/yt-dlp-wrapper"
        FFmpegPath = (isLinux ? "/usr/bin/ffmpeg" : "C:\\ffmpeg\\ffmpeg.exe")
    };
});

builder.Services.AddTransient<ArmaYoutubeDownloader>();
builder.Services.AddTransient<IArmaAudioDownloader, ArmaAudioDownloader>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("adminconn"));
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
    // If you're behind a reverse proxy or load balancer, you might need to clear and add
    // your proxy server's IP range:
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
    // options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("10.0.0.0"), 8));
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<DataContext>();

var app = builder.Build();

// Initialize cookie file
var rootPath = app.Environment.ContentRootPath;
var cookiesDir = Path.Combine(rootPath, "wwwroot", "cookies");
var cookiesFile = Path.Combine(cookiesDir, "file.txt");

if (!Directory.Exists(cookiesDir))
{
    Directory.CreateDirectory(cookiesDir);
}

if (File.Exists(cookiesFile))
{
    File.Delete(cookiesFile);
}

using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("radioconn");

    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();

        var command = new SqlCommand("select [ConfigValue] from ApplicationConfigurations where [ConfigName] = 'YTCookie'", connection);
        var cookieValue = command.ExecuteScalar()?.ToString();

        if (!string.IsNullOrEmpty(cookieValue))
        {
            File.WriteAllText(cookiesFile, cookieValue);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                try
                {
                    // Using UnixFileMode (available in .NET 6+)
                    File.SetUnixFileMode(cookiesFile,
                        UnixFileMode.UserRead | UnixFileMode.UserWrite |
                        UnixFileMode.GroupRead | UnixFileMode.OtherRead);
                }
                catch (Exception ex)
                {
                    // Log the error but don't fail the startup
                    Console.WriteLine($"Warning: Could not set cookie file permissions: {ex.Message}");
                }
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("ApiPolicy");

app.UseAuthorization();

//app.MapControllers();

//app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
//    [FromBody] object empty) =>
//{
//    if (empty != null)
//    {
//        await signInManager.SignOutAsync();
//        return Results.Ok();
//    }
//    return Results.Unauthorized();
//})
//.RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using armaradio.Data;
using armaradio.Repositories;
using armaradio.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using PuppeteerSharp;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment hostEnvironment = builder.Environment;
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
builder.Services.AddTransient<IDapperHelper, DapperHelper>();
builder.Services.AddTransient<IMusicRepo, MusicRepo>();
builder.Services.AddTransient<IArmaWebRequest, ArmaWebRequest>();
builder.Services.AddTransient<IArmaAuth, ArmaAuth>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("adminconn"));
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<DataContext>();

var app = builder.Build();

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

app.UseAuthorization();

//app.MapControllers();

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
})
//.WithOpenApi()
.RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

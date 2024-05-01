using PuppeteerSharp;

var builder = WebApplication.CreateBuilder(args);

string url = $"https://open.spotify.com/search";

var browserFetcher = new BrowserFetcher();
browserFetcher.Browser = SupportedBrowser.Firefox;
browserFetcher.DownloadAsync().Wait();

IBrowser _browser = Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = true,
    Browser = SupportedBrowser.Firefox
}).Result;

IPage _page = _browser.NewPageAsync().Result;
_page.GoToAsync(url);

builder.Services.AddSingleton<IPage>(_page);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();

//app.Urls.Add("http://*:5003");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using armaradio.Models.Odysee;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;

namespace armaradio.Repositories
{
    public class OdyseeSearcher : IDisposable
    {
        private IWebDriver _driver;
        private const string BASE_URL = "https://odysee.com";

        public OdyseeSearcher(bool headless = true)
        {
            var options = new ChromeOptions();

            if (headless)
            {
                options.AddArgument("--headless");
            }

            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Searches Odysee using Selenium to handle JavaScript rendering
        /// </summary>
        public async Task<List<OdyseeSearchResult>> SearchAsync(string searchQuery, int maxResults = 10)
        {
            var results = new List<OdyseeSearchResult>();

            try
            {
                // Navigate to search URL
                string encodedQuery = Uri.EscapeDataString(searchQuery);
                string searchUrl = $"{BASE_URL}/$/search?q={encodedQuery}";

                Console.WriteLine($"Navigating to: {searchUrl}");
                _driver.Navigate().GoToUrl(searchUrl);

                // Wait for search results to load
                //Console.WriteLine("Waiting for results to load...");
                //await Task.Delay(3000); // Give it time to load

                // Try multiple selectors for search results
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

                try
                {
                    wait.Until(d => d.FindElements(By.CssSelector(
                        "div.main--empty.help"
                    )).Count > 0);

                    //wait.Until(d => d.FindElements(By.CssSelector(
                    //    "a[href*='/@'], .claim-preview, .claim-tile, [class*='ClaimPreview']"
                    //)).Count > 0);
                }
                catch
                {
                    Console.WriteLine("Timeout waiting for results");
                }

                // Additional wait for dynamic content
                //await Task.Delay(2000);

                // Find all video links
                var videoElements = _driver.FindElements(By.CssSelector("li[role='link']")).ToList();

                //var videoElements = _driver.FindElements(By.CssSelector("a[href*='/@']"))
                //    .Where(e => {
                //        var href = e.GetAttribute("href");
                //        return !string.IsNullOrEmpty(href) &&
                //               href.Contains("/@") &&
                //               href.Contains(":") &&
                //               !href.EndsWith("/@");
                //    })
                //    .Take(maxResults)
                //    .ToList();

                Console.WriteLine($"Found {videoElements.Count} potential video links");

                foreach (var element in videoElements)
                {
                    try
                    {
                        var result = new OdyseeSearchResult();
                        var parentDiv = element.FindElement(By.CssSelector("div.claim-preview__text"));
                        var fileIdDiv = element.FindElement(By.CssSelector("div.claim-preview__background"));

                        string fileId = fileIdDiv.GetAttribute("style") ?? "";
                        fileId = ((fileId.Split('/').LastOrDefault() ?? "").Split('"').FirstOrDefault() ?? "").Split('.').FirstOrDefault() ?? "";

                        var songALink = parentDiv.FindElement(By.CssSelector("div.claim-preview-info")).FindElement(By.CssSelector("a"));

                        result.Url = songALink.GetAttribute("href");
                        result.FileId = fileId;

                        result.Title = element.FindElement(By.CssSelector("span.truncated-text")).GetAttribute("title");
                        if (string.IsNullOrEmpty(result.Title))
                        {
                            result.Title = songALink.Text?.Trim();
                        }
                        if (string.IsNullOrEmpty(result.Title))
                        {
                            result.Title = songALink.GetAttribute("aria-label");
                        }

                        var channelDiv = parentDiv.FindElement(By.CssSelector("div.claim-tile__info"));
                        result.ChannelName = channelDiv.FindElement(By.CssSelector("span.channel-name")).Text?.Trim();

                        result.Artist = ExtractArtistFromTitle(result.Title);
                        if (string.IsNullOrEmpty(result.Artist) && !string.IsNullOrEmpty(result.ChannelName))
                        {
                            result.Artist = result.ChannelName;
                        }

                        if (!string.IsNullOrEmpty(result.Url) && !string.IsNullOrEmpty(result.Title))
                        {
                            // Avoid duplicates
                            if (!results.Any(r => r.Url == result.Url))
                            {
                                results.Add(result);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing element: {ex.Message}");
                    }
                }

                Console.WriteLine($"Successfully parsed {results.Count} results");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return results;
        }

        private string ExtractArtistFromTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return "";

            // Pattern 1: "Artist - Song"
            var dashMatch = Regex.Match(title, @"^([^-]+)\s*-\s*(.+)$");
            if (dashMatch.Success)
            {
                return dashMatch.Groups[1].Value.Trim();
            }

            // Pattern 2: "Song by Artist"
            var byMatch = Regex.Match(title, @"\s+by\s+(.+)$", RegexOptions.IgnoreCase);
            if (byMatch.Success)
            {
                return byMatch.Groups[1].Value.Trim();
            }

            // Pattern 3: "Artist: Song"
            var colonMatch = Regex.Match(title, @"^([^:]+):\s*(.+)$");
            if (colonMatch.Success)
            {
                return colonMatch.Groups[1].Value.Trim();
            }

            // Pattern 4: Artist in parentheses or brackets at the end
            var bracketMatch = Regex.Match(title, @"[\[\(]([^\]\)]+)[\]\)]$");
            if (bracketMatch.Success)
            {
                return bracketMatch.Groups[1].Value.Trim();
            }

            return "";
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}

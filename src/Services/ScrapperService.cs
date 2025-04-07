using PuppeteerSharp;
using System.Text.Json;
using backend.Models;

namespace backend.Services{
    public class ScrapperService
    {
        public async Task<List<object>> GetScrapperAsync(string name, string source)
        {
            try
            {
                return source switch
                {
                    "WorldBank" => await GetFromWorldBankAsync(name),
                    "OFAC" => await GetFromOFACAsync(name),
                    "LeaksDatabase" => new List<object>(),
                    _ => new List<object>()
                };
            }
            catch (PuppeteerException ex)
            {
                Console.WriteLine($"Puppeteer error: {ex.Message}");
                return new List<object>();
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout: {ex.Message}");
                return new List<object>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return new List<object>();
            }
        }

        private async Task<List<object>> GetFromWorldBankAsync(string name)
        {
            await EnsureBrowserIsDownloaded();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();

            await NavigateToPageAsync(page, "https://projects.worldbank.org/en/projects-operations/procurement/debarred-firms");
            await page.WaitForSelectorAsync("#category");
            await page.WaitForSelectorAsync("#k-debarred-firms .k-grid-content table tbody tr");

            await page.TypeAsync("#category", name);
            await WaitForStableResultsAsync(page, "#k-debarred-firms .k-grid-content table tbody tr");

            var jsonData = await page.EvaluateFunctionAsync(GetWorldBankScript());
            var resultList = JsonSerializer.Deserialize<List<WorldBankResponse>>(jsonData?.ToString() ?? "[]");

            return resultList?.Cast<object>().ToList() ?? new List<object>();
        }

        private async Task<List<object>> GetFromOFACAsync(string name)
        {
            await EnsureBrowserIsDownloaded();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();

            await NavigateToPageAsync(page, "https://sanctionssearch.ofac.treas.gov/", 300000);
            await page.TypeAsync("#ctl00_MainContent_txtLastName", name);
            await page.ClickAsync("#ctl00_MainContent_btnSearch");
            await page.WaitForSelectorAsync("#gvSearchResults");

            await WaitForStableResultsAsync(page, "#gvSearchResults tr");

            var jsonData = await page.EvaluateFunctionAsync(GetOFACScript());
            var resultList = JsonSerializer.Deserialize<List<OfacResponse>>(jsonData?.ToString() ?? "[]");

            return resultList?.Cast<object>().ToList() ?? new List<object>();
        }

        private async Task EnsureBrowserIsDownloaded()
        {
            await new BrowserFetcher().DownloadAsync();
        }

        private async Task NavigateToPageAsync(IPage page, string url, int timeout = 30000)
        {
            await page.GoToAsync(url, new NavigationOptions
            {
                Timeout = timeout,
                WaitUntil = new[] { WaitUntilNavigation.Networkidle2 }
            });
        }

        private async Task WaitForStableResultsAsync(IPage page, string selector)
        {
            await page.EvaluateFunctionAsync(@"(selector) => new Promise(resolve => {
                let lastCount = -1;
                let stableCount = 0;
                const interval = setInterval(() => {
                    const currentCount = document.querySelectorAll(selector).length;
                    if (currentCount === lastCount) {
                        stableCount++;
                        if (stableCount >= 3) {
                            clearInterval(interval);
                            resolve(true);
                        }
                    } else {
                        stableCount = 0;
                        lastCount = currentCount;
                    }
                }, 1000);
            })", selector);
        }

        private string GetWorldBankScript()
        {
            return @"() => {
                const rows = document.querySelectorAll('#k-debarred-firms .k-grid-content table tbody tr');
                return Array.from(rows).map(row => {
                    const cells = row.querySelectorAll('td');
                    return cells.length > 0 ? {
                        web: 'WorldBank',
                        firmName: cells[0]?.innerText?.trim(),
                        address: cells[2]?.innerText?.trim(),
                        country: cells[3]?.innerText?.trim(),
                        fromDate: cells[4]?.innerText?.trim(),
                        toDate: cells[5]?.innerText?.trim(),
                        grounds: cells[6]?.innerText?.trim()
                    } : null;
                }).filter(row => row !== null);
            }";
        }

        private string GetOFACScript()
        {
            return @"() => {
                const rows = document.querySelectorAll('#gvSearchResults tr');
                return Array.from(rows).map(row => {
                    const cells = row.querySelectorAll('td');
                    return cells.length > 0 ? {
                        name: cells[0]?.innerText?.trim(),
                        address: cells[1]?.innerText?.trim(),
                        type: cells[2]?.innerText?.trim(),
                        program: cells[3]?.innerText?.trim(),
                        list: cells[4]?.innerText?.trim(),
                        score: cells[5]?.innerText?.trim(),
                        web: 'OFAC'
                    } : null;
                }).filter(row => row !== null);
            }";
        }
    }
}
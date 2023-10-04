using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using SteamBigData.Data;

namespace SteamBigData
{
	public class ParserService : BackgroundService
	{
        private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
        private readonly ILogger<ParserService> _logger;

        private readonly IServiceProvider _serviceProvider;

        public int itemNameId = 176288467;
        private string url = "https://steamcommunity.com/market/itemordersactivity?country=UA&language=english&currency=1&item_nameid=";
        HttpClient client = new HttpClient();

      

        private class JsonInfo
        {
            public string[] activity { get; set; }

            public int timestamp { get; set; }
        }

        //private class SoldInfo
        //{
        //    public int id { get; set; }

        //    public int itemNameId { get; set; }

        //    public string? buyerUserName { get; set; }

        //    public string? buyerAvatarUrl { get; set; }

        //    public string? sellerUserName { get; set; }

        //    public string? sellerAvatarUrl { get; set; }

        //    public decimal price { get; set; }

        //    public int timestamp { get; set; }
        //}

        public ParserService(
            ILogger<ParserService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _serviceProvider.CreateScope();
                var dc = scope.ServiceProvider.GetRequiredService<SteamBigDataDbContext>();
                try
                {
                    string response = await client.GetStringAsync(url + itemNameId);
                    JsonInfo jsonInfo = JsonSerializer.Deserialize<JsonInfo>(response);

                    foreach (var item in jsonInfo.activity)
                    {
                        if (item.Contains("purchased this item from"))
                        {
                            SoldInfo soldInfo = new();

                            soldInfo.itemNameId = itemNameId;
                            soldInfo.buyerAvatarUrl = item.Substring(item.IndexOf("http"), 4 + item.IndexOf(".jpg") - item.IndexOf("http"));
                            soldInfo.sellerAvatarUrl = item.Substring(item.LastIndexOf("http"), 4 + item.LastIndexOf(".jpg") - item.LastIndexOf("http"));
                            soldInfo.buyerUserName = item.Substring(20 + item.IndexOf("market_ticker_name"), item.IndexOf("purchased") - item.IndexOf("market_ticker_name") - 28);
                            soldInfo.sellerUserName = item.Substring(20 + item.LastIndexOf("market_ticker_name"), item.IndexOf("</span> for") - item.LastIndexOf("market_ticker_name") - 20);
                            soldInfo.price = decimal.Parse(item.Substring(13 + item.IndexOf("</span> for"), item.LastIndexOf("</span>") - item.IndexOf("</span> for") - 13));
                            soldInfo.timestamp = jsonInfo.timestamp;
                            dc.Add(soldInfo);
                        }
                    }
                    await dc.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(
                        $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
                }
            }
        }
    }
}


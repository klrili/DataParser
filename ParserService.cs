using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using SteamBigData.Data;
using System.Diagnostics.CodeAnalysis;

namespace SteamBigData
{
	public class ParserService : BackgroundService
	{
        private readonly TimeSpan _period = TimeSpan.FromSeconds(1);
        private readonly ILogger<ParserService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;


        int[] itemNameIdArray = new int[] { 176288467, 176185874 }; 
        private string url = "https://steamcommunity.com/market/itemordersactivity?country=UA&language=english&currency=1&item_nameid=";
        HttpClient client = new HttpClient();


        private class JsonInfo
        {
            public string[] activity { get; set; }

            public int timestamp { get; set; }
        }

        public ParserService(
            ILogger<ParserService> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);

            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                foreach (var id in itemNameIdArray)
                {
                    GetSaveData(id);
                }
            }
        }

        async void GetSaveData(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var dc = scope.ServiceProvider.GetRequiredService<SteamBigDataDbContext>();
            var a = _configuration.GetConnectionString("ConnectionStrings:DefaultConnection");
            Console.WriteLine(a);
            try
            {
                string response = await client.GetStringAsync(url + id);
                JsonInfo jsonInfo = JsonSerializer.Deserialize<JsonInfo>(response);

                foreach (var item in jsonInfo.activity)
                {
                    if (item.Contains("purchased this item from"))
                    {
                        SoldInfo soldInfo = new();

                        soldInfo.itemNameId = id;
                        soldInfo.buyerAvatarUrl = item.Substring(item.IndexOf("http"), 4 + item.IndexOf(".jpg") - item.IndexOf("http"));
                        soldInfo.sellerAvatarUrl = item.Substring(item.LastIndexOf("http"), 4 + item.LastIndexOf(".jpg") - item.LastIndexOf("http"));
                        soldInfo.buyerUserName = item.Substring(20 + item.IndexOf("market_ticker_name"), item.IndexOf("purchased") - item.IndexOf("market_ticker_name") - 28);
                        soldInfo.sellerUserName = item.Substring(20 + item.LastIndexOf("market_ticker_name"), item.IndexOf("</span> for") - item.LastIndexOf("market_ticker_name") - 20);
                        soldInfo.price = decimal.Parse(item.Substring(13 + item.IndexOf("</span> for"), item.LastIndexOf("</span>") - item.IndexOf("</span> for") - 13));
                        soldInfo.timestamp = jsonInfo.timestamp;

                        if (!dc.SoldInfos.Any(x =>
                            x.itemNameId == soldInfo.itemNameId &&
                            x.price == soldInfo.price &&
                            x.buyerUserName == soldInfo.buyerUserName &&
                            x.sellerUserName == soldInfo.sellerUserName &&
                            x.timestamp == soldInfo.timestamp))
                        {
                            dc.Add(soldInfo);
                        }
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


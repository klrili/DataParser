using System;
using System.Text.Json;
using SteamBigData.Data;

namespace SteamBigData
{
	public class Parser
	{
        private readonly SteamBigDataDbContext dbContext;

        public Parser(SteamBigDataDbContext context)
        {
            this.dbContext = context;
        }

        private class JsonInfo
        {
            public string[] activity { get; set; }

            public int timestamp { get; set; }
        }

        private class SoldInfo
        {
            public int itemNameId { get; set; }

            public string? buyerUserName { get; set; }

            public string? buyerAvatarUrl { get; set; }

            public string? sellerUserName { get; set; }

            public string? sellerAvatarUrl { get; set; }

            public decimal price { get; set; }

            public int timestamp { get; set; }
        }

        public int itemNameId = 176288467;

        private string url = "https://steamcommunity.com/market/itemordersactivity?country=UA&language=english&currency=1&item_nameid=";

        HttpClient client = new HttpClient();


        public void startPars(bool parseStatement = true)
        {
            while (parseStatement)
            {
                ItemParser(url, itemNameId);
                Thread.Sleep(2000);
            }
        }

        async private void ItemParser(string url, int itemNameId)
        {
            List<SoldInfo> soldInfoList = new();

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
                        dbContext.Add(soldInfo);
                        if (!soldInfoList.Contains(soldInfo)) soldInfoList.Add(soldInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            await dbContext.SaveChangesAsync();
            //add soldinfo to database
        }

    }
}


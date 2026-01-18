using Flurl.Http;
using MoneyMagic.GraphQL.Binance;
using MoneyMagic.GraphQL.RequestInputs;

namespace MoneyMagic.GraphQL.Queries
{
    [QueryType]
    public class OrderBookQueries
    {
        public async Task<OrderBook> GetOrderBooks(OrderBookInputs orderBookInputs,
            [Service]ILogger<OrderBookQueries> logger)
        {
            try
            {
                var binanceOrderBookUrl = $"https://api.binance.com/api/v3/depth?symbol={orderBookInputs.Symbol}&limit={orderBookInputs.Size}";
                var orders = await binanceOrderBookUrl.GetJsonAsync<OrderBook>();
                return orders;
            }
            catch(Exception ex)
            {
                logger.LogError("Error fetching order book from Binance API with message {Message}",ex.Message);
                throw;
            }
           
        }
    }
}

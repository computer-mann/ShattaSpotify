using Flurl.Http;
using MoneyMagic.GraphQL.Binance;
using MoneyMagic.GraphQL.RequestInputs;
using MoneyMagic.GraphQL.ViewModels;

namespace MoneyMagic.GraphQL.Queries
{
    [QueryType]
    public class OrderBookQueries
    {
        
        public async Task<DetailedOrderBook> GetOrderBooks(OrderBookInputs orderBookInputs,
            [Service]ILogger<OrderBookQueries> logger)
        {

            return new DetailedOrderBook(await GetBinanceOrderBook(orderBookInputs, logger));
        }

        private async Task<BinanceOrderBook> GetBinanceOrderBook(OrderBookInputs orderBookInputs,ILogger<OrderBookQueries> logger)
        {
            try
            {
                var binanceOrderBookUrl = $"https://api.binance.com/api/v3/depth?symbol={orderBookInputs.Symbol}&limit={orderBookInputs.Size}";
                var orders = await binanceOrderBookUrl.GetJsonAsync<BinanceOrderBook>();
                return orders;
            }
            catch (Exception ex)
            {
                logger.LogError("Error fetching order book from Binance API with message {Message}", ex.Message);
                throw;
            }
        }
    }
}

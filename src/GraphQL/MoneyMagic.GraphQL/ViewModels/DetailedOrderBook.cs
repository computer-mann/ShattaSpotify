using MoneyMagic.GraphQL.Binance;
using System.Text.Json.Serialization;

namespace MoneyMagic.GraphQL.ViewModels
{
    public class DetailedOrderBook
    {
        public readonly long LastUpdateId;
        public List<TradeOrder> Bids { get; } = [];
        public  List<TradeOrder> Asks { get; } = new List<TradeOrder>();

        public int BidsSize => Bids.Count;
        public int AsksSize => Asks.Count;

        public DetailedOrderBook(BinanceOrderBook binanceOrderBook)
        {
            LastUpdateId = binanceOrderBook.LastUpdateId;
            Bids = binanceOrderBook.Bids.Select(orders=> new TradeOrder(orders[0], orders[1])).ToList();
            Asks = binanceOrderBook.Asks.Select(orders => new TradeOrder(orders[0], orders[1])).Where(s => decimal.Parse(s.price) < 100)
                .ToList();
            ;
        }
    }

    public record TradeOrder(string price,string units);
}

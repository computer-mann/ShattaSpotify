namespace MoneyMagic.GraphQL.RequestInputs
{
    public class OrderBookInputs
    {
        public int Size { get; set; }
        public string Symbol { get; set; } = "DASHUSDT";
        public decimal ReasonableBuyPrice { get; set; } = 0;
        public decimal ReasonableSellPrice { get; set; } = decimal.MaxValue;
    }
}

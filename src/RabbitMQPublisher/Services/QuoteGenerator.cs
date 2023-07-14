namespace RabbitMQPublisher.Services;

public class QuoteGenerator
{
    private static readonly Random Random = new Random();
    public event EventHandler<NewMarketDataEventArgs> NewMarketDataEvent;
    private double lastAsk;

    public QuoteGenerator()
    {
        lastAsk = Random.Next(1, 1000);
    }
    public async Task GenerateQuotes(int numberOfTicks, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var marketData = GenerateQuotes(numberOfTicks);
            await Task.Delay(1000); // Delay for one second.
        }
    }

    private async Task<(double Ask, double Bid)> GenerateQuotes(int numberOfTicks)
    {
        double ask = this.lastAsk; // start from the last ask price
        double bid = 0;
        for (int i = 0; i < numberOfTicks; i++)
        {
            double spread = Random.NextDouble() * 10;
            bid = ask + spread;

            int increment = Random.Next(51);
            bool shouldDecrement = Random.Next(2) == 0;
            if (shouldDecrement && ask - increment >= 1)
            {
                ask -= increment;
            }
            else
            {
                ask += increment;
            }
            NewMarketDataEvent?.Invoke(this, new NewMarketDataEventArgs()
            {
                TickInfo = new MarketDataTickInfo()
                {
                    SymbolKey = $"TPS{numberOfTicks}",
                    BestPriceAsk = ask,
                    BestPriceBid = bid
                }
            });
            this.lastAsk = ask;
            await Task.Delay(1); // save the new ask price for next time
        }
        return (ask, bid);
    }
}
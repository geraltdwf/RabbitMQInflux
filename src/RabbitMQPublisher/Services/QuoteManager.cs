namespace RabbitMQPublisher.Services;

public class QuoteManager
{
    private Dictionary<int, (QuoteGenerator Generator, CancellationTokenSource CancellationToken)> _tickGenerators = new();

    public void Subscribe(int ticksPerSecond, EventHandler<NewMarketDataEventArgs> handler)
    {
        if (_tickGenerators.TryGetValue(ticksPerSecond, out var existing))
        {
            existing.Generator.NewMarketDataEvent += handler;
            return;
        }

        var generator = new QuoteGenerator();
        generator.NewMarketDataEvent += handler;
        var cts = new CancellationTokenSource();

        _tickGenerators[ticksPerSecond] = (generator, cts);
        generator.GenerateQuotes(ticksPerSecond, cts.Token);
    }

    public void Unsubscribe(int ticksPerSecond, EventHandler<NewMarketDataEventArgs> handler)
    {
        if (_tickGenerators.TryGetValue(ticksPerSecond, out var existing))
        {
            existing.Generator.NewMarketDataEvent -= handler;
        }
    }

    public void Stop(int ticksPerSecond)
    {
        if (_tickGenerators.TryGetValue(ticksPerSecond, out var existing))
        {
            existing.CancellationToken.Cancel();
            _tickGenerators.Remove(ticksPerSecond);
        }
    }
}
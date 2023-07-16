namespace RabbitMQPublisher;

public class TickerConnector
{
    private ConnectionFactory _factory;
    private QuoteManager _quoteManager;
    private IConnection _connection;
    private IModel _channel;
    private int _ticks;
    public bool isOpen => _channel.IsOpen;

    public TickerConnector(string hostname, int ticks)
    {
        _factory = new ConnectionFactory { HostName = hostname };
        _ticks = ticks;
        _quoteManager = new QuoteManager();
    }


    public void Start()
    {
        // _connection = _factory.CreateConnection();
        // _channel = _connection.CreateModel();
        // _channel.QueueDeclare(queue: "hello",
        //     durable: false,
        //     exclusive: false,
        //     autoDelete: false,
        //     arguments: null);
        // _quoteManager.Subscribe(_ticks, PublishMsg);
        
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare an exchange
        _channel.ExchangeDeclare(exchange: "my_exchange", type: ExchangeType.Direct);

        _quoteManager.Subscribe(_ticks, PublishMsg);
        
    }

    public void Stop()
    {
        _channel.Close();
        _channel.Dispose();
        _connection.Dispose();
    }

    private void PublishMsg(object sender,  NewMarketDataEventArgs e)
    {
        // string message = $"Symbol:{e.TickInfo.SymbolKey} Ask:{e.TickInfo.BestPriceAsk} Bid:{e.TickInfo.BestPriceBid}";
        // var body = Encoding.UTF8.GetBytes(message);
        // _channel.BasicPublish(exchange: string.Empty,
        //     routingKey: "hello",
        //     basicProperties: null,
        //     body: body);
        // Console.WriteLine($" [x] Sent {message}");
        
        string message = $"Symbol:{e.TickInfo.SymbolKey} Ask:{e.TickInfo.BestPriceAsk} Bid:{e.TickInfo.BestPriceBid}";
        var body = Encoding.UTF8.GetBytes(message);

        // Publish the message to the exchange instead of a specific queue
        _channel.BasicPublish(exchange: "my_exchange", routingKey: "", basicProperties: null, body: body);

        Console.WriteLine($" [x] Sent {message}");
    }

}

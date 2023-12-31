namespace RabbitMQReceiver;

public class RabbitMQConsumer
{
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    private EventingBasicConsumer _consumer;
    private InfluxDbPublisherService _influxDbPublisherService;
    public bool isOpen => _channel.IsOpen;

    public RabbitMQConsumer(string hostname, InfluxDbPublisherService influxDbPublisherService)
    {
        _factory = new ConnectionFactory { HostName = hostname };
        _influxDbPublisherService = influxDbPublisherService;
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
        // _consumer = new EventingBasicConsumer(_channel);
        // _consumer.Received += ConsumerOnReceived;
        // _channel.BasicConsume(queue: "hello",
        //     autoAck: true,
        //     consumer: _consumer);
            
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        // Declare an exchange
        _channel.ExchangeDeclare(exchange: "my_exchange", type: ExchangeType.Direct);
        
        // Create a new queue and bind it to the exchange
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: "my_exchange", routingKey: "");
        
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += ConsumerOnReceived;
        
        // Start consuming messages from the queue
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: _consumer);
    }

    public void Stop()
    {
        _consumer.Received -= ConsumerOnReceived;
        _channel.Close();
        _channel.Dispose();
        _connection.Dispose();
    }

    private void ConsumerOnReceived(object? sender, BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [x] Received {message}");
    }
}
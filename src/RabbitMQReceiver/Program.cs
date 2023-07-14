InfluxDbPublisherService service = new InfluxDbPublisherService();
RabbitMQConsumer consumer = new RabbitMQConsumer("rabbitmq", service);
consumer.Start();

while(true)
    Thread.Sleep(50);
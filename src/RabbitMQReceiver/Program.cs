InfluxDbPublisherService service = new InfluxDbPublisherService();
bool isConnected = false;
while (!isConnected)
{
    try
    {
        RabbitMQConsumer consumer = new RabbitMQConsumer("rabbitmq", service);
        consumer.Start();

        if (consumer.isOpen)
            isConnected = true;
    }
    catch (Exception e)
    {
        
    }
}

while(true)
    Thread.Sleep(50);
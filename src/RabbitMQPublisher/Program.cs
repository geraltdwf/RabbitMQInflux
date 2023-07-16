bool isConnected = false;
TickerConnector connector;
while (!isConnected)
{
    Thread.Sleep(1000);
    try
    {
        connector = new TickerConnector("rabbitmq", 10);
        connector.Start();
        if (connector.isOpen)
            isConnected = true;
    }
    catch (Exception e)
    {
        
    }
   
}
while (true)
    Thread.Sleep(100);

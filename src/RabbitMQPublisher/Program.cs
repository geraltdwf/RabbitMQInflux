
TickerConnector connector = new TickerConnector("rabbitmq", 10);
connector.Start();

while (true)
    Thread.Sleep(100);

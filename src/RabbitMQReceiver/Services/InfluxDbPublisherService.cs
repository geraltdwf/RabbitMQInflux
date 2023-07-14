namespace RabbitMQReceiver.Services;

public class InfluxDbPublisherService
{
    public async Task Publish(string messsage)
    {
        try
        {
            var token = "test1";
            using var client = new InfluxDBClient("www", token);

            var writeApiAsync = client.GetWriteApiAsync();

           
            // await writeApiAsync.WritePointsAsync(points,  "", "");
        }
        catch (System.Exception)
        {
                
            Console.WriteLine("Failed to publish influxDB");
        }
           
    }
}
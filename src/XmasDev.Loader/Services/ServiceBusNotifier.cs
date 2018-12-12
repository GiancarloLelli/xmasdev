using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using XmasDev.Loader.Common;

namespace XmasDev.Loader.Services
{
    public class ServiceBusNotifier
    {
        public static async Task Notify()
        {
            var client = new QueueClient(ConfigurationHelper.ServiceBusConnection, "xmasdev");
            var data = new BusPayload { JobId = Guid.NewGuid() };
            var dataText = JsonConvert.SerializeObject(data);
            await client.SendAsync(new Message(Encoding.UTF8.GetBytes(dataText)));
        }
    }

    public class BusPayload
    {
        public Guid JobId { get; set; }
    }
}

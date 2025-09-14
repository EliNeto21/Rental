using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IMessaging
{
    public interface IRabbitMqProducer
    {
        Task PublishAsync<T>(string queue, T message);
    }
}

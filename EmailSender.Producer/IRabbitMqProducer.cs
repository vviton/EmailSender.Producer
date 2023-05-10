using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Producer
{
    public interface IRabbitMqProducer<T>
    {
        public void SendToQueue(T message);
    }
}

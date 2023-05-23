using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static API.Infraestructure.MessagingQeue;

namespace API.Infraestructure
{
    public interface IMessagingQeue
    {
        void SendDefaultQueue(string queueName, string message, Boolean persistent = false);
        Task<string?> ReceiveDefaultQueue(string queueName, HandleMessage handleMessage);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.DTO;
using API.Infraestructure;
using Bogus;
using Moq;
using Newtonsoft.Json;
using static API.Infraestructure.MessagingQeue;

namespace TEST.Infraestructure
{

    public interface MessagingQeueMockWrapper
    {
        void ConfigureMockId(Guid id);
    }
    public class MessagingQeueMock : IMessagingQeue
    {
        private readonly Mock<IMessagingQeue> _mock;
        private Faker faker = new Faker();
        private Guid Id;
        public MessagingQeueMock(Guid? id)
        {
            _mock = new Mock<IMessagingQeue>();
            Id = Guid.NewGuid();
            if (id != null)
            {
                Id = (Guid)id;
            }
            ConfigureMock();
        }

        public IMessagingQeue Object => _mock.Object;

        private void ConfigureMock()
        {

            _mock.Setup(m => m.SendDefaultQueue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Verifiable();
            _mock.Setup(m => m.ReceiveDefaultQueue(It.IsAny<string>(), It.IsAny<HandleMessage>()))
                .Returns((string queueName, HandleMessage handleMessage) =>
                {
                    return Task.Run(async () =>
                    {
                        PedidoMessagingDTO dto = new PedidoMessagingDTO() { Id = Id, idCliente = Guid.NewGuid(), ValorTotal = faker.Random.Decimal(1, 10) };
                        string dtoJson = JsonConvert.SerializeObject(dto);
                        await handleMessage.Invoke(dtoJson, null, null, null);
                        return (string?)null;
                    });
                });
        }

        public void SendDefaultQueue(string queueName, string message, bool persistent = false)
        {
            _mock.Object.SendDefaultQueue(queueName, message, persistent);
        }
        public Task<string?> ReceiveDefaultQueue(string queueName, HandleMessage handleMessage)
        {
            return _mock.Object.ReceiveDefaultQueue(queueName, handleMessage);
        }
    }
}

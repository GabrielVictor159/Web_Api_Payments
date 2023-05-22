using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Domain.DTO;
using API.Infraestructure;
using API.Repository;
using Newtonsoft.Json;

namespace API.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMessagingQeue _messagingQeue;
        public PaymentService(IPaymentRepository paymentRepository, IMessagingQeue messagingQeue)
        {
            _paymentRepository = paymentRepository;
            _messagingQeue = messagingQeue;
        }

        public async Task<Object> AddPaymentAsync(PaymentDTO paymentDTO)
        {
            var paymentDTOValidation = new PaymentDTOValidation().Validate(paymentDTO);
            if (!paymentDTOValidation.IsValid)
            {
                return paymentDTOValidation.ToString();
            }
            List<string> mensagens = new();
            Payment payment;
            Payment? paymentCadastro = null;
            await _messagingQeue.ReceiveFanoutExchange("Pedido", "PayamentPedidoQeue", async (message, channel, model, args) =>
            {
                PedidoMessagingDTO pedido = JsonConvert.DeserializeObject<PedidoMessagingDTO>(message) ?? new PedidoMessagingDTO();

                if (pedido.Id == paymentDTO.IdPedido)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    payment = new Payment(pedido.ValorTotal, pedido.Id, paymentDTO.Method);
                    paymentCadastro = await _paymentRepository.AddPaymentAsync(payment);

                    if (paymentCadastro != null)
                    {
                        if (channel != null && args != null)
                        {
                            channel.BasicAck(args.DeliveryTag, multiple: false);
                        }
                    }
                    else
                    {
                        if (channel != null && args != null)
                        {
                            channel.BasicReject(args.DeliveryTag, requeue: true);
                        }
                    }
                }
                else
                {
                    if (channel != null && args != null)
                    {
                        channel.BasicReject(args.DeliveryTag, requeue: true);
                    }
                }
            });

            if (paymentCadastro == null)
            {
                return "Não foi encontrado nenhum pedido com o Id fornecido.";
            }
            else
            {
                return paymentCadastro;
            }
        }

    }

}
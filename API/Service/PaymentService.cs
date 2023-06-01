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
            await _messagingQeue.ReceiveDefaultQueue("Pedido", async (message, channel, model, args) =>
            {
                PedidoMessagingDTO pedido = JsonConvert.DeserializeObject<PedidoMessagingDTO>(message) ?? new PedidoMessagingDTO();
                if (pedido.Id == paymentDTO.IdPedido)
                {
                    payment = new Payment(pedido.ValorTotal, pedido.Id, paymentDTO.Method);
                    paymentCadastro = await _paymentRepository.AddPaymentAsync(payment);

                    if (paymentCadastro != null)
                    {
                        if (channel != null && args != null)
                        {
                            try
                            {
                                channel.BasicAck(args.DeliveryTag, multiple: false);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error: {e}");
                            }
                        }
                    }
                    else
                    {
                        if (channel != null && args != null)
                        {
                            try
                            {
                                channel.BasicReject(args.DeliveryTag, requeue: true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error: {e}");
                            }
                        }
                    }
                }
                else
                {
                    if (channel != null && args != null)
                    {
                        try
                        {
                            channel.BasicReject(args.DeliveryTag, requeue: true);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error: {e}");
                        }
                    }
                }
            });

            if (paymentCadastro == null)
            {
                return "Não foi encontrado nenhum pedido com o Id fornecido.";
            }
            else
            {
                PaymentMessageDTO dtoMessage = new PaymentMessageDTO() { IdPedido = paymentCadastro.IdPedido, IdPayment = paymentCadastro.Id };
                var json = JsonConvert.SerializeObject(dtoMessage);
                _messagingQeue.SendDefaultQueue("Pagamentos", json, true);
                return paymentCadastro;
            }
        }

        public async Task<Object> GetOneAsync(Guid id)
        {
            var result = await _paymentRepository.GetPaymentByIdAsync(id);
            if (result == null)
            {
                return "Nenhum Pagamento com esse id foi encontrado.";
            }
            return result;
        }

    }

}
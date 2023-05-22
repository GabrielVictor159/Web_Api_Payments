using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain.DTO
{
    public class PedidoMessagingDTO
    {
        public Guid Id { get; set; }
        public Guid idCliente { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain.DTO
{
    public class PaymentDTO : CreditCardDTO
    {
        public Guid IdPedido { get; set; }
        public String Method { get; set; } = "";
    }
}
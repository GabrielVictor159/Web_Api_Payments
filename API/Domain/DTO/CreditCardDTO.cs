using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain.DTO
{
    public class CreditCardDTO
    {
        public String Number { get; set; } = "";
        public String NomeTitular { get; set; } = "";
        public String DataValidade { get; set; } = "";
        public int CVV { get; set; } = 0;
    }
}
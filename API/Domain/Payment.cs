using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace API.Domain
{
    public class Payment
    {
        public Payment(decimal valor, Guid idPedido, String method)
        {
            this.Valor = valor;
            this.IdPedido = idPedido;
            this.Method = method;
        }
        public Payment()
        { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public Guid IdPedido { get; set; }
        [Required]
        public String Method { get; set; } = "";
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Enum;
using FluentValidation;
namespace API.Domain
{
    public class PaymentValidation : AbstractValidator<Payment>
    {
        public PaymentValidation()
        {
            RuleFor(payment => payment.Valor)
            .NotNull()
            .NotEmpty()
            .WithMessage("A propriedade Valor não pode ser nula ou vazia.")
            .GreaterThan(0)
            .WithMessage("A propriedade Valor deve ser maior que 0.");

            RuleFor(payment => payment.IdPedido)
            .NotNull()
            .NotEmpty()
            .WithMessage("A propriedade IdPedido não pode ser nula ou vazia.");

            RuleFor(payment => payment.Method)
            .NotNull()
            .NotEmpty()
            .WithMessage("A propriedade Method não pode ser nula ou vazia.")
            .Must(method => System.Enum.TryParse(typeof(PaymentMethods), method, out _))
            .WithMessage("O tipo de pagamento não esta listado no sistema.");

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Enum;
using FluentValidation;

namespace API.Domain.DTO
{
    public class PaymentDTOValidation : AbstractValidator<PaymentDTO>
    {
        public PaymentDTOValidation()
        {
            Include(new CreditCardDTOValidation());
            RuleFor(dto => dto.IdPedido)
            .NotEmpty()
            .NotNull()
            .WithMessage("o IdPedido não pode ser nulo ou vazio.");

            RuleFor(payment => payment.Method)
           .NotNull()
           .NotEmpty()
           .WithMessage("A propriedade Method não pode ser nula ou vazia.")
           .Must(method => System.Enum.TryParse(typeof(PaymentMethods), method, out _))
           .WithMessage("O tipo de pagamento não esta listado no sistema.");
        }

    }
}
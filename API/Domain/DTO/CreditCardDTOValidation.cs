using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace API.Domain.DTO
{
    public class CreditCardDTOValidation : AbstractValidator<CreditCardDTO>
    {
        public CreditCardDTOValidation()
        {
            RuleFor(dto => dto.CVV)
            .NotNull()
            .NotEmpty()
            .WithMessage("A variável CVV não pode ser nula ou vazia.")
            .GreaterThan(0)
            .WithMessage("A variável CVV não pode ser 0.")
            .Must(x => x.ToString().Length == 3)
            .WithMessage("A variável CVV deve ter exatamente 3 dígitos.");

            RuleFor(dto => dto.NomeTitular)
            .NotNull()
            .NotEmpty()
            .WithMessage("A variável NommeTitular não poder ser nula ou vazia.");


        }
    }
}
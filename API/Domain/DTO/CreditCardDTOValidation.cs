using System;
using System.Collections.Generic;
using System.Globalization;
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
            .WithMessage("A variável CVV não pode ser 0.");

            RuleFor(dto => dto.NomeTitular)
            .NotNull()
            .NotEmpty()
            .WithMessage("A variável NomeTitular não poder ser nula ou vazia.");

            RuleFor(dto => dto.DataValidade)
            .NotNull()
            .NotEmpty()
            .WithMessage("A variável DataValidade não pode ser nula ou vazia.")
            .Matches(@"^(0[1-9]|1[0-2])\/[0-9]{2}$")
            .WithMessage("A variável DataValidade deve seguir o padrão MM/AA, onde MM é o mês (01 a 12) e AA é o ano (00 a 99).")
            .Must(dataValidade =>
            {
                if (DateTime.TryParseExact(dataValidade, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expirationDate))
                {
                    return expirationDate > DateTime.Now;
                }
                return false;
            })
            .WithMessage("A variável DataValidade deve ser uma data futura.");

            RuleFor(dto => dto.Number)
            .NotNull()
            .NotEmpty()
            .WithMessage("A variável Number não pode ser nula ou vazia.")
            .Matches(@"^[0-9]{14,17}$")
            .WithMessage("A variável Number deve conter apenas dígitos numéricos e ter 12 a 19 dígitos.");

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.DTO;
using API.Domain.Enum;
using Bogus;
using FluentAssertions;

namespace TEST.Domain.DTO
{
    public class PaymentDTOValidationTest
    {
        private Faker faker = new Faker("pt_BR");
        private PaymentDTOValidation dtoValidation = new PaymentDTOValidation();

        [Fact]
        public void IdPedidoTest()
        {
            PaymentDTO dto = new PaymentDTO()
            {
                NomeTitular = faker.Name.FullName(),
                Number = string.Join("", faker.Random.Digits(16)),
                DataValidade = faker.Date.Future().ToString("MM/yy"),
                CVV = int.Parse(faker.Finance.CreditCardCvv()),
                Method = PaymentMethods.CARTAOMASTERCARD.ToString()
            };
            var result1 = dtoValidation.Validate(dto);
            result1.IsValid.Should().BeFalse();
            dto.IdPedido = Guid.NewGuid();
            var result2 = dtoValidation.Validate(dto);
            result2.IsValid.Should().BeTrue();
        }
    }
}
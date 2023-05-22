using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.DTO;
using Bogus;
using Bogus.DataSets;
using FluentAssertions;

namespace TEST.Domain.DTO
{
    public class CreditCardDTOValidationTest
    {
        private Faker faker = new Faker("pt_BR");
        private CreditCardDTOValidation dtoValidation = new CreditCardDTOValidation();
        [Fact]
        public void CvvTest()
        {
            CreditCardDTO dto = new CreditCardDTO()
            {
                Number = string.Join("", faker.Random.Digits(16)),
                DataValidade = faker.Date.Future().ToString("MM/yy"),
                NomeTitular = faker.Name.FullName()
            };
            var result1 = dtoValidation.Validate(dto);
            result1.IsValid.Should().BeFalse();
            dto.CVV = int.Parse(faker.Finance.CreditCardCvv());
            var result2 = dtoValidation.Validate(dto);
            result2.IsValid.Should().BeTrue();
        }

        [Fact]
        public void NumberTest()
        {
            CreditCardDTO dto = new CreditCardDTO()
            {
                DataValidade = faker.Date.Future().ToString("MM/yy"),
                NomeTitular = faker.Name.FullName(),
                CVV = int.Parse(faker.Finance.CreditCardCvv())
            };
            var result1 = dtoValidation.Validate(dto);
            result1.IsValid.Should().BeFalse();
            dto.Number = "a";
            var result2 = dtoValidation.Validate(dto);
            result2.IsValid.Should().BeFalse();
            dto.Number = "6526512";
            var result3 = dtoValidation.Validate(dto);
            result3.IsValid.Should().BeFalse();
            dto.Number = string.Join("", faker.Random.Digits(16));
            var result4 = dtoValidation.Validate(dto);
            result4.IsValid.Should().BeTrue();
        }

        [Fact]
        public void DataValidadeTest()
        {
            CreditCardDTO dto = new CreditCardDTO()
            {
                Number = string.Join("", faker.Random.Digits(16)),
                NomeTitular = faker.Name.FullName(),
                CVV = int.Parse(faker.Finance.CreditCardCvv())
            };
            var result1 = dtoValidation.Validate(dto);
            result1.IsValid.Should().BeFalse();
            dto.DataValidade = "sadad";
            var result2 = dtoValidation.Validate(dto);
            result2.IsValid.Should().BeFalse();
            dto.DataValidade = faker.Date.Past().ToString("MM/yy");
            var result3 = dtoValidation.Validate(dto);
            result3.IsValid.Should().BeFalse();
            dto.DataValidade = faker.Date.Future().ToString("MM/yy");
            var result4 = dtoValidation.Validate(dto);
            result4.IsValid.Should().BeTrue();
        }

        [Fact]
        public void NomeTitularTest()
        {
            CreditCardDTO dto = new CreditCardDTO()
            {
                Number = string.Join("", faker.Random.Digits(16)),
                DataValidade = faker.Date.Future().ToString("MM/yy"),
                CVV = int.Parse(faker.Finance.CreditCardCvv())
            };
            var result1 = dtoValidation.Validate(dto);
            result1.IsValid.Should().BeFalse();
            dto.NomeTitular = faker.Name.FullName();
            var result2 = dtoValidation.Validate(dto);
            result2.IsValid.Should().BeTrue();
        }
    }
}
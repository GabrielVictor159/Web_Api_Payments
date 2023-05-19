using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Domain.Enum;
using Bogus;
using FluentAssertions;

namespace TEST.Domain
{
    public class PaymentValidationTest
    {
        private Faker faker = new Faker();
        [Fact]
        public void ValorTest()
        {
            Payment payment = new Payment();
            payment.IdPedido = Guid.NewGuid();
            payment.Method = PaymentMethods.CARTAOMASTERCARD.ToString();
            var validation = new PaymentValidation().Validate(payment);
            validation.IsValid.Should().BeFalse();
            payment.Valor = 0;
            var validation2 = new PaymentValidation().Validate(payment);
            validation2.IsValid.Should().BeFalse();
            payment.Valor = faker.Random.Int(1, 1000);
            var validation3 = new PaymentValidation().Validate(payment);
            validation3.IsValid.Should().BeTrue();
        }

        [Fact]
        public void MethodTest()
        {
            Payment payment = new Payment();
            payment.IdPedido = Guid.NewGuid();
            payment.Valor = faker.Random.Int(1, 1000);
            var validation = new PaymentValidation().Validate(payment);
            validation.IsValid.Should().BeFalse();
            payment.Method = faker.Random.String2(10);
            var validation2 = new PaymentValidation().Validate(payment);
            validation2.IsValid.Should().BeFalse();
            payment.Method = PaymentMethods.CARTAOMASTERCARD.ToString();
            var validation3 = new PaymentValidation().Validate(payment);
            validation3.IsValid.Should().BeTrue();
        }

        [Fact]
        public void IdPedidoTest()
        {
            Payment payment = new Payment();
            payment.Valor = faker.Random.Int(1, 1000);
            payment.Method = PaymentMethods.CARTAOMASTERCARD.ToString();
            var validation = new PaymentValidation().Validate(payment);
            validation.IsValid.Should().BeFalse();
            payment.IdPedido = Guid.NewGuid();
            var validation2 = new PaymentValidation().Validate(payment);
            validation2.IsValid.Should().BeTrue();
        }
    }
}
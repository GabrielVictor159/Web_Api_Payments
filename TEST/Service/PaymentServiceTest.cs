using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Domain.DTO;
using API.Domain.Enum;
using API.Infraestructure;
using API.Repository;
using API.Service;
using Bogus;
using Bogus.DataSets;
using FluentAssertions;
using TEST.Infraestructure;
using Xunit.Frameworks.Autofac;

namespace TEST.Service
{
    [UseAutofacTestFramework]
    public class PaymentServiceTest
    {
        private Faker faker = new Faker();
        private readonly PaymentService _service;
        private readonly ApplicationDbContext _context;
        private Guid idPedido = Guid.NewGuid();
        public PaymentServiceTest(IPaymentRepository repository, ApplicationDbContext context)
        {
            _service = new PaymentService(repository, new MessagingQeueMock(idPedido));
            _context = context;
        }

        [Fact]
        public async void AddPaymentAsyncTest()
        {
            PaymentDTO dto = new PaymentDTO()
            {
                IdPedido = Guid.NewGuid(),
                Method = PaymentMethods.CARTAOMASTERCARD.ToString(),
                Number = string.Join("", faker.Random.Digits(16)),
                DataValidade = faker.Date.Future().ToString("MM/yy"),
                NomeTitular = faker.Name.FullName(),
                CVV = int.Parse(faker.Finance.CreditCardCvv())
            };
            var result1 = await _service.AddPaymentAsync(dto);
            result1.Should().BeOfType<string>();
            dto.IdPedido = idPedido;
            var result2 = await _service.AddPaymentAsync(dto);
            result2.Should().BeOfType<Payment>();
        }

        [Fact]
        public async void GetOneAsyncTest()
        {
            Payment payment = new Payment(faker.Random.Decimal(1, 20), Guid.NewGuid(), PaymentMethods.CARTAOMASTERCARD.ToString());
            await _context.payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            var result1 = await _service.GetOneAsync(Guid.NewGuid());
            result1.Should().BeOfType<string>();
            var result2 = await _service.GetOneAsync(payment.Id);
            result2.Should().BeOfType<Payment>();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Domain.Enum;
using API.Infraestructure;
using API.Repository;
using Bogus;
using FluentAssertions;
using Xunit.Frameworks.Autofac;

namespace TEST.Repository
{
    [UseAutofacTestFramework]
    public class PaymentRepositoryTest
    {
        private Faker faker = new Faker();
        private readonly IPaymentRepository _repository;
        private readonly ApplicationDbContext _context;
        public PaymentRepositoryTest(IPaymentRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [Fact]
        public async void AddPaymentAsyncTest()
        {
            Payment payment = new Payment(faker.Random.Decimal(1, 20), Guid.NewGuid(), PaymentMethods.CARTAOMASTERCARD.ToString());
            var paymentRegister = await _repository.AddPaymentAsync(payment);
            paymentRegister.Should().BeOfType<Payment>();
            var paymentRegister2 = await _repository.AddPaymentAsync(payment);
            paymentRegister2.Should().BeNull();
        }

        [Fact]
        public async void GetPaymentByIdAsyncTest()
        {
            Payment payment = new Payment(faker.Random.Decimal(1, 20), Guid.NewGuid(), PaymentMethods.CARTAOMASTERCARD.ToString());
            await _context.payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            var result1 = await _repository.GetPaymentByIdAsync(Guid.NewGuid());
            result1.Should().BeNull();
            var result2 = await _repository.GetPaymentByIdAsync(payment.Id);
            result2.Should().BeOfType<Payment>();
        }
    }
}
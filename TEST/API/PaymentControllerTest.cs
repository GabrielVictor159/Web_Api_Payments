using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Domain;
using API.Domain.DTO;
using API.Domain.Enum;
using API.Infraestructure;
using API.Repository;
using API.Service;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TEST.Infraestructure;
using Xunit.Frameworks.Autofac;

namespace TEST.API
{
    [UseAutofacTestFramework]
    public class PaymentControllerTest
    {
        private Faker faker = new Faker();
        private PaymentController _controller;
        private Guid idPedido = Guid.NewGuid();
        private readonly ApplicationDbContext _context;
        public PaymentControllerTest(IPaymentRepository iPaymentRepository, ApplicationDbContext context)
        {
            IPaymentService paymentService = new PaymentService(iPaymentRepository, new MessagingQeueMock(idPedido));
            _controller = new PaymentController(paymentService);
            _context = context;
        }

        [Fact]
        public async void PaymentTest()
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
            var result1 = await _controller.Payment(dto);
            result1.Result.Should().BeOfType<BadRequestObjectResult>();
            dto.IdPedido = idPedido;
            var result2 = await _controller.Payment(dto);
            result2.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetOne()
        {
            Payment payment = new Payment(faker.Random.Decimal(1, 20), Guid.NewGuid(), PaymentMethods.CARTAOMASTERCARD.ToString());
            await _context.payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            var result1 = await _controller.GetOne(Guid.NewGuid());
            result1.Result.Should().BeOfType<BadRequestObjectResult>();
            var result2 = await _controller.GetOne(payment.Id);
            result2.Result.Should().BeOfType<OkObjectResult>();
        }

    }
}
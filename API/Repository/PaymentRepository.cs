using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Infraestructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace API.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PaymentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Payment?> AddPaymentAsync(Payment payment)
        {
            if (payment.Id != null)
            {
                var pedidoConsulta = await GetPaymentByIdAsync(payment.Id);
                if (pedidoConsulta != null)
                {
                    return null;
                }
            }
            await dbContext.payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetPaymentByIdAsync(Guid id)
        {
            return await dbContext.payments.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
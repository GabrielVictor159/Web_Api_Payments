using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;

namespace API.Repository
{
    public interface IPaymentRepository
    {
        Task<Payment?> AddPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByIdAsync(Guid id);

    }
}
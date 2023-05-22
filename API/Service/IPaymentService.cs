using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.DTO;

namespace API.Service
{
    public interface IPaymentService
    {
        Task<Object> AddPaymentAsync(PaymentDTO paymentDTO);
        Task<Object> GetOneAsync(Guid id);
    }
}
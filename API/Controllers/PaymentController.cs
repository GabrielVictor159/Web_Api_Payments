using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.DTO;
using API.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _iPaymentService;
        public PaymentController(IPaymentService iPaymentService)
        {
            _iPaymentService = iPaymentService;
        }
        [HttpPost]
        public async Task<ActionResult<String>> Payment(PaymentDTO dto)
        {
            try
            {
                Object result = await _iPaymentService.AddPaymentAsync(dto);
                if (result is string e)
                {
                    return BadRequest(e);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor: " + e.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Payments")]
    public class PaymentController : ControllerBase
    {
        public PaymentController()
        {

        }
        // [HttpPost]
        // [Route("VISA")]
        // public async Task<ActionResult<String>> PaymentVisa()
        // {
        //     try
        //     {

        //     }
        //     catch(Exception e)
        //     {
        //         return StatusCode(500, "Ocorreu um erro interno no servidor: " + e.Message); 
        //     }
        // }
    }
}
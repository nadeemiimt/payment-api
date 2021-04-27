using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApi.Controllers.Payment
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentControllerTemp : ControllerBase
    {
        // POST: PaymentController/Create
        [HttpPost]
        public Task<string> ProcessPaymentAsync(IFormCollection collection)
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}

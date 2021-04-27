using Microsoft.AspNetCore.Mvc;
using PaymentCommon.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PaymentCommon.Interfaces;
using PaymentCommon.Models.Response;

namespace PaymentApi.Controllers.Payment
{
    [Route("api/v1")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private readonly IPaymentBusiness _paymentBusiness;
        
        public PaymentController(ILoggerManager logger, IPaymentBusiness paymentBusiness)
        {
            this._logger = logger;
            this._paymentBusiness = paymentBusiness;
        }

        /// <summary>Process payment request.</summary>
        /// <param name="paymentRequestModel">Payment details required for processing.</param>
        /// <returns>Ok</returns>
        [HttpPost]
        [Route("payment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomErrorDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessPaymentAsync([FromBody]PaymentRequestModel paymentRequestModel)
        {
            try
            {
                var result = await this._paymentBusiness.ProcessPaymentAsync(paymentRequestModel);

                if (result == PaymentStatus.Processed)
                {
                    return new OkResult();
                }
                else
                {
                    var status = Enum.GetName(typeof(PaymentStatus), result);
                    throw new Exception($"Payment status is {status}");
                }
            }
            catch (Exception e)
            {
                this._logger.LogError(e);
                this.Response.StatusCode = 500;
                throw new Exception($"Payment failed. Error message: {e.Message}", e);
            }
        }
    }
}
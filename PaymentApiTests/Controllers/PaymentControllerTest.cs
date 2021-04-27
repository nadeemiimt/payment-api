using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PaymentApi.Controllers.Payment;
using PaymentCommon.Interfaces;
using PaymentCommon.Models;
using PaymentCommon.Models.Response;

namespace PaymentApiTests.Controllers
{
    /// <summary>
    /// Test class to test PaymentController.
    /// </summary>
    [TestClass]
    public class PaymentControllerTest
    {
        /// <summary>
        /// Logger ref.
        /// </summary>
        private ILoggerManager _logger;

        /// <summary>
        /// Payment business.
        /// </summary>
        private IPaymentBusiness _paymentBusiness;

        /// <summary>
        /// Payment controller.
        /// </summary>
        private readonly PaymentController _paymentController;

        public PaymentControllerTest()
        {
            _logger = Substitute.For<ILoggerManager>();
            _paymentBusiness = Substitute.For<IPaymentBusiness>();

            _paymentController = new PaymentController(_logger, _paymentBusiness);
        }

        /// <summary>
        /// Method to test valid request response.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PaymentController_ForValidRequest_200StatusIsReturnedAsync()
        {
            //Arrange
            this._paymentBusiness.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(Task.FromResult(PaymentStatus.Processed));

            //Act
            var result = await this._paymentController.ProcessPaymentAsync(new PaymentRequestModel());

            var actualStatus = (result as OkResult)?.StatusCode;
            //Assert

            Assert.AreEqual(200, actualStatus);
            await this._paymentBusiness.Received(1).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            this._logger.Received(0).LogError(Arg.Any<Exception>());
        }

        /// <summary>
        /// Method to test error.
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task PaymentController_ForUnsuccessfulSaveRequest_500StatusIsReturnedAsync()
        {
            //Arrange
            this._paymentBusiness.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(Task.FromResult(PaymentStatus.Failed));

            //Act
            var result = await this._paymentController.ProcessPaymentAsync(new PaymentRequestModel());

            var actualStatus = (result as StatusCodeResult)?.StatusCode;
            //Assert

            Assert.AreEqual(500, actualStatus);
            await this._paymentBusiness.Received(1).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            this._logger.Received(0).LogError(Arg.Any<Exception>());
        }

        /// <summary>
        /// Method to test error if exception occurs.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task PaymentController_ForProcessPaymentException_500StatusIsReturnedAsync()
        {
            //Arrange
            this._paymentBusiness.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Throws(x => throw new Exception());

            //Act
            var result = await this._paymentController.ProcessPaymentAsync(new PaymentRequestModel());

            var actualStatus = (result as StatusCodeResult)?.StatusCode;
            //Assert

            Assert.AreEqual(500, actualStatus);
            await this._paymentBusiness.Received(1).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            this._logger.Received(1).LogError(Arg.Any<Exception>());
        }


    }
}

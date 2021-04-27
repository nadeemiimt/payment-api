using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PaymentBusiness.DI;
using PaymentCommon.Interfaces;
using PaymentCommon.Models;
using PaymentCommon.Models.Response;
using PaymentCommon.Resources;
using PaymentEntities.Interfaces;

namespace PaymentBusinessTests.Payment
{
    [TestClass]
    public class PaymentBusinessTest
    {
        private readonly ILoggerManager _logger;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly ICheapPaymentGateway _cheapPaymentGateway;
        private readonly IExpensivePaymentGateway _expensivePaymentGateway;
        private readonly IPremiumPaymentGateway _premiumPaymentGateway;

        private readonly Register.ServiceResolver _serviceAccessor;

        private readonly PaymentBusiness.Payment.PaymentBusiness _paymentBusiness;

        public PaymentBusinessTest()
        {
            _logger = Substitute.For<ILoggerManager>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();

            _cheapPaymentGateway = Substitute.For<ICheapPaymentGateway>();
            _expensivePaymentGateway = Substitute.For<IExpensivePaymentGateway>();
            _premiumPaymentGateway = Substitute.For<IPremiumPaymentGateway>();

            _serviceAccessor = Substitute.For<Register.ServiceResolver>();
            _serviceAccessor.Invoke(Constants.CHEAP).Returns(_cheapPaymentGateway);
            _serviceAccessor.Invoke(Constants.EXPENSIVE).Returns(_expensivePaymentGateway);
            _serviceAccessor.Invoke(Constants.PREMIUM).Returns(_premiumPaymentGateway);

            _mapper.Map<PaymentEntities.Entities.Payment>(Arg.Any<PaymentRequestModel>()).Returns(GetPayment());

            _paymentBusiness = new PaymentBusiness.Payment.PaymentBusiness(_logger, _unitOfWork, _mapper, _serviceAccessor);
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_ForValidRequestHavingAmountLessThan20_CheapGatewayIsCalledAndReturnsProcessedStatusAsync()
        {
            //Arrange
            var model = GetPaymentRequestModel(9);

            _cheapPaymentGateway.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(Task.FromResult(PaymentStatus.Processed));

            _unitOfWork.PaymentsRepository.When(x => x.AddAsync(Arg.Any<PaymentEntities.Entities.Payment>())).Do(x => { });
            _unitOfWork.PaymentsRepository.When(x => x.UpdateAsync(Arg.Any<PaymentEntities.Entities.Payment>())).Do(x => { });
            _unitOfWork.When(x => x.Complete()).Do( x=> {});

            //Act
            var actual = await _paymentBusiness.ProcessPaymentAsync(model);

            //Assert

            Assert.AreEqual(PaymentStatus.Processed, actual);

            _serviceAccessor.Received(1).Invoke(Arg.Any<string>());
            await _cheapPaymentGateway.Received(1).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _unitOfWork.PaymentsRepository.Received(1).AddAsync(Arg.Any<PaymentEntities.Entities.Payment>());
            await _unitOfWork.PaymentsRepository.Received(1).UpdateAsync(Arg.Any<PaymentEntities.Entities.Payment>());
            _unitOfWork.Received(1).Complete();
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_ForValidRequestHavingAmountGreaterThan20AndLessThan500_ExpensiveGatewayIsCalledAndReturnsProcessedStatusAsync()
        {
            //Arrange
            var model = GetPaymentRequestModel(25);

            _expensivePaymentGateway.IsAvailableAsync()
                .Returns(Task.FromResult(true));

            _expensivePaymentGateway.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(Task.FromResult(PaymentStatus.Processed));

            _unitOfWork.PaymentsRepository.When(x => x.AddAsync(Arg.Any<PaymentEntities.Entities.Payment>())).Do(x => { });
            _unitOfWork.PaymentsRepository.When(x => x.AddAsync(Arg.Any<PaymentEntities.Entities.Payment>())).Do(x => { });
            _unitOfWork.When(x => x.Complete()).Do(x => { });

            //Act
            var actual = await _paymentBusiness.ProcessPaymentAsync(model);

            //Assert

            Assert.AreEqual(PaymentStatus.Processed, actual);

            _serviceAccessor.Received(1).Invoke(Arg.Any<string>());
            await _cheapPaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _expensivePaymentGateway.Received(1).IsAvailableAsync();
            await _expensivePaymentGateway.Received(1).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _unitOfWork.PaymentsRepository.Received(1).AddAsync(Arg.Any<PaymentEntities.Entities.Payment>());
            _unitOfWork.Received(1).Complete();
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_ForValidRequestHavingAmountGreaterThan500_PremiumGatewayIsCalledAndReturnsProcessedStatusAsync()
        {
            //Arrange
            var model = GetPaymentRequestModel(550);

            _premiumPaymentGateway.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(Task.FromResult(PaymentStatus.Processed));

            _unitOfWork.PaymentsRepository.When(x => x.AddAsync(Arg.Any<PaymentEntities.Entities.Payment>())).Do(x => { });
            _unitOfWork.When(x => x.Complete()).Do(x => { });

            //Act
            var actual = await _paymentBusiness.ProcessPaymentAsync(model);

            //Assert

            Assert.AreEqual(PaymentStatus.Processed, actual);

            _serviceAccessor.Received(1).Invoke(Arg.Any<string>());
            await _cheapPaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _expensivePaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _expensivePaymentGateway.Received(0).IsAvailableAsync();
            await _premiumPaymentGateway.Received(1).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _unitOfWork.PaymentsRepository.Received(1).AddAsync(Arg.Any<PaymentEntities.Entities.Payment>());
            _unitOfWork.Received(1).Complete();
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_ForValidRequestHavingAmountGreaterThan20AndExpensiveGatewayNotAvailable_CheapGatewayIsCalledAndReturnsProcessedStatusAsync()
        {
            //Arrange
            var model = GetPaymentRequestModel(25);

            _expensivePaymentGateway.IsAvailableAsync()
                .Returns(Task.FromResult(false));

            _cheapPaymentGateway.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(Task.FromResult(PaymentStatus.Processed));

            _unitOfWork.PaymentsRepository.When(x => x.AddAsync(Arg.Any<PaymentEntities.Entities.Payment>())).Do(x => { });
            _unitOfWork.When(x => x.Complete()).Do(x => { });

            //Act
            var actual = await _paymentBusiness.ProcessPaymentAsync(model);

            //Assert

            Assert.AreEqual(PaymentStatus.Processed, actual);

            _serviceAccessor.Received(2).Invoke(Arg.Any<string>());
            await _expensivePaymentGateway.Received(1).IsAvailableAsync();
            await _expensivePaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _unitOfWork.PaymentsRepository.Received(1).AddAsync(Arg.Any<PaymentEntities.Entities.Payment>());
            await _cheapPaymentGateway.Received(1).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            _unitOfWork.Received(1).Complete();
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_ForValidRequestHavingAmountGreaterThan500AndPremiumFailsFirst2TimesAndPassedNextTime_PremiumGatewayIsCalled3TimesAndReturnsProcessedStatusAsync()
        {
            //Arrange
            var model = GetPaymentRequestModel(550);

            _premiumPaymentGateway.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(x => throw new Exception(), x => throw new Exception(), x => Task.FromResult(PaymentStatus.Processed));

            _unitOfWork.PaymentsRepository.When(x => x.AddAsync(Arg.Any<PaymentEntities.Entities.Payment>())).Do(x => { });
            _unitOfWork.When(x => x.Complete()).Do(x => { });

            //Act
            var actual = await _paymentBusiness.ProcessPaymentAsync(model);

            //Assert

            Assert.AreEqual(PaymentStatus.Processed, actual);

            _serviceAccessor.Received(2).Invoke(Arg.Any<string>());
            await _cheapPaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _expensivePaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _expensivePaymentGateway.Received(0).IsAvailableAsync();
            await _premiumPaymentGateway.Received(3).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _unitOfWork.PaymentsRepository.Received(1).AddAsync(Arg.Any<PaymentEntities.Entities.Payment>());
            _unitOfWork.Received(1).Complete();
            _logger.Received(2).LogError(Arg.Any<Exception>());
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_ForValidRequestHavingAmountGreaterThan500AndPremiumFails4Times_PremiumGatewayIsCalled3TimesAndFailedStatusIsReturnedAsync()
        {
            //Arrange
            var model = GetPaymentRequestModel(540);

            _premiumPaymentGateway.ProcessPaymentAsync(Arg.Any<PaymentRequestModel>())
                .Returns(x => throw new Exception(), x => throw new Exception(), x => throw new Exception(), x => throw new Exception(), x => Task.FromResult(PaymentStatus.Processed));

            _logger.When(x => x.LogError(Arg.Any<Exception>())).Do(x => { });

            //Act
            var actual = await _paymentBusiness.ProcessPaymentAsync(model);

            //Assert

            Assert.AreEqual(PaymentStatus.Failed, actual);

            _serviceAccessor.Received(2).Invoke(Arg.Any<string>());
            await _cheapPaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _expensivePaymentGateway.Received(0).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _expensivePaymentGateway.Received(0).IsAvailableAsync();
            await _premiumPaymentGateway.Received(4).ProcessPaymentAsync(Arg.Any<PaymentRequestModel>());
            await _unitOfWork.PaymentsRepository.Received(1).AddAsync(Arg.Any<PaymentEntities.Entities.Payment>());
            _unitOfWork.Received(1).Complete();
            _logger.Received(4).LogError(Arg.Any<Exception>());
        }

        private PaymentRequestModel GetPaymentRequestModel(double amount)
        {
            return new PaymentRequestModel()
            {
                CreditCardNumber = "4111111111111111",
                CardHolder = "Nadeem",
                Amount = amount,
                ExpirationDate = DateTime.Today.AddDays(1),
                SecurityCode = "123"
            };
        }

        private PaymentEntities.Entities.Payment GetPayment()
        {
            return new PaymentEntities.Entities.Payment()
            {
                CreditCardNumber = "4111111111111111",
                CardHolder = "Nadeem",
                Amount = 100,
                ExpirationDate = DateTime.Today.AddDays(1),
                SecurityCode = "123"
            };
        }
    }
}

using System;
using CreditCardValidator;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentApi.Validators;

namespace PaymentApiTests.Validators
{
    /// <summary>
    /// Test class to test PaymentRequestModelValidator.
    /// </summary>
    [TestClass]
    public class PaymentRequestModelValidatorTest
    {
        /// <summary>
        /// For null card number.
        /// </summary>
        [TestMethod]
        public void WhenCreditCardNumberNull_ShouldHaveError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.CreditCardNumber, null as string);
        }

        /// <summary>
        /// Valid card number.
        /// </summary>
        [TestMethod]
        public void WhenHaveValidCreditCardNumber_ShouldHaveNoError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldNotHaveValidationErrorFor(m => m.CreditCardNumber, GetValidCardNumber());
        }

        /// <summary>
        /// Invalid card number.
        /// </summary>
        [TestMethod]
        public void WhenHaveInvalidCreditCardNumber_ShouldHaveError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.CreditCardNumber, "123848484849");
        }

        /// <summary>
        /// Null CardHolder name.
        /// </summary>
        [TestMethod]
        public void WhenCardHolderNull_ShouldHaveError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.CardHolder, null as string);
        }

        /// <summary>
        /// Valid card holder name.
        /// </summary>
        [TestMethod]
        public void WhenHaveValidCardHolder_ShouldHaveNoError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldNotHaveValidationErrorFor(m => m.CardHolder, "Test");
        }

        /// <summary>
        /// Default expiration date.
        /// </summary>
        [TestMethod]
        public void WhenExpirationDateDefault_ShouldHaveError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.ExpirationDate, new DateTime());
        }

        /// <summary>
        /// Valid non expired expiration date.
        /// </summary>
        [TestMethod]
        public void WhenHaveValidExpirationDate_ShouldHaveNoError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldNotHaveValidationErrorFor(m => m.ExpirationDate, DateTime.Now.AddDays(1));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void WhenHaveInvalidExpirationDate_ShouldHaveError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.ExpirationDate, DateTime.Now.AddDays(-1));
        }

        /// <summary>
        /// Zero amount transaction.
        /// </summary>
        [TestMethod]
        public void WhenAmountIsZero_ShouldHaveError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Amount, 0);
        }

        /// <summary>
        /// Invalid amount transaction.
        /// </summary>
        [TestMethod]
        public void WhenAmountIsNegative_ShouldHaveError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Amount, -0.5);
        }

        /// <summary>
        /// Valid amount transaction.
        /// </summary>
        [TestMethod]
        public void WhenValidAmount_ShouldHaveNoError()
        {
            var sut = new PaymentRequestModelValidator();
            sut.ShouldNotHaveValidationErrorFor(m => m.Amount, 0.5);
        }

        /// <summary>
        /// Provides a Valid VISA card number.
        /// </summary>
        /// <returns></returns>
        private string GetValidCardNumber()
        {
            return CreditCardFactory.RandomCardNumber(CardIssuer.Visa);
        }
    }
}

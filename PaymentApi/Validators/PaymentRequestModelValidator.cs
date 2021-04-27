using System;
using CreditCardValidator;
using FluentValidation;
using PaymentCommon.Models;
using PaymentCommon.Resources;

namespace PaymentApi.Validators
{
    /// <summary>
    /// PaymentRequestModelValidator class.
    /// </summary>
    public class PaymentRequestModelValidator : AbstractValidator<PaymentRequestModel>
    {
        /// <summary>
        /// API Fluent Validation
        /// </summary>
        public PaymentRequestModelValidator()
        {
            
            RuleFor(m => m.CreditCardNumber)
                .NotEmpty()
                .WithMessage(og => string.Format(MessageResource.InvalidParameter, "creditCardNumber"));

            RuleFor(m => m.CreditCardNumber)
                .Must(IsValidCard).When(x => !string.IsNullOrWhiteSpace(x.CreditCardNumber))
                .WithMessage(og => string.Format(MessageResource.InvalidCard));

            RuleFor(m => m.CardHolder)
                .NotEmpty()
                .WithMessage(og => string.Format(MessageResource.InvalidParameter, "cardHolder"));

            RuleFor(m => m.ExpirationDate)
                .NotEmpty()
                .WithMessage(og => string.Format(MessageResource.InvalidParameter, "expirationDate"));

            RuleFor(m => m.ExpirationDate)
                .Must(x => x.Date >= DateTime.Now.Date)
                .WithMessage(og => string.Format(MessageResource.MustBeFutureDate, "expirationDate"));

            RuleFor(m => m.Amount)
                .NotEmpty()
                .WithMessage(og => string.Format(MessageResource.InvalidParameter, "amount"));

            RuleFor(m => m.Amount)
                .GreaterThan(0)
                .WithMessage(og => string.Format(MessageResource.ParameterMustBePositive, "amount"));

            RuleFor(m => m.SecurityCode)
                .Length(3)
                .When(x => !string.IsNullOrWhiteSpace(x?.SecurityCode))
                .WithMessage(og => string.Format(MessageResource.CvvLengthError, "securityCode", 3));

        }

        /// <summary>
        /// Card validation using CreditCardDetector package which usage Luhn algorithm.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        private static bool IsValidCard(string cardNumber)
        {
            cardNumber = cardNumber.Replace("-", "");
            cardNumber = cardNumber.Replace(" ", "");
            CreditCardDetector detector = new CreditCardDetector(cardNumber);
            return detector.IsValid();
        }
    }
}

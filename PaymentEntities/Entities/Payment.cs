using System;

namespace PaymentEntities.Entities
{
    public class Payment: BaseEntity
    {
        public string CreditCardNumber { get; set; }

        /// <summary>Card holder name.</summary>
        public string CardHolder { get; set; }

        /// <summary>Card expiration date.</summary>
        public DateTimeOffset ExpirationDate { get; set; }

        /// <summary>Security code.</summary>
        public string SecurityCode { get; set; }

        /// <summary>Amount to be processed.</summary>
        public double Amount { get; set; }

        public int PaymentStatusId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}

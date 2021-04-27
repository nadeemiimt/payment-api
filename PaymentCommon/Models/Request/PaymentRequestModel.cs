using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PaymentCommon.Models
{
    /// <summary>This defines a payment details required for processing.</summary>
    /// Note: Fluent Validation can also be used if there are custom validations.
    public class PaymentRequestModel
    {
        /// <summary>Credit card number.</summary>
        [JsonProperty("creditCardNumber", Required = Required.Always)]
        [Required(AllowEmptyStrings = false)]
        public string CreditCardNumber { get; set; }

        /// <summary>Card holder name.</summary>
        [JsonProperty("cardHolder", Required = Required.Always)]
        [Required(AllowEmptyStrings = false)]
        public string CardHolder { get; set; }

        /// <summary>Card expiration date.</summary>
        [JsonProperty("expirationDate", Required = Required.Always)]
        [Required(AllowEmptyStrings = false)]
        public DateTime ExpirationDate { get; set; }

        /// <summary>Security code.</summary>
        [JsonProperty("securityCode", Required = Required.AllowNull, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(3, MinimumLength = 3)]
        public string SecurityCode { get; set; }

        /// <summary>Amount to be processed.</summary>
        [JsonProperty("amount", Required = Required.Always)]
        public double Amount { get; set; }


    }
}

using System;
using Newtonsoft.Json;

namespace PaymentCommon.Models
{
    /// <summary>
    /// Custom Error Details.
    /// </summary>
    public class CustomErrorDetails
    {
        /// <summary>Application status code specific to operation</summary>
        [JsonProperty("status", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }

        /// <summary>Trace id.</summary>
        [JsonProperty("traceId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string TraceId { get; set; }

        /// <summary>Friendly error message (any serializable type)</summary>
        [JsonProperty("message", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public object Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

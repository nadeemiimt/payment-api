using Newtonsoft.Json;

namespace PaymentCommon.Models.Response
{
    /// <summary>
    /// Error details.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>Error type details.</summary>
        [JsonProperty("type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>Error title.</summary>
        [JsonProperty("title", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>Error status code.</summary>
        [JsonProperty("status", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }

        /// <summary>Trace id.</summary>
        [JsonProperty("traceId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string TraceId { get; set; }

        /// <summary>Friendly error message (any serializable type)</summary>
        [JsonProperty("errors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public object Errors { get; set; }
    }
}

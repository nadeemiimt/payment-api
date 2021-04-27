namespace PaymentCommon.Resources
{
    /// <summary>
    /// Error message resources.
    /// </summary>
    public static class MessageResource
    {
        /// <summary>
        ///   Looks up a string similar to Invalid value for parameter '{0}'.
        /// </summary>
        public const string InvalidParameter = "Invalid value for parameter '{0}'.";

        /// <summary>
        ///   Looks up a string similar to Parameter '{0}' must be positive.
        /// </summary>
        public const string ParameterMustBePositive = "Parameter '{0}' must be positive.";

        /// <summary>
        ///   Looks up a string similar to Parameter '{0}' must have length greater than {1}.
        /// </summary>
        public const string CvvLengthError = "Parameter '{0}' must have length greater than {1}.";

        /// <summary>
        ///   Looks up a string similar to Invalid card number.
        /// </summary>
        public const string InvalidCard = "Invalid card number.";

        /// <summary>
        ///   Looks up a string similar to Parameter {0} should contain future date.
        /// </summary>
        public const string MustBeFutureDate = "Parameter {0} should contain future date.";
    }
}

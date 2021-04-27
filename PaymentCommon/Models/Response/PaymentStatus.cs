namespace PaymentCommon.Models.Response
{
    /// <summary>
    /// Possible payment status values.
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 1,
        Processed,
        Failed
    }
}

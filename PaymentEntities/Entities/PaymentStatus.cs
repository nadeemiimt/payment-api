namespace PaymentEntities.Entities
{
    public class PaymentStatus: BaseEntity
    {
        public Status Status { get; set; }

        public Payment Payment { get; set; }

        public int PaymentId { get; set; }
        public int StatusId { get; set; }
    }
}

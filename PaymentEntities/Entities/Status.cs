namespace PaymentEntities.Entities
{
    public class Status: BaseEntity
    {
        public string Name { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}

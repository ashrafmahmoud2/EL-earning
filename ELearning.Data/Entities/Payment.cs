namespace ELearning.Data.Entities
{
    public class Payment : AuditableEntity
    {
        public Guid PaymentId { get; set; } = Guid.CreateVersion7();
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public Guid EnrollmentId { get; set; }
        public string? StripePaymentIntentId { get; set; }

        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public Enrollment Enrollment { get; set; } = default!;
    }

}



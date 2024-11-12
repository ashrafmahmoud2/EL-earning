using ELearning.Data.Enums;

namespace ELearning.Data.Entities;

public class Enrollment : AuditableEntity
{
    public Guid EnrollmentId { get; set; } = Guid.CreateVersion7();

    public bool IsActive { get; set; } = true;

    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }

    public DateTime enrolledAt { get; set; } = DateTime.Now;

    public DateTime? completedAt { get; set; } = default!;

    public string Status { get; set; }=string.Empty;


    public Student student { get; set; } = default!;

    public Course course { get; set; } = default!;

    public ICollection<Payment> Payments { get; set; } = [];


}



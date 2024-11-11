namespace ELearning.Data.Entities;

public class Instructor : AuditableEntity
{
    public Guid InstructorId { get; set; } = Guid.CreateVersion7();

    public ApplicationUser User { get; set; } = default!;

    public string Expertise  { get; set; } = String.Empty;

    public string Biography { get; set; } = String.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime HireDate { get; set; } = DateTime.UtcNow;

    public ICollection<Course> Courses { get; set; } = [];

}



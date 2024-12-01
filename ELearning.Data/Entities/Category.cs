namespace ELearning.Data.Entities;

public class Category : AuditableEntity
{
    public Guid CategoryId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
   // public int NumberOfCourses { get; set; } 
    public bool IsActive { get; set; } = true;

    public ICollection<Course> Courses { get; set; } = [];
}




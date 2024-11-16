namespace ELearning.Data.Entities;

public class Course : AuditableEntity
{

    public Guid CourseId { get; set; } = Guid.CreateVersion7();
    public bool IsActive { get; set; } = true;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal Price { get; set; } = decimal.Zero;
    public Guid InstructorId { get; set; }
    public Guid CategoryId { get; set; } 
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string TrailerVideoUrl { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public List<string> Prerequisites { get; set; } = [];
    public List<string> LearningObjectives { get; set; } = [];
    public TimeSpan? TotalTime { get; set; } = null;


    public Category Category { get; set; } = default!;
    public Instructor Instructor { get; set; } = default!;
    public ICollection<Enrollment> Enrollments { get; set; } = [];
    public ICollection<Section> sections { get; set; } = [];
}




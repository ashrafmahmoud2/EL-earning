namespace ELearning.Data.Entities;

 public class Lesson : AuditableEntity
 {
     public Guid LessonId { get; set; } = Guid.CreateVersion7();
     public string Title { get; set; } = string.Empty;
     public string? Description { get; set; } 
     public int OrderIndex { get; set; }
     public bool IsActive { get; set; } = true;
     public Guid SectionId { get; set; }
     public Section Section { get; set; } = default!;
     public ICollection<Quiz> Quizzes { get; set; } = [];
    public ICollection<Document> Documents { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
 }








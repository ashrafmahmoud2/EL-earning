namespace ELearning.Data.Entities
{

    public class Section : AuditableEntity
    {
        public Guid SectionId { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = default!;

        public ICollection<Lesson> Lessons { get; set; } = [];
    }

}



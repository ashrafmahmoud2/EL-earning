using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Entities;
public class Student : AuditableEntity
{
    public Guid StudentId { get; set; } = Guid.CreateVersion7();

    public ApplicationUser User { get; set; } = default!;

    public string UserId { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;


    public ICollection<Enrollment> Enrollments { get; set; } = [];

    public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
}


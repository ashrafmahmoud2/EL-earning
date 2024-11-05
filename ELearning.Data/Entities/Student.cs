using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Entities;
public class Student:AuditableEntity
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    //public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

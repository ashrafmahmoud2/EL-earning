using ELearning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Service.IService;
public interface IStudentService
{
    Task<Student> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> GetAllStudentsAsync(CancellationToken cancellationToken = default);
    Task<Student> CreateStudentAsync(Student student, CancellationToken cancellationToken = default);
    Task<Student> UpdateStudentAsync(Student student, CancellationToken cancellationToken = default);
    Task DeleteStudentAsync(Guid id, CancellationToken cancellationToken = default);
}

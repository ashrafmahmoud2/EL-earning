using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Students;
using ELearning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ELearning.Service.IService;
public interface IStudentService
{
    Task<Result<StudentResponse>> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentResponse>> GetAllStudentsAsync(CancellationToken cancellationToken = default);
    Task<Result<StudentResponse>> CreateStudentAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<Result<StudentResponse>> UpdateStudentAsync(Guid id, StudentRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Core.MediatrHandlers.Student.Queries.GetStudentById;
public class GetStudentByIdQuery : IRequest<ApiResponse<StudentDto>>
{
    public Guid Id { get; set; }
}

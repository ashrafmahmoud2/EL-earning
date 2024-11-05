using ELearning.Core.Base.ApiResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;
public class UpdateStudentCommand : IRequest<ApiResponse<StudentDto>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    // Add other properties as needed
}

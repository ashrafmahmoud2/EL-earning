using ELearning.Core.Base.ApiResponse;
using ELearning.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ELearning.Core.MediatrHandlers.Student.Commands.UpdateStudent;
public class UpdateCommentCommand : IRequest<ApiResponse<CommentDto>>
{
    public Guid id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CommentText { get; set; } = string.Empty;
    public Guid LessonId { get; set; } 
    public string ApplicationUserID { get; set; } = string.Empty;
}

 
using ELearning.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Core.MediatrHandlers.Student.Queries.GetCommenByIdQuery;
public class GetCommentByIdQuery : IRequest<ApiResponse<CommentDto>>
{
    public Guid Id { get; set; }
}

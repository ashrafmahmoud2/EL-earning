using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Comment;

public record CommentResponse
(
     Guid CommentId,
       string Title,
     string CommentText,
     Guid LessonId,
     string CommentedByUserId,
     string CommentedBy,
     bool IsActive
);

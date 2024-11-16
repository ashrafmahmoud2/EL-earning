namespace ELearning.Data.Contracts.Comment;

public record CommentRequest
(
     string Title,
     string CommentText,
     Guid LessonId,
     string ApplicationUserID
);

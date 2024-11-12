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

    public bool IsActive { get; set; } = true;


    public ICollection<Enrollment> Enrollments { get; set; } = [];
    //  public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
}

//public class Question : AuditableEntity
//{
//    public Guid QuestionId { get; set; } = Guid.NewGuid();
//    public string Text { get; set; } = string.Empty;
//    public int OrderIndex { get; set; }
//    public bool IsActive { get; set; } = true;
//    public Guid QuizId { get; set; }
//    public Quiz Quiz { get; set; } = default!;
//    public ICollection<Answer> Answers { get; set; } = [];
//}

//public class Answer : AuditableEntity
//{
//    public Guid AnswerId { get; set; } = Guid.NewGuid();
//    public string Text { get; set; } = string.Empty;
//    public bool IsCorrect { get; set; } = false;
//    public bool IsActive { get; set; } = true;
//    public Guid QuestionId { get; set; }
//    public Question Question { get; set; } = default!;
//}

//public class QuizAttempt : AuditableEntity
//{
//    public Guid QuizAttemptId { get; set; } = Guid.NewGuid();
//    public DateTime AttemptDate { get; set; } = DateTime.UtcNow;
//    public Guid QuizId { get; set; }
//    public Guid StudentId { get; set; }
//    public bool IsPassed { get; set; }
//    public int Score { get; set; }
//    public ICollection<Question> Questions { get; set; } = [];
//    public ICollection<Answer> SelectedAnswers { get; set; } = [];

//    public Student student { get; set; } = default!;
//    public Quiz Quiz { get; set; } = default!;
//}


//public class Comment : AuditableEntity
//{
//    public Guid CommentId { get; set; } = Guid.NewGuid();
//    public string Title { get; set; } = string.Empty;
//    public string CommentText { get; set; } = string.Empty;
//    public bool IsActive { get; set; } = true;
//    public Guid LessonId { get; set; }
//    public Lesson Lesson { get; set; } = default!;
//}





//public class Question : AuditableEntity
//{
//    public Guid QuestionId { get; set; } = Guid.NewGuid();
//    public string Text { get; set; } = string.Empty;
//    public int OrderIndex { get; set; }
//    public bool IsActive { get; set; } = true;
//    public Guid QuizId { get; set; }
//    public Quiz Quiz { get; set; } = default!;
//    public ICollection<Answer> Answers { get; set; } = [];
//}

//public class Answer : AuditableEntity
//{
//    public Guid AnswerId { get; set; } = Guid.NewGuid();
//    public string Text { get; set; } = string.Empty;
//    public bool IsCorrect { get; set; } = false;
//    public bool IsActive { get; set; } = true;
//    public Guid QuestionId { get; set; }
//    public Question Question { get; set; } = default!;
//}

//public class QuizAttempt : AuditableEntity
//{
//    public Guid QuizAttemptId { get; set; } = Guid.NewGuid();
//    public DateTime AttemptDate { get; set; } = DateTime.UtcNow;
//    public Guid QuizId { get; set; }
//    public Guid StudentId { get; set; }
//    public bool IsPassed { get; set; }
//    public int Score { get; set; }
//    public ICollection<Question> Questions { get; set; } = [];
//    public ICollection<Answer> SelectedAnswers { get; set; } = [];

//    public Student student { get; set; } = default!;
//    public Quiz Quiz { get; set; } = default!;
//}





//public class Comment : AuditableEntity
//{
//    public Guid CommentId { get; set; } = Guid.NewGuid();
//    public string Title { get; set; } = string.Empty;
//    public string CommentText { get; set; } = string.Empty;
//    public bool IsActive { get; set; } = true;
//    public Guid LessonId { get; set; }
//    public Lesson Lesson { get; set; } = default!;
//}


//public class Comment : AuditableEntity
//{
//    public Guid CommentId { get; set; } = Guid.NewGuid();
//    public string Title { get; set; } = string.Empty;
//    public string CommentText { get; set; } = string.Empty;
//    public bool IsActive { get; set; } = true;
//    public Guid LessonId { get; set; }
//    public Lesson Lesson { get; set; } = default!;
//}



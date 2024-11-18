using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record CategorysErrors
{
    public static readonly Error CategoryNotFound =
        new("Category.CategoryNotFound", "Category is not found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidPermissions =
        new("Category.InvalidPermissions", "Invalid permissions", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedCategory =
        new("Category.DuplicatedCategory", "Another Category with the same name is already exists", StatusCodes.Status409Conflict);
}

/*
CategoryErrors
CommentErrors
CourseErrors
DocumentErrors
EnrollmentErrors
InstructorErrors
LessonErrors
PaymentErrors
QuestionErrors
QuizAttemptErrors
QuizErrors
RoleErrors
SectionErrors
StudentErrors
UserErrors

*/


/* new
 Answers
CategorysErrors
CommentsErrors
CoursesErrors
DocumentsErrors
EnrollmentErrors
InstructorsErrors
LessonsErrors
PaymentErrors
QuestionsErrors
QuizAttemptsErrors
QuizsErrors
RoleErrors
SectionsErrors
StudentsErrors
UserErrors
*/
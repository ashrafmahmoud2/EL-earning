using ELearning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data.Contracts.Document;
public record DocumentResponse
(
    Guid DocumentId,
    string Title,
    string? Description,
    string DocumentPath,
    bool IsActive,
    Guid LessonId
   // string LessoneName
   //string CreatedBy
 );


select* From Answers --stop in make DocumentResponse;
select* From Categorys
select* From Comments
select* From Courses
select* From Documents
select* From Enrollment
select* From Instructors
select* From Lessons
select* From Payment
select* From Questions
select* From Quizs
select* From QuizAttempts
select* From Sections
select* From Students
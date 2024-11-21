using Azure;
using ELearning.Data.Contracts.Answer;
using ELearning.Data.Contracts.Users;
using ELearning.Data.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Data.Contracts.Categorys;
using ELearning.Data.Contracts.Comment;
using ELearning.Data.Contracts.Course;
using ELearning.Data.Contracts.Enrollment;
using ELearning.Data.Contracts.Instrctors;
using ELearning.Data.Contracts.Lesson;
using ELearning.Data.Contracts.Payment;
using ELearning.Data.Contracts.Question;
using ELearning.Data.Contracts.QuizAttempt;
using ELearning.Data.Contracts.Quiz;
using ELearning.Data.Contracts.Section;
using ELearning.Data.Contracts.Students;

namespace ELearning.Service.Mapping;
public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {



        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);


        config.NewConfig<CreateUserRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Email)
           .Map(dest => dest.EmailConfirmed, src => true);


        config.NewConfig<UpdateUserRequest, ApplicationUser>()
          .Map(dest => dest.UserName, src => src.Email)
          .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper());



        //Answer
        TypeAdapterConfig<Answer, AnswerResponse>.NewConfig()
        .Map(dest => dest.CreatedBy, src => src.CreatedBy.FirstName + " " + src.CreatedBy.LastName);

        config.NewConfig<Question, AnswerResponse>()
         .Map(dest => dest.QuestionText, src => src.Text);


        //Category

        config.NewConfig<Category, CategoryResponse>()
        // .Map(dest => dest.CreatedBy, src => src.CreatedBy.FirstName + "" + src.CreatedBy.LastName);
        .Map(dest => dest.CreatedBy, src => src.CreatedBy != null
     ? src.CreatedBy.FirstName + " " + src.CreatedBy.LastName
     : "Unknown");



        //Comment
        config.NewConfig<Comment, CommentResponse>()
            .Map(dest => dest.CommentedBy, serc => serc.ApplicationUser.FirstName + " " + serc.ApplicationUser.LastName);

       


        //Course
        config.NewConfig<Course, CourseResponse>()
            .Map(dest => dest.InstructorName, serc => serc.Instructor.User.FirstName + " " + serc.Instructor.User.LastName)
            .Map(dest => dest.TotalTime, serc => serc.TotalTime ?? TimeSpan.Zero);  // Handle nullable TotalTime


        //Enrollment
        config.NewConfig<Enrollment, EnrollmentResponse>()
           .Map(dest => dest.StudentName, src => $"{src.student.User.FirstName} {src.student.User.LastName}")
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
           .Map(dest => dest.CourseTitle, src => src.course.Title);


        //Instructor
        config.NewConfig<Instructor, InstructorResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
           .Map(dest => dest.InstructorName, src => $"{src.User.FirstName} {src.User.LastName}")
           .Map(dest => dest.Email, src => src.User.Email);


        //Student
        config.NewConfig<Student, StudentResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
           .Map(dest => dest.StudentName, src => $"{src.User.FirstName} {src.User.LastName}")
             .Map(dest => dest.Email, src => src.User.Email);

        //Lesson
        config.NewConfig<Lesson, LessonResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
           .Map(dest => dest.sectionName, src => src.Section.Title);

        //Category
        config.NewConfig<Category, CategoryResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}");
        


        //Payment
        config.NewConfig<Payment, PaymentResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
           .Map(dest => dest.StudentName, src =>$"{src.Enrollment.student.User.FirstName} {src.Enrollment.student.User.LastName}" )
           .Map(dest => dest.StudentId, src => src.Enrollment.StudentId)
           .Map(dest => dest.CourseId, src => src.Enrollment.CourseId)
           .Map(dest => dest.CourseTitle, src => src.Enrollment.course.Title);


        //Question
        config.NewConfig<Question, QuestionResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
           .Map(dest => dest.QuizTitle, src => src.Quiz.Title);

        //QuizAttempt
        config.NewConfig<QuizAttempt, QuizAttemptResponse>()
           .Map(dest => dest.QuizTitle, src => src.Quiz.Title)
           .Map(dest => dest.StudentName, src => $"{src.student.User.FirstName} {src.student.User.LastName}");


        //Quiz
        config.NewConfig<Quiz, QuizResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
           .Map(dest => dest.LessoneName, src => src.Lesson.Title);

        //Section
        config.NewConfig<Section, SectionResponse>()
           .Map(dest => dest.CreatedBy, src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}")
                      .Map(dest => dest.CourseTitle, src => src.Course.Title);



    }
}

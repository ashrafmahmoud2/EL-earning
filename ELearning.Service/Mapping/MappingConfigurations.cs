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




    }
}

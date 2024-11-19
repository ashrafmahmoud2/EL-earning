using ELearning.Core.DTOs;
using ELearning.Data.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Core.Mapping;
public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

      //  config.NewConfig<Comment, CommentDto>()
      //.Map(dest => dest.CommentedBy,
      //     src => src.ApplicationUser != null ? src.ApplicationUser.FirstName + " " + src.ApplicationUser.LastName : "Unknown");

    }
}

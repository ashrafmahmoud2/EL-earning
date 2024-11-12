using ELearning.Data.Contracts.Users;
using ELearning.Data.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

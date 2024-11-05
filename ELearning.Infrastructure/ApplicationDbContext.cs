using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ELearning.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace ELearning.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<Data.Entities.ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }


}
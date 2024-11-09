using ELearning.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using ELearning.Data.Abstractions.ResultPattern;

using ELearning.Data.Errors;
using ELearning.Infrastructure;
using ELearning.Data.Contracts.Students;
using Mapster;
namespace ELearning.Service.Service;

public class StudentService : BaseRepository<Student>, IStudentService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public StudentService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<StudentResponse>> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var students = await _unitOfWork.Repository<Student>()
                                         .FindAsync(x => x.StudentId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var student = students.FirstOrDefault();

        if (student is null)
            return Result.Failure<StudentResponse>(StudentErrors.StudentNotFound);

        var studentResponse = student.Adapt<StudentResponse>();

        return Result.Success(studentResponse);
    }

    public async Task<Result<StudentResponse>> CreateStudentAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        if (user is null)
            Result.Failure(StudentErrors.StudentNotFound);

        var student = new Student()
        {
            User = user!
        };
        await _unitOfWork.Repository<Student>().AddAsync(student, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(student.Adapt<StudentResponse>());
    }

    public async Task<IEnumerable<StudentResponse>> GetAllStudentsAsync(CancellationToken cancellationToken = default)
    {


        var students = await _unitOfWork.Repository<Student>()
            .FindAsync(
                s => true,
include: q => q.Include(s => s.CreatedBy).Include(s => s.User),
                cancellationToken: cancellationToken);




        return students.Select(s => new StudentResponse(
       StudentId: s.StudentId,
       StudentName: s.User != null ? s.User.FirstName + " " + s.User.LastName : "Unknown",
       CreatedBy: s.CreatedBy.FirstName + " " + s.CreatedBy.LastName,
       CreatedOn: s.CreatedOn,
      Email: s.User?.Email ?? "No Email",
            IsActive: s.IsActive

   )).ToList();

    }

    public async Task<Result<StudentResponse>> UpdateStudentAsync(Guid id, StudentRequest request, CancellationToken cancellationToken = default)
    {
        var students = await _unitOfWork.Repository<Student>()
                                         .FindAsync(x => x.StudentId == id,
                                         q => q.Include(x => x.CreatedBy).Include(x => x.User), cancellationToken);
        var student = students.FirstOrDefault();

        if (student.User is null)
            return Result.Failure<StudentResponse>(StudentErrors.StudentNotFound);

        if (await _unitOfWork.Repository<Student>().AnyAsync(x => x.User.Email == request.Email && x.StudentId != id, cancellationToken))
            return Result.Failure<StudentResponse>(StudentErrors.DuplicatedStudent);

        // Update student details
        student.User.FirstName = request.FirstName;
        student.User.LastName = request.LastName;
        student.User.Email = request.Email;
        student.User.UserName = request.Email;

       await _unitOfWork.Repository<Student>().UpdateAsync(student,cancellationToken);
       await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(student.Adapt<StudentResponse>());
    }


    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var instructors = await _unitOfWork.Repository<Student>()
                                           .FindAsync(x => x.StudentId == id, q => q.Include(x => x.CreatedBy).Include(x => x.User), cancellationToken);
        var instructor = instructors.FirstOrDefault();

        if (instructor is null)
            return Result.Failure(StudentErrors.StudentNotFound);

        instructor.IsActive = !instructor.IsActive;

        // Save changes to the database
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

}

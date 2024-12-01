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
using Azure.Core;
using ELearning.Data.Contracts.Comment;
using Mailjet.Client.Resources;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ELearning.Data.Contracts.Lesson;
using ELearning.Data.Contracts.Enrollment;
namespace ELearning.Service.Service;



public class StudentService : BaseRepository<Student>, IStudentService
{

    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);


    public StudentService(ApplicationDbContext context, IUnitOfWork unitOfWork, ICacheService cacheService) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<StudentResponse>> GetStudentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Repository<Student>()
                                         .FirstOrDefaultAsync(x => x.StudentId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                                .Include(x => x.User)
                                         , cancellationToken);

        if (student is null)
            return Result.Failure<StudentResponse>(StudentsErrors.NotFound);

        var studentResponse = student.Adapt<StudentResponse>();

        return Result.Success(studentResponse);
    }

    public async Task<Result<IEnumerable<StudentResponse>>> GetAllStudentsAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "Student:GetAll";

        var cachedResult = await _cacheService.GetCacheAsync<IEnumerable<StudentResponse>>(cacheKey);

        if (cachedResult.IsSuccess && cachedResult.Value != null)
            return Result.Success(cachedResult.Value);


        if (cachedResult.IsFailure && cachedResult.Error != CashErrors.NotFound)
            return Result.Failure<IEnumerable<StudentResponse>>(cachedResult.Error);


        var students = await _unitOfWork.Repository<Student>()
            .FindAsync(
                s => s.IsActive,
                 q => q.Include(s => s.CreatedBy)
                 .Include(s => s.User),
                cancellationToken: cancellationToken);


        var studentsResponses = students.Adapt<IEnumerable<StudentResponse>>();

        // Cache the adapted response
        await _cacheService.SetCacheAsync(cacheKey, studentsResponses, _cacheDuration);

        return Result.Success(studentsResponses); 
    }

    public async Task<Result> CreateStudentAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == user.Id))
            return Result.Failure<StudentResponse>(UserErrors.UserNotFound);

        if (user is null)
            Result.Failure(StudentsErrors.NotFound);

        var student = new Student()
        {
            User = user!
        };
        await _unitOfWork.Repository<Student>().AddAsync(student, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Student:GetAll");
        
        return Result.Success();
    }

    public async Task<Result<StudentResponse>> UpdateStudentAsync(Guid id, StudentRequest request, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Repository<Student>()
                                         .FirstOrDefaultAsync(x => x.StudentId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                                .Include(x => x.User)
                                         , cancellationToken);

        if (student.User is null)
            return Result.Failure<StudentResponse>(StudentsErrors.NotFound);

        if (await _unitOfWork.Repository<Student>().AnyAsync(x => x.User.Email == request.Email && x.StudentId != id, cancellationToken))
            return Result.Failure<StudentResponse>(StudentsErrors.DuplicatedStudent);

        // Update student details
        student.User.FirstName = request.FirstName;
        student.User.LastName = request.LastName;
        student.User.Email = request.Email;
        student.User.UserName = request.Email;

        await _unitOfWork.Repository<Student>().UpdateAsync(student, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);


        // Remove the cached students
        await _cacheService.RemoveCacheAsync("Student:GetAll");

        return Result.Success(student.Adapt<StudentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _unitOfWork.Repository<Student>()
                                         .FirstOrDefaultAsync(x => x.StudentId == id ,
                                         q => q.Include(x => x.CreatedBy)
                                                .Include(x => x.User)
                                         , cancellationToken);

        if (student is null)
            return Result.Failure(StudentsErrors.NotFound);

        student.IsActive = !student.IsActive;

        // Save changes to the database
        await _unitOfWork.CompleteAsync(cancellationToken);


        // Remove the cached students
        await _cacheService.RemoveCacheAsync("Student:GetAll");

        return Result.Success();
    }

    public async Task<Result> DeleteStudentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _unitOfWork.Repository<Student>()
                .FirstOrDefaultAsync(
                    x => x.StudentId == id && x.IsActive,
                    q => q.Include(x => x.CreatedBy)
                          .Include(x => x.User),
                    cancellationToken);

            if (student is null)
            {
                return Result.Failure(StudentsErrors.NotFound);
            }

            // Delete related user if exists
            if (student.User != null)
            {
                await _unitOfWork.Repository<ApplicationUser>().RemoveAsync(student.User, cancellationToken);
            }

            // Delete the student record
            await _unitOfWork.Repository<Student>().RemoveAsync(student, cancellationToken);

            // Commit transaction
            await _unitOfWork.CompleteAsync(cancellationToken);


            // Remove the cached students
            await _cacheService.RemoveCacheAsync("Student:GetAll");

            return Result.Success();
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            //stop in make the why error genric;
            var sqlErrorMessage = sqlEx.Message ?? "Foreign key violation occurred.";

            var allTablesInDb = GetAllTablesInDb();
            string foreignKeyTable = null;

            foreach (var table in allTablesInDb)
            {
                if (sqlErrorMessage.Contains(table))
                {
                    foreignKeyTable = table;
                    break;
                }
            }

            return Result.Failure(StudentsErrors.ForeignKeyViolation with { Description = $"The student cannot be deleted because {foreignKeyTable} ." });
        }
        catch (Exception ex)
        {
            return Result.Failure(
            StudentsErrors.UnexpectedError with { Description = $"An unexpected error occurred: {ex.Message}" });

        }
    }

    private string GetForeignKeyTableFromError(string sqlerrormessage)
    {
        var alltablesindb = GetAllTablesInDb(); // replace with your logic to get all tables
        foreach (var table in alltablesindb)
        {
            if (sqlerrormessage.Contains(table))
            {
                return table;
            }
        }
        return "unknown table";
    }

    private List<string> GetAllTablesInDb()
    {
        var tables = _context.Model.GetEntityTypes()
            .Select(t => t.GetTableName())  // Get the table names
            .ToList();  // Use ToList instead of ToListAsync

        return tables;
    }

}

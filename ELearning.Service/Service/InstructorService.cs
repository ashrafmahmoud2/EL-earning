using ELearning.Service.IService;
using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;

using ELearning.Data.Errors;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Data.Contracts.Students;
using ELearning.Data.Contracts.Instrctors;
namespace ELearning.Service.Service;

public class InstructorService : BaseRepository<Instructor>, IInstructorService
{


    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public InstructorService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<InstructorResponse>> GetAllInstructorsAsync(CancellationToken cancellationToken = default)
    {
        var Instructors = await _unitOfWork.Repository<Instructor>()
            .FindAsync(
                s => true,
                include: q => q.Include(s => s.CreatedBy).Include(s => s.User),
                cancellationToken: cancellationToken);


        return Instructors.Select(s => new InstructorResponse(
               InstructorId: s.InstructorId,
               InstructorName: s.User != null ? s.User.FirstName + " " + s.User.LastName : "Unknown",
               CreatedBy: s.CreatedBy.FirstName + " " + s.CreatedBy.LastName,
               CreatedOn: s.CreatedOn,
               Email: s.User?.Email ?? "No Email",
               IsActive: s.IsActive
           )).ToList();
    }

    public async Task<Result<InstructorResponse>> GetInstructorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Fetch the instructor with the given ID and include related entities
        var instructors = await _unitOfWork.Repository<Instructor>()
                                           .FindAsync(x => x.InstructorId == id, q => q.Include(x => x.CreatedBy).Include(x => x.User), cancellationToken);
        var instructor = instructors.FirstOrDefault();

        if (instructor is null)
            return Result.Failure<InstructorResponse>(InstructorErrors.InstructorNotFound);

        // Map the instructor to an InstructorResponse
        var instructorResponse = new InstructorResponse(
            InstructorId: instructor.InstructorId,
            InstructorName: instructor.User != null ? instructor.User.FirstName + " " + instructor.User.LastName : "Unknown",
            CreatedBy: instructor.CreatedBy.FirstName + " " + instructor.CreatedBy.LastName,
            CreatedOn: instructor.CreatedOn,
            Email: instructor.User?.Email ?? "No Email",
            IsActive: instructor.IsActive
        );

        return Result.Success(instructorResponse);
    }


    public async Task<Result<InstructorResponse>> CreateInstructorAsync(ApplicationUser user, InstructorRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            Result.Failure(InstructorErrors.InstructorNotFound);

        var Instructor = new Instructor()
        {
            User = user!,
            Expertise = request.expertise,
            biography = request.biography


        };
        await _unitOfWork.Repository<Instructor>().AddAsync(Instructor, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(Instructor.Adapt<InstructorResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var instructors = await _unitOfWork.Repository<Instructor>()
                                           .FindAsync(x => x.InstructorId == id, q => q.Include(x => x.CreatedBy).Include(x => x.User), cancellationToken);
        var instructor = instructors.FirstOrDefault();

        if (instructor is null)
            return Result.Failure(InstructorErrors.InstructorNotFound);

        instructor.IsActive = !instructor.IsActive;

        // Save changes to the database
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateInstructorAsync(Guid id, InstructorRequest request, CancellationToken cancellationToken = default)
    {
        var instructors = await _unitOfWork.Repository<Instructor>()
                                         .FindAsync(x => x.InstructorId == id,
                                         q => q.Include(x => x.CreatedBy).Include(x => x.User), cancellationToken);
        var instrcotr = instructors.FirstOrDefault();

        if (instrcotr.User is null)
            return Result.Failure<InstructorResponse>(InstructorErrors.InstructorNotFound);

        if (await _unitOfWork.Repository<Instructor>().AnyAsync(x => x.User.Email == request.Email && x.InstructorId != id, cancellationToken))
            return Result.Failure<InstructorResponse>(InstructorErrors.DuplicatedInstructor);

        // Update student details
        instrcotr.User.FirstName = request.FirstName;
        instrcotr.User.LastName = request.LastName;
        instrcotr.User.Email = request.Email;
        instrcotr.User.UserName = request.Email;
        instrcotr.biography = request.biography;
        instrcotr.Expertise = request.expertise;

        await _unitOfWork.Repository<Instructor>().UpdateAsync(instrcotr, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

   
}
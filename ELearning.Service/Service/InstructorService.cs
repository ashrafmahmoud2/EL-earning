﻿using ELearning.Service.IService;
using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;

using ELearning.Data.Errors;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Data.Contracts.Students;
using ELearning.Data.Contracts.Instrctors;
using ELearning.Data.Contracts.Comment;
using Mailjet.Client.Resources;
using System.Xml.Linq;
using ELearning.Data.Contracts.Enrollment;
namespace ELearning.Service.Service;

public class InstructorService : BaseRepository<Instructor>, IInstructorService
{


    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public InstructorService(ApplicationDbContext context, IUnitOfWork unitOfWork, ICacheService cacheService) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<IEnumerable<InstructorResponse>>> GetAllInstructorsAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "Instructors:GetAll";

        var cachedInstructors = await _cacheService.GetCacheAsync<IEnumerable<InstructorResponse>>(cacheKey);

        if (cachedInstructors.IsSuccess && cachedInstructors.Value != null)
            return Result.Success(cachedInstructors.Value);


        if (cachedInstructors.IsFailure && cachedInstructors.Error != CashErrors.NotFound)
            return Result.Failure<IEnumerable<InstructorResponse>>(cachedInstructors.Error);

        // Retrieve instructors from the database
        var instructors = await _unitOfWork.Repository<Instructor>()
            .FindAsync(
                x => x.IsActive,
                include: q => q.Include(s => s.CreatedBy)
                               .Include(s => s.User),
                cancellationToken: cancellationToken
            );

        // Adapt instructors to InstructorResponse
        var instructorResponses = instructors.Adapt<IEnumerable<InstructorResponse>>();

        // Cache the adapted response
        await _cacheService.SetCacheAsync(cacheKey, instructorResponses, _cacheDuration);

        return Result.Success(instructorResponses); 
    }


    public async Task<Result<InstructorResponse>> GetInstructorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var instructor = await _unitOfWork.Repository<Instructor>()
                                           .FirstOrDefaultAsync(x => x.InstructorId == id && x.IsActive,
                                                 q => q.Include(x => x.CreatedBy)
                                                 .Include(x => x.User),
                                   cancellationToken);

        if (instructor is null)
            return Result.Failure<InstructorResponse>(InstructorsErrors.InstructorNotFound);



        return Result.Success(instructor.Adapt<InstructorResponse>());
    }

    public async Task<Result<InstructorResponse>> CreateInstructorAsync(ApplicationUser user, InstructorRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == user.Id))
            return Result.Failure<InstructorResponse>(UserErrors.UserNotFound);

        if (await _unitOfWork.Repository<Instructor>().AnyAsync(x => x.User.Email == request.Email && x.IsActive, cancellationToken))
            return Result.Failure<InstructorResponse>(InstructorsErrors.DuplicatedInstructor);

        if (request is null)
            Result.Failure(InstructorsErrors.InstructorNotFound);

        var Instructor = new Instructor()
        {
            User = user!,
            Expertise = request.Expertise,
            Biography = request.Biography


        };
        await _unitOfWork.Repository<Instructor>().AddAsync(Instructor, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Instructors:GetAll");

        return Result.Success(Instructor.Adapt<InstructorResponse>());
    }

    public async Task<Result> UpdateInstructorAsync(Guid id, InstructorRequest request, CancellationToken cancellationToken = default)
    {


        var instructor = await _unitOfWork.Repository<Instructor>()
                                        .FirstOrDefaultAsync(x => x.InstructorId == id && x.IsActive,
                                              q => q.Include(x => x.CreatedBy)
                                              .Include(x => x.User),
                                cancellationToken);

        if (instructor.User is null)
            return Result.Failure<InstructorResponse>(InstructorsErrors.InstructorNotFound);

        if (await _unitOfWork.Repository<Instructor>().AnyAsync(x => x.User.Email == request.Email && x.InstructorId != id, cancellationToken))
            return Result.Failure<InstructorResponse>(InstructorsErrors.DuplicatedInstructor);

        // Update student details
        instructor.User.FirstName = request.FirstName;
        instructor.User.LastName = request.LastName;
        instructor.User.Email = request.Email;
        instructor.User.UserName = request.Email;
        instructor.Biography = request.Biography;
        instructor.Expertise = request.Expertise;

        await _unitOfWork.Repository<Instructor>().UpdateAsync(instructor, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Instructors:GetAll");

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var instructor = await _unitOfWork.Repository<Instructor>()
                                        .FirstOrDefaultAsync(x => x.InstructorId == id && x.IsActive,
                                              q => q.Include(x => x.CreatedBy)
                                              .Include(x => x.User),
                                cancellationToken);


        if (instructor is null)
            return Result.Failure(InstructorsErrors.InstructorNotFound);

        instructor.IsActive = !instructor.IsActive;

        // Save changes to the database
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Instructors:GetAll");

        return Result.Success();
    }



}
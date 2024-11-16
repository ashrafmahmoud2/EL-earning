﻿using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Contracts.Comment;
using ELearning.Data.Errors;
using System.Xml.Linq;
using ELearning.Data.Contracts.Answer;
using Azure;
namespace ELearning.Service.Service;

public class CommentService : BaseRepository<Comment>, ICommentService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CommentResponse>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<Comment>()
                                         .FirstOrDefaultAsync(x => x.CommentId == id,
                                         q => q.Include(x => x.ApplicationUser),
                                         cancellationToken);

        if (comment is null)
            return Result.Failure<CommentResponse>(CommentErrors.CommentNotFound);

        var CommentResponse = comment.Adapt<CommentResponse>();

        return Result.Success(CommentResponse);
    }

    public async Task<Result> CreateCommentAsync(CommentRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<CommentResponse>(LessonErrors.LessonNotFound);

        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == request.ApplicationUserID))
            return Result.Failure<CommentResponse>(UserErrors.UserNotFound);

        if (await _unitOfWork.Repository<Comment>().AnyAsync(x => x.LessonId == request.LessonId && x.ApplicationUser.Id == request.ApplicationUserID && x.Title == request.Title && x.CommentText == request.CommentText))
            return Result.Failure<CommentResponse>(CommentErrors.DuplicatedComment);

        if (request is null)
            Result.Failure(CommentErrors.CommentNotFound);


        var comment = request.Adapt<Comment>();
        comment.CommentedByUserId = request.ApplicationUserID;

        await _unitOfWork.Repository<Comment>().AddAsync(comment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<CommentResponse>>> GetAllCommentsAsync(CancellationToken cancellationToken = default)
    {


        var comments = await _unitOfWork.Repository<Comment>()
                                .FindAsync(s => true,
                                 q => q.Include(x => x.ApplicationUser),
                                  cancellationToken);

        return Result.Success(comments.Adapt<IEnumerable<CommentResponse>>());
    }

    public async Task<Result<CommentResponse>> UpdateCommentAsync(Guid CommentId, CommentRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<CommentResponse>(LessonErrors.LessonNotFound);

        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == request.ApplicationUserID))
            return Result.Failure<CommentResponse>(UserErrors.UserNotFound);



        var Comment = await _unitOfWork.Repository<Comment>()
                                         .FirstOrDefaultAsync(x => x.CommentId == CommentId,
                                         q => q.Include(x => x.ApplicationUser)
                                         ,cancellationToken);

        if (Comment is null)
            return Result.Failure<CommentResponse>(CommentErrors.CommentNotFound);


        Comment.IsEdited = true;
        Comment.Title = request.Title;
        Comment.CommentText = request.CommentText;
        Comment.UpdatedCommentedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Comment>().UpdateAsync(Comment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(Comment.Adapt<CommentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Comments = await _unitOfWork.Repository<Comment>()
                                           .FindAsync(x => x.CommentId == id);
        var Comment = Comments.FirstOrDefault();

        if (Comment is null)
            return Result.Failure(CommentErrors.CommentNotFound);

        Comment.IsActive = !Comment.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<int>> CountCommentsForLesson(Guid lessonId, CancellationToken cancellationToken = default)
    {
        var count = await _context.Comments
            .Where(x => x.LessonId == lessonId)
            .CountAsync();

        return Result.Success(count);
    }


}




 using ELearning.Infrastructure.Base;
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
using ELearning.Data.Abstractions;
using ELearning.Data.Contracts.Course;
using Microsoft.Extensions.Caching.Hybrid;
namespace ELearning.Service.Service;

public class CommentService : BaseRepository<Comment>, ICommentService
{
   
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly HybridCache _hybridCache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public CommentService(ApplicationDbContext context, IUnitOfWork unitOfWork, HybridCache hybridCache) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _hybridCache = hybridCache;
    }

    public async Task<Result<CommentResponse>> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<Comment>()
                                         .FirstOrDefaultAsync(x => x.CommentId == id && x.IsActive,
                                         q => q.Include(x => x.ApplicationUser),
                                         cancellationToken);

        if (comment is null)
            return Result.Failure<CommentResponse>(CommentsErrors.NotFound);

        var CommentResponse = comment.Adapt<CommentResponse>();

        return Result.Success(CommentResponse);
    }

    public async Task<Result<IEnumerable<CommentResponse>>> GetAllCommentsAsync(CancellationToken cancellationToken = default)
    {
        var cacheKey = "Comments:GetAll";

        // Check if data is in the cache
        var cachedComments = await _hybridCache.GetOrCreateAsync<IEnumerable<CommentResponse>>(
            cacheKey,

            async ct =>
            {
                // Retrieve comments from the database
                var comments = await _unitOfWork.Repository<Comment>()
                    .FindAsync(
                        s => s.IsActive,
                        query => query.Include(c => c.ApplicationUser),
                        ct // Pass the cancellation token to the FindAsync method
                    );

                // Adapt comments to CommentResponse
                var commentResponses = comments.Adapt<IEnumerable<CommentResponse>>();

                return commentResponses;
            },

            options: null,
            cancellationToken: cancellationToken
        );

        return Result.Success(cachedComments);
    }

    public async Task<Result> CreateCommentAsync(CommentRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<CommentResponse>(LessonsErrors.NotFound);

        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == request.commentedByUserId))
            return Result.Failure<CommentResponse>(UserErrors.UserNotFound);

        if (await _unitOfWork.Repository<Comment>().AnyAsync(x => x.LessonId == request.LessonId && x.ApplicationUser.Id == request.commentedByUserId && x.Title == request.Title && x.CommentText == request.CommentText))
            return Result.Failure<CommentResponse>(CommentsErrors.DuplicatedComment);

        if (request is null)
            Result.Failure(CommentsErrors.NotFound);


        var comment = request.Adapt<Comment>();
        comment.CommentedByUserId = request.commentedByUserId;

        await _unitOfWork.Repository<Comment>().AddAsync(comment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
       await _hybridCache.RemoveAsync("Comments:GetAll");


        return Result.Success();
    }

    public async Task<Result<CommentResponse>> UpdateCommentAsync(Guid commentId, CommentRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<CommentResponse>(LessonsErrors.NotFound);

        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == request.commentedByUserId))
            return Result.Failure<CommentResponse>(UserErrors.UserNotFound);



        var Comment = await _unitOfWork.Repository<Comment>()
                                         .FirstOrDefaultAsync(x => x.CommentId == commentId && x.IsActive,
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

        // Remove the cached 
        await _hybridCache.RemoveAsync("Comments:GetAll");

        return Result.Success(Comment.Adapt<CommentResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _unitOfWork.Repository<Comment>()
                                           .FirstOrDefaultAsync(x => x.CommentId == id && x.IsActive);

        if (comment is null)
            return Result.Failure(CommentsErrors.NotFound);

        comment.IsActive = !comment.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _hybridCache.RemoveAsync("Comments:GetAll");

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




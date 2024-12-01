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
namespace ELearning.Service.Service;

public class CommentService : BaseRepository<Comment>, ICommentService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public CommentService(ApplicationDbContext context, IUnitOfWork unitOfWork, ICacheService cacheService) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
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
        var cachedComments = await _cacheService.GetCacheAsync<IEnumerable<CommentResponse>>(cacheKey);

        if (cachedComments != null)
        {
            return Result.Success(cachedComments);
        }

        // Retrieve comments from the database
        var comments = await _unitOfWork.Repository<Comment>()
                            .FindAsync(
                                s => s.IsActive,
                                query => query.Include(c => c.ApplicationUser),
                                cancellationToken
                            );

        // Adapt comments to CommentResponse
        var commentResponses = comments.Adapt<IEnumerable<CommentResponse>>();

        // Cache the adapted response
        await _cacheService.SetCacheAsync(cacheKey, commentResponses, _cacheDuration);

        return Result.Success(commentResponses);
    }

    public async Task<Result> CreateCommentAsync(CommentRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<CommentResponse>(LessonsErrors.NotFound);

        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == request.ApplicationUserID))
            return Result.Failure<CommentResponse>(UserErrors.UserNotFound);

        if (await _unitOfWork.Repository<Comment>().AnyAsync(x => x.LessonId == request.LessonId && x.ApplicationUser.Id == request.ApplicationUserID && x.Title == request.Title && x.CommentText == request.CommentText))
            return Result.Failure<CommentResponse>(CommentsErrors.DuplicatedComment);

        if (request is null)
            Result.Failure(CommentsErrors.NotFound);


        var comment = request.Adapt<Comment>();
        comment.CommentedByUserId = request.ApplicationUserID;

        await _unitOfWork.Repository<Comment>().AddAsync(comment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Comments:GetAll");

        return Result.Success();
    }

    public async Task<Result<CommentResponse>> UpdateCommentAsync(Guid commentId, CommentRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<CommentResponse>(LessonsErrors.NotFound);

        if (!await _unitOfWork.Repository<ApplicationUser>().AnyAsync(x => x.Id == request.ApplicationUserID))
            return Result.Failure<CommentResponse>(UserErrors.UserNotFound);



        var Comment = await _unitOfWork.Repository<Comment>()
                                         .FirstOrDefaultAsync(x => x.CommentId == commentId && x.IsActive,
                                         q => q.Include(x => x.ApplicationUser)
                                         ,cancellationToken);

        if (Comment is null)
            return Result.Failure<CommentResponse>(CommentsErrors.NotFound);


        Comment.IsEdited = true;
        Comment.Title = request.Title;
        Comment.CommentText = request.CommentText;
        Comment.UpdatedCommentedAt = DateTime.UtcNow;

        await _unitOfWork.Repository<Comment>().UpdateAsync(Comment, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Remove the cached 
        await _cacheService.RemoveCacheAsync("Comments:GetAll");

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
        await _cacheService.RemoveCacheAsync("Comments:GetAll");

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




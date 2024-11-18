using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Contracts.Quiz;
using ELearning.Data.Errors;
using ELearning.Data.Contracts.Question;
namespace ELearning.Service.Service;

public class  QuizService : BaseRepository< Quiz>, IQuizService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public  QuizService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<QuizResponse>> GetQuizByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var  quiz = await _unitOfWork.Repository< Quiz>()
                                         .FirstOrDefaultAsync(x => x. QuizId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                               .Include(x => x.Lesson),
                                         cancellationToken);


        if (quiz is null)
            return Result.Failure<QuizResponse>(QuizsErrors.NotFound);

        var quizResponse = quiz.Adapt< QuizResponse>();

        return Result.Success(quizResponse);
    }

    public async Task<IEnumerable<QuizResponse>> GetAllQuizsAsync(CancellationToken cancellationToken = default)
    {
        var quizs = await _unitOfWork.Repository<Quiz>()
            .FindAsync(
                s => s.IsActive,
                 q => q.Include(x => x.CreatedBy)
                       .Include(x => x.Lesson),
                cancellationToken: cancellationToken);

        return quizs.Adapt<IEnumerable<QuizResponse>>();
    }

    public async Task<Result> CreateQuizAsync(QuizRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<QuizResponse>(LessonsErrors.NotFound);
        
        if (await _unitOfWork.Repository<Quiz>().AnyAsync(x => x.LessonId == request.LessonId && x.Title == request.Title))
            return Result.Failure<QuizResponse>(QuizsErrors.DuplicatedQuiz);


        if (request is null)
            Result.Failure( QuizsErrors. NotFound);


        var  Quiz = request.Adapt< Quiz>();
        await _unitOfWork.Repository< Quiz>().AddAsync( Quiz, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success( );
    }

    public async Task<Result< QuizResponse>> UpdateQuizAsync(Guid quizId,  QuizRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<QuizResponse>(LessonsErrors.NotFound);

        var quiz = await _unitOfWork.Repository<Quiz>()
                                        .FirstOrDefaultAsync(x => x.QuizId == quizId && x.IsActive,
                                        q => q.Include(x => x.CreatedBy)
                                              .Include(x => x.Lesson),
                                        cancellationToken);

        if (quiz is null)
            return Result.Failure< QuizResponse>( QuizsErrors. NotFound);


        quiz.Description = request.Description;
        quiz.Title = request.Title;
        quiz.LessonId = request.LessonId; 

          await _unitOfWork.Repository<Quiz>().UpdateAsync(quiz, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(quiz.Adapt<QuizResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var  quiz = await _unitOfWork.Repository< Quiz>()
                                           .FirstOrDefaultAsync(x => x. QuizId == id && x.IsActive);

        if (quiz is null)
            return Result.Failure( QuizsErrors. NotFound);

        quiz.IsActive= !quiz.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}




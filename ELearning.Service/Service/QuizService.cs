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
        var  Quizs = await _unitOfWork.Repository< Quiz>()
                                         .FindAsync(x => x. QuizId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var  Quiz =  Quizs.FirstOrDefault();

        if ( Quiz is null)
            return Result.Failure<QuizResponse>(QuizErrors.QuizNotFound);

        var  QuizResponse =  Quiz.Adapt< QuizResponse>();

        return Result.Success( QuizResponse);
    }

    public async Task<Result< QuizResponse>> CreateQuizAsync(QuizRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<QuizResponse>(LessonErrors.LessonNotFound);


        if (request is null)
            Result.Failure( QuizErrors. QuizNotFound);


        var  Quiz = request.Adapt< Quiz>();
        await _unitOfWork.Repository< Quiz>().AddAsync( Quiz, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success( Quiz.Adapt< QuizResponse>());
    }

    public async Task<IEnumerable<QuizResponse>> GetAllQuizsAsync(CancellationToken cancellationToken = default)
    {
        var quizs = await _unitOfWork.Repository<Quiz>()
            .FindAsync(
                s => true,
                cancellationToken: cancellationToken);

        // Corrected typo: Use Adapt instead of Adabt
        return quizs.Adapt<IEnumerable<QuizResponse>>();
    }

    public async Task<Result< QuizResponse>> UpdateQuizAsync(Guid quizId,  QuizRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Lesson>().AnyAsync(x => x.LessonId == request.LessonId))
            return Result.Failure<QuizResponse>(LessonErrors.LessonNotFound);


        var quiz = await _unitOfWork.Repository<Quiz>()
                                         .FirstOrDefaultAsync(x => x.QuizId == quizId,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (quiz is null)
            return Result.Failure< QuizResponse>( QuizErrors. QuizNotFound);


        quiz.Description = request.Description;
        quiz.Title = request.Title;
        quiz.LessonId = request.LessonId; 

          await _unitOfWork.Repository<Quiz>().UpdateAsync(quiz, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(quiz.Adapt<QuizResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var  Quizs = await _unitOfWork.Repository< Quiz>()
                                           .FindAsync(x => x. QuizId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var  Quiz =  Quizs.FirstOrDefault();

        if ( Quiz is null)
            return Result.Failure( QuizErrors. QuizNotFound);

         Quiz.IsActive= ! Quiz.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}




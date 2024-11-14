using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Contracts.QuizAttempt;
using ELearning.Data.Errors;
namespace ELearning.Service.Service;

public class QuizAttemptService : BaseRepository<QuizAttempt>, IQuizAttemptService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public QuizAttemptService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<QuizAttemptResponse>> GetQuizAttemptByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var QuizAttempts = await _unitOfWork.Repository<QuizAttempt>()
                                         .FindAsync(x => x.QuizAttemptId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var QuizAttempt = QuizAttempts.FirstOrDefault();

        if (QuizAttempt is null)
            return Result.Failure<QuizAttemptResponse>(QuizAttemptErrors.QuizAttemptNotFound);

        var QuizAttemptResponse = QuizAttempt.Adapt<QuizAttemptResponse>();

        return Result.Success(QuizAttemptResponse);
    }

    public async Task<Result<QuizAttemptResponse>> CreateQuizAttemptAsync(QuizAttemptRequest request, CancellationToken cancellationToken = default)
    {
        //START DAY With sentic commite in githup;
        //stop in make the Update and test conttroler


        if (request is null)
            return Result.Failure<QuizAttemptResponse>(QuizAttemptErrors.QuizAttemptNotFound);

        bool quizAttemptExists = await _unitOfWork.Repository<QuizAttempt>()
        .AnyAsync(x => x.StudentId == request.StudentId && x.QuizId == request.QuizId, cancellationToken);

        if (quizAttemptExists)
            return Result.Failure<QuizAttemptResponse>(QuizAttemptErrors.DuplicatedQuizAttempt);


        var quizAttempt = request.Adapt<QuizAttempt>();

        int totalQuestions = await _unitOfWork.Repository<Question>()
            .CountAsync(q => q.QuizId == request.QuizId, cancellationToken);

        int correctAnswers = 0;
        int incorrectAnswers = 0;

        // Check each answer's correctness
        foreach (var questionAnswer in request.QuestionAnswerResponse)
        {
            foreach (var answerId in questionAnswer.SelectedAnswersIds)
            {
                bool isCorrectAnswer = await _unitOfWork.Repository<Answer>()
                    .AnyAsync(a => a.AnswerId == answerId && a.QuestionId == questionAnswer.QuestionId && a.IsCorrect, cancellationToken);

                if (isCorrectAnswer)
                    correctAnswers++;
                else
                    incorrectAnswers++;
            }
        }


        quizAttempt.TotalQuestions = totalQuestions;
        quizAttempt.CorrectAnswersCount = correctAnswers;
        quizAttempt.IncorrectAnswersCount = incorrectAnswers;
        quizAttempt.NotAnswersQuestionsCount = Math.Max(0, totalQuestions - (correctAnswers + incorrectAnswers));
        quizAttempt.ScorePercentage = totalQuestions > 0 ? (correctAnswers * 100) / totalQuestions : 0;
        quizAttempt.HasPassed = quizAttempt.ScorePercentage >= 70;

        await _unitOfWork.Repository<QuizAttempt>().AddAsync(quizAttempt, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        var response = quizAttempt.Adapt<QuizAttemptResponse>();
        return Result.Success(response);
    }

    public async Task<IEnumerable<QuizAttemptResponse>> GetAllQuizAttemptsAsync(CancellationToken cancellationToken = default)
    {
        var QuizAttempts = await _unitOfWork.Repository<QuizAttempt>()
            .FindAsync(
                s => true,
                cancellationToken: cancellationToken);

        // Corrected typo: Use Adapt instead of Adabt
        return QuizAttempts.Adapt<IEnumerable<QuizAttemptResponse>>();
    }

    public async Task<Result<QuizAttemptResponse>> UpdateQuizAttemptAsync(Guid QuizAttemptId, QuizAttemptRequest request, CancellationToken cancellationToken = default)
    {

        var QuizAttempt = await _unitOfWork.Repository<QuizAttempt>()
                                         .FirstOrDefaultAsync(x => x.QuizAttemptId == QuizAttemptId,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (QuizAttempt is null)
            return Result.Failure<QuizAttemptResponse>(QuizAttemptErrors.QuizAttemptNotFound);


        //QuizAttempt.SelectedAnswers = request.se;

        await _unitOfWork.Repository<QuizAttempt>().UpdateAsync(QuizAttempt, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(QuizAttempt.Adapt<QuizAttemptResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var QuizAttempts = await _unitOfWork.Repository<QuizAttempt>()
                                           .FindAsync(x => x.QuizAttemptId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var QuizAttempt = QuizAttempts.FirstOrDefault();

        if (QuizAttempt is null)
            return Result.Failure(QuizAttemptErrors.QuizAttemptNotFound);

        QuizAttempt.IsActive = !QuizAttempt.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }


}




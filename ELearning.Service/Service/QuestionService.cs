using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Errors;
using static System.Collections.Specialized.BitVector32;
using ELearning.Data.Contracts.Question;
using ELearning.Data.Contracts.Payment;
namespace ELearning.Service.Service;

public class QuestionService : BaseRepository<Question>, IQuestionService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public QuestionService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<QuestionResponse>> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Questions = await _unitOfWork.Repository<Question>()
                                         .FindAsync(x => x.QuestionId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Question = Questions.FirstOrDefault();

        if (Question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

        var QuestionResponse = Question.Adapt<QuestionResponse>();

        return Result.Success(QuestionResponse);
    }

    public async Task<Result<QuestionResponse>> CreateQuestionAsync(QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Quiz>().AnyAsync(x => x.QuizId == request.QuizId))
            return Result.Failure<QuestionResponse>(QuizErrors.QuizNotFound);

        if (request is null)
            Result.Failure(QuestionErrors.QuestionNotFound);
        var question = request.Adapt<Question>();

        question.OrderIndex = await GetNextOrderIndexForQuestionAsync(question.QuizId, cancellationToken);

        await _unitOfWork.Repository<Question>().AddAsync(question, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(question.Adapt<QuestionResponse>());
    }

    public async Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(CancellationToken cancellationToken = default)
    {
        var Questions = await _unitOfWork.Repository<Question>()
            .FindAsync(
                s => true,
                cancellationToken: cancellationToken);

        // Corrected typo: Use Adapt instead of Adabt
        return Questions.Adapt<IEnumerable<QuestionResponse>>();
    }

    public async Task<Result<QuestionResponse>> UpdateQuestionAsync(Guid QuestionId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Quiz>().AnyAsync(x => x.QuizId == request.QuizId))
            return Result.Failure<QuestionResponse>(QuizErrors.QuizNotFound);


        var Question = await _unitOfWork.Repository<Question>()
                                         .FirstOrDefaultAsync(x => x.QuestionId == QuestionId,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (Question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);


        Question.Text = request.Text;
        Question.QuizId = request.QuizId;

        await _unitOfWork.Repository<Question>().UpdateAsync(Question, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(Question.Adapt<QuestionResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Questions = await _unitOfWork.Repository<Question>()
                                           .FindAsync(x => x.QuestionId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Question = Questions.FirstOrDefault();

        if (Question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        Question.IsActive = !Question.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<int> GetNextOrderIndexForQuestionAsync(Guid QuizId, CancellationToken cancellationToken)
    {
        var lastOrderIndex = await _unitOfWork.Repository<Question>()
            .Find(s => s.QuizId == QuizId)
            .MaxAsync(s => (int?)s.OrderIndex, cancellationToken);

        return (lastOrderIndex ?? 0) + 1;
    }
}




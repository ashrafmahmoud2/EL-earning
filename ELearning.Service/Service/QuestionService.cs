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
        var question = await _unitOfWork.Repository<Question>()
                                         .FirstOrDefaultAsync(x => x.QuestionId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                                .Include(x => x.Quiz)
                                                , cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionsErrors.NotFound);

        var questionResponse = question.Adapt<QuestionResponse>();

        return Result.Success(questionResponse);
    }

    public async Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(CancellationToken cancellationToken = default)
    {
        var questions = await _unitOfWork.Repository<Question>()
            .FindAsync(
                s => s.IsActive,
                q => q.Include(x => x.CreatedBy)
               .Include(x => x.Quiz)
               , cancellationToken);

        return questions.Adapt<IEnumerable<QuestionResponse>>();
    }

    public async Task<Result> CreateQuestionAsync(QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Quiz>().AnyAsync(x => x.QuizId == request.QuizId))
            return Result.Failure<QuestionResponse>(QuizsErrors.NotFound);

          if (!await _unitOfWork.Repository<Question>().AnyAsync(x => x.Text == request.Text))
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestion);


        if (request is null)
            Result.Failure(QuestionsErrors.NotFound);
        var question = request.Adapt<Question>();

        question.OrderIndex = await GetNextOrderIndexForQuestionAsync(question.QuizId, cancellationToken);

        await _unitOfWork.Repository<Question>().AddAsync(question, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<QuestionResponse>> UpdateQuestionAsync(Guid questionId, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Repository<Quiz>().AnyAsync(x => x.QuizId == request.QuizId))
            return Result.Failure<QuestionResponse>(QuizsErrors.NotFound);


        var question = await _unitOfWork.Repository<Question>()
                                        .FirstOrDefaultAsync(x => x.QuestionId == questionId && x.IsActive,
                                        q => q.Include(x => x.CreatedBy)
                                               .Include(x => x.Quiz)
                                               , cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionsErrors.NotFound);


        question.Text = request.Text;
        question.QuizId = request.QuizId;

        await _unitOfWork.Repository<Question>().UpdateAsync(question, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var question = await _unitOfWork.Repository<Question>()
                                         .FirstOrDefaultAsync(x => x.QuestionId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                                .Include(x => x.Quiz)
                                                , cancellationToken);

        if (question is null)
            return Result.Failure(QuestionsErrors.NotFound);

        question.IsActive = !question.IsActive;

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




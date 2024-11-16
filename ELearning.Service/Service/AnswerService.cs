using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Contracts.Answer;
using ELearning.Data.Errors;
namespace ELearning.Service.Service;

public class AnswerService : BaseRepository<Answer>, IAnswerService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AnswerService(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AnswerResponse>> GetAnswerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Answers = await _unitOfWork.Repository<Answer>()
                                         .FindAsync(x => x.AnswerId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Answer = Answers.FirstOrDefault();

        if (Answer is null)
            return Result.Failure<AnswerResponse>(AnswerErrors.AnswerNotFound);

        var AnswerResponse = Answer.Adapt<AnswerResponse>();

        return Result.Success(AnswerResponse);
    }

    public async Task<Result<AnswerResponse>> CreateAnswerAsync(AnswerRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Question>().AnyAsync(x => x.QuestionId == request.QuestionId))
            return Result.Failure<AnswerResponse>(QuestionErrors.QuestionNotFound);

        if (request is null)
            Result.Failure(AnswerErrors.AnswerNotFound);
        var Answer = request.Adapt<Answer>();



        await _unitOfWork.Repository<Answer>().AddAsync(Answer, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success(Answer.Adapt<AnswerResponse>());
    }

    public async Task<IEnumerable<AnswerResponse>> GetAllAnswersAsync(CancellationToken cancellationToken = default)
    {
        var Answers = await _unitOfWork.Repository<Answer>()
            .FindAsync(
                s => true,
                cancellationToken: cancellationToken);

        // Corrected typo: Use Adapt instead of Adabt
        return Answers.Adapt<IEnumerable<AnswerResponse>>();
    }

    public async Task<Result<AnswerResponse>> UpdateAnswerAsync(Guid AnswerId, AnswerRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Question>().AnyAsync(x => x.QuestionId == request.QuestionId))
            return Result.Failure<AnswerResponse>(QuestionErrors.QuestionNotFound);

        var Answer = await _unitOfWork.Repository<Answer>()
                                         .FirstOrDefaultAsync(x => x.AnswerId == AnswerId,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (Answer is null)
            return Result.Failure<AnswerResponse>(AnswerErrors.AnswerNotFound);




        // Check if updating the answer would leave the question with no correct answer
        if (request.IsCorrect == false)
        {
            bool hasCorrectAnswer = await _unitOfWork.Repository<Answer>()
                .AnyAsync(x => x.IsCorrect == true && x.QuestionId == request.QuestionId && x.AnswerId != AnswerId, cancellationToken);

            if (!hasCorrectAnswer)
            {
                return Result.Failure<AnswerResponse>(AnswerErrors.MissingCorrectAnswer);
            }
        }

        Answer.Text = request.Text;
        Answer.IsCorrect = request.IsCorrect;



        await _unitOfWork.Repository<Answer>().UpdateAsync(Answer, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(Answer.Adapt<AnswerResponse>());
    }

    public async Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Answers = await _unitOfWork.Repository<Answer>()
                                           .FindAsync(x => x.AnswerId == id, q => q.Include(x => x.CreatedBy), cancellationToken);
        var Answer = Answers.FirstOrDefault();

        if (Answer is null)
            return Result.Failure(AnswerErrors.AnswerNotFound);

        Answer.IsActive = !Answer.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }


}




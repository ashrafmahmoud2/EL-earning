using ELearning.Infrastructure.Base;
using ELearning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Infrastructure;
using Mapster;
using ELearning.Service.IService;
using ELearning.Data.Contracts.Answer;
using ELearning.Data.Errors;
using Azure.Core;
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
        var answer = await _unitOfWork.Repository<Answer>()
                                         .FirstOrDefaultAsync(x => x.AnswerId == id && x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                               .Include(x => x.Question)
                                         , cancellationToken);

        if (answer is null)
            return Result.Failure<AnswerResponse>(AnswersErrors.NotFound);

        var AnswerResponse = answer.Adapt<AnswerResponse>();

        return Result.Success(AnswerResponse);
    }

    public async Task<IEnumerable<AnswerResponse>> GetAllAnswersAsync(CancellationToken cancellationToken = default)
    {
        var Answers = await _unitOfWork.Repository<Answer>()
                                         .FindAsync(x => x.IsActive,
                                         q => q.Include(x => x.CreatedBy)
                                               .Include(x => x.Question)
                                         , cancellationToken);

        return Answers.Adapt<IEnumerable<AnswerResponse>>();
    }

    public async Task<Result> CreateAnswerAsync(AnswerRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Question>().AnyAsync(x => x.QuestionId == request.QuestionId))
            return Result.Failure<AnswerResponse>(QuestionsErrors.NotFound);

        if (await _unitOfWork.Repository<Answer>().AnyAsync(x => x.Text == request.Text && x.IsActive))
            return Result.Failure<AnswerResponse>(AnswerErrors.DuplicatedAnswer);

        if (request is null)
            Result.Failure(AnswersErrors.NotFound);
        var Answer = request.Adapt<Answer>();



        await _unitOfWork.Repository<Answer>().AddAsync(Answer, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<AnswerResponse>> UpdateAnswerAsync(Guid AnswerId, AnswerRequest request, CancellationToken cancellationToken = default)
    {

        if (!await _unitOfWork.Repository<Question>().AnyAsync(x => x.QuestionId == request.QuestionId))
            return Result.Failure<AnswerResponse>(QuestionsErrors.NotFound);

        var Answer = await _unitOfWork.Repository<Answer>()
                                         .FirstOrDefaultAsync(x => x.AnswerId == AnswerId && x.IsActive,
                                         q => q.Include(x => x.CreatedBy), cancellationToken);

        if (Answer is null)
            return Result.Failure<AnswerResponse>(AnswersErrors.NotFound);




        // Check if updating the answer would leave the question with no correct answer
        if (request.IsCorrect == false)
        {
            bool hasCorrectAnswer = await _unitOfWork.Repository<Answer>()
                .AnyAsync(x => x.IsCorrect == true && x.QuestionId == request.QuestionId && x.AnswerId != AnswerId, cancellationToken);

            if (!hasCorrectAnswer)
            {
                return Result.Failure<AnswerResponse>(AnswersErrors.MissingCorrectAnswer);
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
                                           .FindAsync(x => x.AnswerId == id && x.IsActive);
        var Answer = Answers.FirstOrDefault();

        if (Answer is null)
            return Result.Failure(AnswersErrors.NotFound);

        Answer.IsActive = !Answer.IsActive;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
  
    public async Task<Result> DeleteAnswerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var answer = await _unitOfWork.Repository<Answer>()
                                           .FirstOrDefaultAsync(x => x.AnswerId == id && x.IsActive);


        if (answer.IsCorrect)
        {
            bool hasCorrectAnswer = await _unitOfWork.Repository<Answer>()
                .AnyAsync(x => x.IsCorrect == true && x.QuestionId == answer.QuestionId && x.AnswerId != answer.AnswerId, cancellationToken);

            if (!hasCorrectAnswer)
            {
                return Result.Failure<AnswerResponse>(AnswersErrors.MissingCorrectAnswer);
            }
        }

        if (answer is null)
            return Result.Failure(AnswersErrors.NotFound);

       await _unitOfWork.Repository<Answer>().RemoveAsync(answer);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }


}




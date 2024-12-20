﻿using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Contracts.Question;

namespace ELearning.Service.IService;

public interface IQuestionService
{
    Task<Result<QuestionResponse>> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(CancellationToken cancellationToken = default);
    Task<Result> CreateQuestionAsync(QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> UpdateQuestionAsync(Guid questionId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(Guid id, CancellationToken cancellationToken = default);
}


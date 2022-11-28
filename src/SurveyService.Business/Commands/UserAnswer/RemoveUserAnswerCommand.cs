using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer;

public class RemoveUserAnswerCommand : IRemoveUserAnswerCommand
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IQuestionRepository _questionRepository;
  private readonly IUserAnswerRepository _userAnswerRepository;
  private readonly IResponseCreator _responseCreator;

  public RemoveUserAnswerCommand(
    IHttpContextAccessor httpContextAccessor,
    IQuestionRepository questionRepository,
    IUserAnswerRepository userAnswerRepository,
    IResponseCreator responseCreator)
  {
    _httpContextAccessor = httpContextAccessor;
    _questionRepository = questionRepository;
    _userAnswerRepository = userAnswerRepository;
    _responseCreator = responseCreator;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(RemoveUserAnswerRequest request)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();
    DbQuestion dbQuestion = await _questionRepository.GetQuestionWithAnswersAsync(request.questionId);

    if (dbQuestion is null)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.NotFound,
        new List<string> { "Question not found." });
    }

    DbUserAnswer userAnswer = dbQuestion.Options
      .Select(x => x.UsersAnswers.Where(y => y.Id == request.userAnswerId).FirstOrDefault())
        .FirstOrDefault();

    if (userAnswer is null)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.NotFound,
        new List<string> { "Answer not found." });
    }

    if (senderId != dbQuestion.CreatedBy && senderId != userAnswer.UserId)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.Forbidden,
        new List<string> { "You have no rights to remove this answer." });
    }

    return new OperationResultResponse<bool>(body: await _userAnswerRepository.RemoveAsync(userAnswer));
  }
}


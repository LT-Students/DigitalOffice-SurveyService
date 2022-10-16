using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question;

public class GetQuestionCommand : IGetQuestionCommand
{
  private readonly IQuestionRepository _repository;
  private readonly IQuestionResponseMapper _questionResponseMapper;
  private readonly IResponseCreator _responseCreator;
  private readonly IUserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public GetQuestionCommand(
    IHttpContextAccessor httpContextAccessor,
    IQuestionRepository questionRepository, 
    IQuestionResponseMapper questionResponseMapper,
    IResponseCreator responseCreator,
    IUserService userService)
  {
    _httpContextAccessor = httpContextAccessor;
    _repository = questionRepository;
    _questionResponseMapper = questionResponseMapper;
    _responseCreator = responseCreator;
    _userService = userService;
  }
  
  public async Task<OperationResultResponse<QuestionResponse>> ExecuteAsync(GetQuestionFilter filter)
  {
    OperationResultResponse<QuestionResponse> response = new();
    if (filter is null)
    {
      return _responseCreator.CreateFailureResponse<QuestionResponse>(
        HttpStatusCode.BadRequest,
        new List<string> { "You must enter 'questionid'." });
    }

    DbQuestion dbQuestion = await _repository.GetAsync(filter);
    
    if (dbQuestion.Deadline > DateTime.Now && !dbQuestion.HasRealTimeResult)
    {
      return _responseCreator.CreateFailureResponse<QuestionResponse>(
        HttpStatusCode.Forbidden,
        new List<string> { "Question has no real-time results, result can be reached only after deadline." });
    }

    if (dbQuestion.IsPrivate && _httpContextAccessor.HttpContext.GetUserId() != dbQuestion.CreatedBy)
    {
      return _responseCreator.CreateFailureResponse<QuestionResponse>(
        HttpStatusCode.Forbidden,
        new List<string> { "Question is private, only author can view the results." });
    }
    
    if (dbQuestion.IsAnonymous && filter.IncludeUserInfo)
    {
      filter.IncludeUserInfo = false;
      response.Errors.Add("Personal information about user can't be loaded as question is anonymous.");
    }

    if (filter.IncludeOptions)
    {
      List<Tuple<DbOption, List<Tuple<DbUserAnswer, UserData>>>> optionsInfo = new();
      List<string> totalErrors = new ();
      foreach (DbOption option in dbQuestion.Options)
      {
        List<Tuple<DbUserAnswer, UserData>> usersInfo = new();

        foreach (DbUserAnswer optionUsersAnswer in option.UsersAnswers)
        {
          List<string> errors = new();
          UserData userData = filter.IncludeUserInfo
            ? (await _userService.GetUsersDataAsync(new List<Guid> { optionUsersAnswer.UserId }, errors)).FirstOrDefault()
            : null;
          usersInfo.Add(Tuple.Create(optionUsersAnswer, userData));
          totalErrors.AddRange(errors);
        }

        optionsInfo.Add(Tuple.Create(option,usersInfo));
      }

      response.Body = _questionResponseMapper.Map(dbQuestion, optionsInfo);
      response.Errors.AddRange(totalErrors);
    }
    else
    {
      response.Body = _questionResponseMapper.Map(dbQuestion);
    }

    return response;
  }
}

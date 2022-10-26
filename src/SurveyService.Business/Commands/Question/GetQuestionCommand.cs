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

    DbQuestion dbQuestion = await _repository.GetAsync(filter);

    if (dbQuestion is null)
    {
      return _responseCreator.CreateFailureResponse<QuestionResponse>(
        HttpStatusCode.NotFound,
        new List<string> {"Question with specified id wasn't found"});
    }
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
      response.Errors.Add("Personal information about users can't be loaded as question is anonymous.");
    }
    
    var users = dbQuestion.Options.SelectMany(x => x.UsersAnswers.Select(y => y.UserId))
      .Distinct()
      .ToList();
    List<string> errors = new();
    List<UserData> usersData = filter.IncludeUserInfo
      ? await _userService.GetUsersDataAsync(users, errors)
      : null;
    
    if (errors.Count != 0)
    {
      return _responseCreator.CreateFailureResponse<QuestionResponse>(
        HttpStatusCode.BadRequest,
        errors);
    }
    
    response.Body = _questionResponseMapper.Map(dbQuestion, usersData);

    return response;
  }
}

using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
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
  private readonly IOptionInfoMapper _optionInfoMapper;
  private readonly IUserAnswerInfoMapper _userAnswerInfoMapper;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public GetQuestionCommand(
    IHttpContextAccessor httpContextAccessor,
    IQuestionRepository questionRepository, 
    IQuestionResponseMapper questionResponseMapper,
    IResponseCreator responseCreator,
    IUserService userService,
    IOptionInfoMapper optionInfoMapper,
    IUserAnswerInfoMapper userAnswerInfoMapper)
  {
    _httpContextAccessor = httpContextAccessor;
    _repository = questionRepository;
    _questionResponseMapper = questionResponseMapper;
    _responseCreator = responseCreator;
    _userService = userService;
    _optionInfoMapper = optionInfoMapper;
    _userAnswerInfoMapper = userAnswerInfoMapper;
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

    List<OptionInfo> optionsInfo = new();

    foreach (DbOption option in dbQuestion.Options)
    {
      List<UserAnswerInfo> userAnswersInfo = new();
      
      foreach (DbUserAnswer optionUsersAnswer in option.UsersAnswers)
      {
        UserData userData = filter.IncludeUserInfo 
          ? (await _userService.GetUsersDataAsync(new List<Guid> { optionUsersAnswer.UserId }, null)).FirstOrDefault()
          : null;
        UserAnswerInfo userAnswerInfo = _userAnswerInfoMapper.Map(optionUsersAnswer, userData);
        userAnswersInfo.Add(userAnswerInfo);
      }

      OptionInfo optionInfo = _optionInfoMapper.Map(option, userAnswersInfo);
      optionsInfo.Add(optionInfo);
    }
    
    response.Body = _questionResponseMapper.Map(dbQuestion, optionsInfo);

    return response;
  }
}

using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
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
  private readonly IUserInfoMapper _userInfoMapper;
  private readonly IUserAnswerInfoMapper _userAnswerInfoMapper;

  public GetQuestionCommand(
    IQuestionRepository questionRepository, 
    IQuestionResponseMapper questionResponseMapper,
    IResponseCreator responseCreator,
    IUserService userService,
    IOptionInfoMapper optionInfoMapper,
    IUserInfoMapper userInfoMapper,
    IUserAnswerInfoMapper userAnswerInfoMapper)
  {
    _repository = questionRepository;
    _questionResponseMapper = questionResponseMapper;
    _responseCreator = responseCreator;
    _userService = userService;
    _optionInfoMapper = optionInfoMapper;
    _userInfoMapper = userInfoMapper;
    _userAnswerInfoMapper = userAnswerInfoMapper;
  }
  
  public async Task<OperationResultResponse<QuestionResponse>> ExecuteAsync(GetQuestionFilter filter)
  {
    OperationResultResponse<QuestionResponse> response = new();

    if (filter is null || filter.QuestionId is null)
    {
      return _responseCreator.CreateFailureResponse<QuestionResponse>(
        HttpStatusCode.BadRequest,
        new List<string> { "You must enter 'questionid'" });
    }

    DbQuestion dbQuestion = await _repository.GetAsync(filter);
    List<OptionInfo> optionInfos = new();
    foreach (DbOption option in dbQuestion.Options)
    {
      List<UserAnswerInfo> userAnswerInfos = new();
      foreach (DbUserAnswer optionUsersAnswer in option.UsersAnswers)
      {
        UserData userData = filter.IncludeUserInfo 
          ? (await _userService.GetUsersDataAsync(new List<Guid> { optionUsersAnswer.UserId }, null)).FirstOrDefault()
          : null;
        UserAnswerInfo userAnswerInfo = _userAnswerInfoMapper.Map(optionUsersAnswer, userData);
        userAnswerInfos.Add(userAnswerInfo);
      }

      OptionInfo optionInfo = _optionInfoMapper.Map(option, userAnswerInfos);
      optionInfos.Add(optionInfo);
    }

    response.Body = _questionResponseMapper.Map(dbQuestion, optionInfos);
    
    return response;
  }
}

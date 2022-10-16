using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses;

public class QuestionResponseMapper : IQuestionResponseMapper
{
  private readonly IOptionInfoMapper _optionInfoMapper;
  private readonly IUserAnswerInfoMapper _userAnswerInfoMapper;

  public QuestionResponseMapper(
    IOptionInfoMapper optionInfoMapper,
    IUserAnswerInfoMapper userAnswerInfoMapper)
  {
    _optionInfoMapper = optionInfoMapper;
    _userAnswerInfoMapper = userAnswerInfoMapper;
  }
  
  public QuestionResponse Map(DbQuestion dbQuestion, List<Tuple<DbOption, List<Tuple<DbUserAnswer, UserData>>>> optionInfos = null)
  {
    return dbQuestion is null
      ? null
      : new QuestionResponse
      {
        Id = dbQuestion.Id,
        Content = dbQuestion.Content,
        Deadline = dbQuestion.Deadline,
        HasRealTimeResult = dbQuestion.HasRealTimeResult,
        IsAnonymous = dbQuestion.IsAnonymous,
        IsRevoteAvailable = dbQuestion.IsRevoteAvailable,
        IsObligatory = dbQuestion.IsObligatory,
        IsPrivate = dbQuestion.IsPrivate,
        HasMultipleChoice = dbQuestion.HasMultipleChoice,
        HasCustomOptions = dbQuestion.HasCustomOptions,
        IsActive = dbQuestion.IsActive,
        Options = optionInfos?
          .Select(x =>
            _optionInfoMapper.Map(x.Item1, (x.Item2)
              .Select(y => 
                _userAnswerInfoMapper.Map(y.Item1, y.Item2))
              .ToList()))
          .ToList()
      };
  }
}

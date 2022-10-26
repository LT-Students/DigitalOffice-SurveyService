using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class QuestionInfoMapper : IQuestionInfoMapper
{
  private readonly IOptionInfoMapper _optionInfoMapper;

  public QuestionInfoMapper(IOptionInfoMapper optionInfoMapper)
  {
    _optionInfoMapper = optionInfoMapper;
  }

  public QuestionInfo Map(DbQuestion dbQuestion, List<UserData> usersData)
  {
    return dbQuestion is null
      ? null
      : new QuestionInfo
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
        Options = dbQuestion.Options?.Select(o => _optionInfoMapper.Map(o, usersData)).ToList()
      };
  }
}

using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class QuestionInfoMapper : IQuestionInfoMapper
{
  public QuestionInfo Map(DbQuestion dbQuestion, List<OptionInfo> optionInfos)
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
        Options = optionInfos
      };
  }
}

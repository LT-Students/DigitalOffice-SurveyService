using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses;

public class QuestionResponseMapper : IQuestionResponseMapper
{
  public QuestionResponse Map(DbQuestion dbQuestion, List<OptionInfo> optionInfos)
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
        Options = optionInfos
      };
  }
}

using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses;

public class QuestionResponseMapper : IQuestionResponseMapper
{
  private readonly IOptionInfoMapper _mapper;
  public QuestionResponseMapper(
    IOptionInfoMapper optionInfoMapper
    )
  {
    _mapper = optionInfoMapper;
  }
  public QuestionResponse Map(DbQuestion dbQuestion)
  {
    return dbQuestion is null
      ? null
      : new QuestionResponse
      {
        Id = dbQuestion.Id,
        GroupId = dbQuestion.GroupId,
        Content = dbQuestion.Content,
        Deadline = dbQuestion.Deadline,
        HasRealTimeResult = dbQuestion.HasRealTimeResult,
        IsAnonymous = dbQuestion.IsAnonymous,
        IsRevoteAvailable = dbQuestion.IsRevoteAvailable,
        IsObligatory = dbQuestion.IsObligatory,
        IsPrivate = dbQuestion.IsPrivate,
        HasMultipleChoice = dbQuestion.HasMultipleChoice,
        HasCustomOptions = dbQuestion.HasCustomOptions,
        Options = dbQuestion.Options.Select(_mapper.Map).ToList()
      };
  }
}

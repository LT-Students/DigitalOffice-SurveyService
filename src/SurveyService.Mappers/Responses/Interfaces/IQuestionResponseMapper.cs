using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;

public interface IQuestionResponseMapper
{
  QuestionResponse Map(DbQuestion dbQuestion);
}

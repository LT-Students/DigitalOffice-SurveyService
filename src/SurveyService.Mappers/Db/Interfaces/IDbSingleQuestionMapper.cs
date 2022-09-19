using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;

namespace LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbSingleQuestionMapper
{
  DbQuestion Map(CreateSingleQuestionRequest request);
}

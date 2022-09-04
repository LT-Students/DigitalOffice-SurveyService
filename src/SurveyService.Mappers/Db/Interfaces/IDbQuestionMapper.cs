using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbQuestionMapper
{
  DbQuestion Map(CreateQuestionRequest createQuestionRequest, Guid? groupId);
}

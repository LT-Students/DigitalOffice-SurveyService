using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;

public interface IDbQuestionMapper
{
  DbQuestion Map(CreateQuestionRequest dbQuestionRequest, Guid? groupId);
}
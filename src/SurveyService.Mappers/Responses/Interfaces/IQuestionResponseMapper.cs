using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;

[AutoInject]
public interface IQuestionResponseMapper
{
  QuestionResponse Map(DbQuestion dbQuestion, List<UserData> usersData = null);
}

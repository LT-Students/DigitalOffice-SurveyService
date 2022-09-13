using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbUserAnswerMapper
{
  DbUserAnswer Map(Guid id);
}

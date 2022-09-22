using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbOptionMapper
{
  DbOption Map(CreateOptionRequest request);
  DbOption Map(CreateQuestionOptionRequest request, Guid questionId);
}

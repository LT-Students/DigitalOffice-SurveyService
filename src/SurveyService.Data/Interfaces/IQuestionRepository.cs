using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IQuestionRepository
{
  Task<Guid?> CreateAsync(DbQuestion dbQuestion);

  Task<DbQuestion> GetPropertiesAsync(GetQuestionPropertiesFilter filter);
}
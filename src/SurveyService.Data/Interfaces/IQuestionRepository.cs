using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IQuestionRepository
{
  Task<Guid?> CreateAsync(DbQuestion dbQuestion);

  Task<DbQuestion> GetAsync(Guid questionId);

  Task<bool> DoesExistAsync(Guid questionId);
}

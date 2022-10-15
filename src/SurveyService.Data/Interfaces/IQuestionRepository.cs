using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IQuestionRepository
{
  Task<Guid?> CreateAsync(DbQuestion dbQuestion);

  Task<bool> CheckGroupProperties(Guid groupId, DateTime? deadline, bool hasRealTimeResult);

  Task<DbQuestion> GetAsync(Guid questionId);

  Task<bool> DoesExistAsync(Guid questionId);

  Task<(List<DbQuestion>, int totalCount)> FindByAuthorAsync(FindByAuthorFilter filter);
}

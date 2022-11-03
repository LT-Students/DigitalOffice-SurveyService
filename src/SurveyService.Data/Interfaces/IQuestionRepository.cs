using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using Microsoft.AspNetCore.JsonPatch;
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

  Task<DbQuestion> GetAsync(GetQuestionFilter filter);

  Task<bool> DoesExistAsync(Guid questionId);

  Task DisactivateAsync(ICollection<DbQuestion> questions);

  Task<(List<DbQuestion>, int totalCount)> FindByAuthorAsync(FindQuestionsFilter filter, Guid authorId);

  Task<DbQuestion> GetQuestionWithAnswersAsync(Guid questionId);

  Task<bool> EditAsync(JsonPatchDocument<DbQuestion> patch, DbQuestion question);
}

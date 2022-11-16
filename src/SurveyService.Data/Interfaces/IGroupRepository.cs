using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IGroupRepository
{
  Task<Guid?> CreateAsync(DbGroup dbGroup);

  Task<DbGroup> GetAsync(GetGroupFilter filter);

  Task<DbGroup> GetAsync(Guid groupId);

  Task<(List<DbGroup>, int totalCount)> FindByAuthorAsync(FindQuestionsFilter filter, Guid authorId);

  Task<bool> EditAsync(JsonPatchDocument<DbGroup> patch, DbGroup dbGroup, Guid modifiedBy);
}

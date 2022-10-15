using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IGroupRepository
{
  Task<Guid?> CreateAsync(DbGroup dbGroup);

  Task<(List<DbGroup>, int totalCount)> FindByAuthorAsync(FindByAuthorFilter filter);
}

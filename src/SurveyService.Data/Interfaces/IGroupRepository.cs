using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IGroupRepository
{
  Task<Guid?> CreateAsync(DbGroup dbGroup);

  Task<DbGroup> GetAsync(GetGroupFilter filter);
}

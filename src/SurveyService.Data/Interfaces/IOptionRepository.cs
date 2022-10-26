using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IOptionRepository
{
  Task<Guid?> CreateAsync(DbOption dbOption);
  Task<List<DbOption>> GetByIdsAsync(List<Guid> optionIds);
  Task<bool> EditOptionsActivityAsync(ICollection<DbOption> options);
}

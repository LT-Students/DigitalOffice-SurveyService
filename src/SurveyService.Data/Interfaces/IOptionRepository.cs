using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IOptionRepository
{
  Task<Guid?> CreateAsync(DbOption dbOption);

  Task<DbOption> GetAsync(Guid optionId);
  
  Task<List<DbOption>> GetByIdsAsync(List<Guid> optionIds);
  
  Task DisactivateAsync(ICollection<DbOption> options);

  Task<bool> EditAsync(JsonPatchDocument<DbOption> patch, DbOption dbOption, Guid? modifiedBy = null);
}

using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Data.Provider.MsSql.Ef.Models;
using LT.DigitalOffice.SurveyService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

[AutoInject]
public interface IOptionRepository
{
  Task<Guid?> CreateAsync(DbOption dbOption);
}

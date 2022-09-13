using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;

public class OptionRepository : IOptionRepository
{
  public Task<Guid?> CreateAsync(DbOption dbOption)
  {
    throw new NotImplementedException();
  }
}
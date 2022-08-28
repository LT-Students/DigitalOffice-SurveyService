using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;
using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.SurveyService.Data.Provider
{
  [AutoInject(InjectType.Scoped)]
  public interface IDataProvider : IBaseDataProvider
  {
    DbSet<DbQuestion> Questions { get; set; }
  }
}

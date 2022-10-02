using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;

namespace LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;

[AutoInject]
public interface IOptionInfoMapper
{
  OptionInfo Map(DbOption dbOption);
}

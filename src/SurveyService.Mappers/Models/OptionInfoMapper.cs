using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class OptionInfoMapper: IOptionInfoMapper
{
  private readonly IUserAnswerInfoMapper _mapper;
  
  public OptionInfoMapper(
    IUserAnswerInfoMapper userAnswerInfoMapper)
  {
    _mapper = userAnswerInfoMapper;
  }
  
  public OptionInfo Map(DbOption dbOption)
  {
    return dbOption is null
      ? null
      : new OptionInfo
      {
        Id = dbOption.Id,
        Content = dbOption.Content,
        IsCustom = dbOption.IsCustom,
        UsersAnswers = dbOption.UsersAnswers.Select(_mapper.Map).ToList()
      };
  }
}

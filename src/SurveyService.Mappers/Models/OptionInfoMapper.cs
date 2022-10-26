using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Models;

public class OptionInfoMapper: IOptionInfoMapper
{
  private readonly IUserAnswerInfoMapper _userAnswerInfoMapper;

  public OptionInfoMapper(IUserAnswerInfoMapper userAnswerInfoMapper)
  {
    _userAnswerInfoMapper = userAnswerInfoMapper;
  }

  public OptionInfo Map(DbOption dbOption, List<UserData> usersData)
  {
    return dbOption is null
      ? null
      : new OptionInfo
      {
        Id = dbOption.Id,
        Content = dbOption.Content,
        IsCustom = dbOption.IsCustom,
        UsersAnswers = dbOption.UsersAnswers?.Select(ua => _userAnswerInfoMapper.Map(ua, usersData)).ToList()
      };
  }
}

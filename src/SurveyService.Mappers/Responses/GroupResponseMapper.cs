using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Group;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses;

public class GroupResponseMapper : IGroupResponseMapper
{
  private readonly IQuestionInfoMapper _questionInfoMapper;

  public GroupResponseMapper(IQuestionInfoMapper questionInfoMapper)
  {
    _questionInfoMapper = questionInfoMapper;
  }

  public GroupResponse Map(DbGroup dbGroup, List<UserData> usersData)
  {
    return dbGroup is null
      ? null
      : new GroupResponse
      {
        Id = dbGroup.Id,
        Subject = dbGroup.Subject,
        Description = dbGroup.Description,
        IsActive = dbGroup.IsActive,
        Questions = dbGroup.Questions.Select(q => _questionInfoMapper.Map(q, usersData)).ToList()
      };
  }
}

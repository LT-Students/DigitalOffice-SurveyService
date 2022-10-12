using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Group;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Mappers.Responses;

public class QuestionResponseMapper : IGroupResponseMapper
{
  public GroupResponse Map(DbGroup dbGroup, List<QuestionInfo> questionInfos)
  {
    return dbGroup is null
      ? null
      : new GroupResponse
      {
        Id = dbGroup.Id,
        Subject = dbGroup.Subject,
        Description = dbGroup.Description,
        isActive = dbGroup.IsActive,
        Questions = questionInfos
      };
  }
}
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Mvc;
using System;
namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbGroupQuestionMapper : IDbGroupQuestionMapper
{
  private IDbSingleQuestionMapper _dbSingleQuestionMapper;

  public DbGroupQuestionMapper(
    [FromServices] IDbSingleQuestionMapper dbSingleQuestionMapper)
  {
    _dbSingleQuestionMapper = dbSingleQuestionMapper;
  }
  
  public DbQuestion Map(CreateGroupQuestionRequest request, Guid groupId, DateTime? groupDeadline, bool groupHasRealTimeResult)
  {
    return request is null
      ? null
      : _dbSingleQuestionMapper.Map(
        new CreateSingleQuestionRequest 
        {
          GroupId = groupId,
          Content = request.Content,
          Deadline = groupDeadline,
          HasRealTimeResult = groupHasRealTimeResult,
          IsAnonymous = request.IsAnonymous,
          IsRevoteAvaible = request.IsRevoteAvaible,
          IsObligatory = request.IsObligatory,
          IsPrivate = request.IsPrivate,
          HasMultipleChoice = request.HasMultipleChoice,
          HasCustomOptions = request.HasCustomOptions,
          Options = request.Options
        });
  }
}

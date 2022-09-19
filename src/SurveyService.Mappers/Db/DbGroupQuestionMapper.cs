using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbGroupQuestionMapper : IDbGroupQuestionMapper
{
  private IHttpContextAccessor _httpContextAccessor;
  private IDbQuestionOptionMapper _dbQuestionOptionsMapper;
  private IDbSingleQuestionMapper _dbSingleQuestionMapper;

  public DbGroupQuestionMapper(
    [FromServices] IHttpContextAccessor httpContextAccessor,
    [FromServices] IDbQuestionOptionMapper dbQuestionOptionMapper,
    [FromServices] IDbSingleQuestionMapper dbSingleQuestionMapper)
  {
    _httpContextAccessor = httpContextAccessor;
    _dbQuestionOptionsMapper = dbQuestionOptionMapper;
    _dbSingleQuestionMapper = dbSingleQuestionMapper;
  }
  
  public DbQuestion Map(CreateGroupQuestionRequest request, Guid groupId, DateTime? groupDeadline, bool groupHasRealTimeResult)
  {
    Guid questionId = Guid.NewGuid();
    
    var t = new CreateSingleQuestionRequest {
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
    };
    return request is null
      ? null
      : _dbSingleQuestionMapper.Map(t);
  }
  
}

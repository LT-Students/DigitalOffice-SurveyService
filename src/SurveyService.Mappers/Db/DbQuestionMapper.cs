using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbQuestionMapper : IDbQuestionMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IDbOptionMapper _dbOptionMapper;

  public DbQuestionMapper(
    IHttpContextAccessor httpContextAccessor, 
    IDbOptionMapper dbOptionMapper)
  {
    _httpContextAccessor = httpContextAccessor;
    _dbOptionMapper = dbOptionMapper;
  }

  public DbQuestion Map(CreateGroupQuestionRequest request, Guid groupId, DateTime? groupDeadline, bool groupHasRealTimeResult)
  {
    return request is null
      ? null
      : Map(
        new CreateSingleQuestionRequest 
        {
          GroupId = groupId,
          Content = request.Content,
          Deadline = groupDeadline,
          HasRealTimeResult = groupHasRealTimeResult,
          IsAnonymous = request.IsAnonymous,
          IsRevoteAvailable = request.IsRevoteAvailable,
          IsObligatory = request.IsObligatory,
          IsPrivate = request.IsPrivate,
          HasMultipleChoice = request.HasMultipleChoice,
          HasCustomOptions = request.HasCustomOptions,
          Options = request.Options
        });
  }
  
  public DbQuestion Map(CreateSingleQuestionRequest request)
  {
    Guid questionId = Guid.NewGuid();

    return request is null
      ? null
      : new DbQuestion
      {
        Id = questionId,
        GroupId = request.GroupId,
        Content = request.Content,
        Deadline = request.Deadline,
        HasRealTimeResult = request.HasRealTimeResult,
        IsAnonymous = request.IsAnonymous,
        IsRevoteAvailable = request.IsRevoteAvailable,
        IsObligatory = request.IsObligatory,
        IsPrivate = request.IsPrivate,
        HasMultipleChoice = request.HasMultipleChoice,
        HasCustomOptions = request.HasCustomOptions,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
        Options = request.Options?.Select(option => _dbOptionMapper.Map(option, questionId)).ToList()
      };
  }
}

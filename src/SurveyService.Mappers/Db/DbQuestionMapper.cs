using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbQuestionMapper : IDbQuestionMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DbQuestionMapper(
    IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }
  public DbQuestion Map(CreateQuestionRequest request, Guid? groupId = null)
  {
    return new DbQuestion 
    {
      Id = Guid.NewGuid(),
      GroupId = groupId,
      Content = request.Content,
      Deadline = request.Deadline,
      HasRealTimeResult = request.HasRealTimeResult,
      IsAnonymous = request.IsAnonymous,
      IsRevoteAvailable = request.IsRevoteAvailable,
      IsObligatory = request.IsObligatory,
      IsPrivate = request.IsPrivate,
      HasMultipleChoice = request.HasMultipleChoice,
      HasCustomOptions = request.HasCustomOptions,
      IsActive = request.IsActive,
      CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
      CreatedAtUtc = DateTime.Now
    };
  }
}

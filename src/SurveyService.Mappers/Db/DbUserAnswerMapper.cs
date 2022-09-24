using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbUserAnswerMapper : IDbUserAnswerMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DbUserAnswerMapper(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public DbUserAnswer Map(Guid id)
  {
    return new DbUserAnswer
    {
      Id = Guid.NewGuid(),
      OptionId = id,
      UserId = _httpContextAccessor.HttpContext.GetUserId(),
      CreatedAtUtc = DateTime.UtcNow
    };
  }
}

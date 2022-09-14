﻿using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbQuestionOptionMapper : IDbQuestionOptionMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DbQuestionOptionMapper(
    IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public DbOption Map(CreateQuestionOptionRequest request, Guid questionId)
  {
    return request is null
      ? null
      : new DbOption
      {
        Id = Guid.NewGuid(),
        QuestionId = questionId,
        Content = request.Content,
        IsCustom = request.IsCustom,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };
  }
}

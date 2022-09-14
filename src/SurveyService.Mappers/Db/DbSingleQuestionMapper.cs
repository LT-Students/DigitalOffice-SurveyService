﻿using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbSingleQuestionMapper : IDbSingleQuestionMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IDbOptionMapper _dbOptionsMapper;

  public DbSingleQuestionMapper(IHttpContextAccessor httpContextAccessor, IDbOptionMapper dbOptionsMapper)
  {
    _httpContextAccessor = httpContextAccessor;
    _dbOptionsMapper = dbOptionsMapper;
  }

  public DbQuestion Map(CreateSingleQuestionRequest request)
  {
    Guid questionId = Guid.NewGuid();

    for(int i = 0; i < request.Options.Count; i++)
    {
      request.Options[i].QuestionId = questionId;
    }

    return request is null
    ? null
    : new DbQuestion()
    {
      Id = questionId,
      GroupId = request.GroupId,
      Content = request.Content,
      Deadline = request.Deadline,
      HasRealTimeResult = request.HasRealTimeResult,
      IsAnonymous = request.IsAnonymous,
      IsRevoteAvailable = request.IsRevoteAvaible,
      IsObligatory = request.IsObligatory,
      IsPrivate = request.IsPrivate,
      HasMultipleChoice = request.HasMultipleChoice,
      HasCustomOptions = request.HasCustomOptions,
      IsActive = true,
      CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
      CreatedAtUtc = DateTime.UtcNow,
      Options = request.Options.Select(option => _dbOptionsMapper.Map(option)).ToList()
    };
  }
}
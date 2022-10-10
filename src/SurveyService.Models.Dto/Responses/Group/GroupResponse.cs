﻿using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Responses.Group;

public record GroupResponse
{
  public Guid Id { get; set; }
  public string Subject { get; set; }
  public string Description { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public List<QuestionInfo> Questions { get; set; }
}
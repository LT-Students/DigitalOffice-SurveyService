using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using Microsoft.AspNetCore.Http;
using System;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbGroupMapper : IDbGroupMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DbGroupMapper(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }
  
  public DbGroup Map(CreateGroupRequest request)
  {
    return request is null
      ? null
      : new DbGroup 
      {
        Id = Guid.NewGuid(),
        Subject = request.Subject,
        Description = request.Description,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.Now,
        ModifiedBy = null, 
        ModifiedAtUtc = null,
        Questions = request.Questions
      };
  }
}
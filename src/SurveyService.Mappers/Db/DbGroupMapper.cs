using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LT.DigitalOffice.SurveyService.Mappers.Db;

public class DbGroupMapper : IDbGroupMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IDbGroupQuestionMapper _groupQuestionMapper;
  
  public DbGroupMapper(
    IHttpContextAccessor httpContextAccessor,
    IDbGroupQuestionMapper groupQuestionMapper)
  {
    _httpContextAccessor = httpContextAccessor;
    _groupQuestionMapper = groupQuestionMapper;
  }
  
  public DbGroup Map(CreateGroupRequest request)
  {
    Guid groupId = Guid.NewGuid();
    
    return request is null
      ? null
      : new DbGroup 
      {
        Id = groupId,
        Subject = request.Subject,
        Description = request.Description,
        IsActive = true,
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow,
        Questions = request.Questions.Select(question => _groupQuestionMapper.Map(question, groupId, request.Deadline, request.HasRealTimeResult)).ToList()
      };
  }
}

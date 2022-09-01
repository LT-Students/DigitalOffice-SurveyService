using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Group;

public class CreateGroupCommand : ICreateGroupCommand
{
  private readonly IGroupRepository _groupRepository;
  private readonly IDbGroupMapper _dbGroupMapper;
  private readonly IResponseCreator _responseCreator;
  private readonly ICreateGroupRequestValidator _validator;
  private readonly IHttpContextAccessor _httpContextAccessor;
  
  public CreateGroupCommand(
    IGroupRepository groupRepository,
    IDbGroupMapper dbGroupMapper,
    IResponseCreator responseCreator,
    ICreateGroupRequestValidator validator,
    IHttpContextAccessor httpContextAccessor)
  {
    _groupRepository = groupRepository;
    _dbGroupMapper = dbGroupMapper;
    _responseCreator = responseCreator;
    _validator = validator;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateGroupRequest request)
  {
    ValidationResult validationResult = await _validator.ValidateAsync(request);
    
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(vf => vf.ErrorMessage).ToList()
      );
    }

    Guid? createGroupGuid = await _groupRepository.CreateAsync(_dbGroupMapper.Map(request));
    
    if (createGroupGuid is null)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        new List<string> { "Error occured while creating group." });
    }
    
    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
    
    return new OperationResultResponse<Guid?>(body: createGroupGuid);
  }
}

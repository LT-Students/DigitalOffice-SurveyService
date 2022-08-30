using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
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
  
  public CreateGroupCommand(
    IGroupRepository groupRepository,
    IDbGroupMapper dbGroupMapper,
    IResponseCreator responseCreator,
    ICreateGroupRequestValidator validator)
  {
    _groupRepository = groupRepository;
    _dbGroupMapper = dbGroupMapper;
    _responseCreator = responseCreator;
    _validator = validator;
  }

  public async Task<OperationResultResponse<Guid>> ExecuteAsync(CreateGroupRequest request)
  {
    ValidationResult validationResult = await _validator.ValidateAsync(request);
    
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(vf => vf.ErrorMessage).ToList()
      );
    }

    DbGroup dbGroup = _dbGroupMapper.Map(request);
    Guid? createGroupGuid = await _groupRepository.CreateAsync(dbGroup);
    
    if (createGroupGuid is null)
    {
      return _responseCreator.CreateFailureResponse<Guid>(
        HttpStatusCode.BadRequest,
        new List<string> { "Error occured while creating group." }
        );
    }
    
    return new OperationResultResponse<Guid>(body: (Guid)createGroupGuid);
  }
}
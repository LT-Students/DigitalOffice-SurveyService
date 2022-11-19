using FluentValidation.Results;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using LT.DigitalOffice.SurveyService.Validation.Group.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Group;

public class EditGroupCommand : IEditGroupCommand
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IGroupRepository _groupRepository;
  private readonly IQuestionRepository _questionRepository;
  private readonly IAccessValidator _accessValidator;
  private readonly IResponseCreator _responseCreator;
  private readonly IEditGroupRequestValidator _validator;
  private readonly IPatchDbGroupMapper _mapper;

  public EditGroupCommand(
    IHttpContextAccessor httpContextAccessor,
    IGroupRepository groupRepository,
    IAccessValidator accessValidator,
    IResponseCreator responseCreator,
    IEditGroupRequestValidator validator,
    IQuestionRepository questionRepository,
    IPatchDbGroupMapper mapper)
  {
    _httpContextAccessor = httpContextAccessor;
    _groupRepository = groupRepository;
    _questionRepository = questionRepository;
    _accessValidator = accessValidator;
    _responseCreator = responseCreator;
    _validator = validator;
    _mapper = mapper;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid GroupId, JsonPatchDocument<EditGroupRequest> request)
  {
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();
    DbGroup dbGroup = await _groupRepository.GetAsync(GroupId);

    if (dbGroup is null)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.NotFound,
        new List<string> { "Group not found." });
    }

    if (!await _accessValidator.IsAdminAsync(senderId)
      && senderId != dbGroup.CreatedBy)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.Forbidden,
        new List<string> { "You have no rights to edit this group" });
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(e => e.ErrorMessage).ToList());
    }

    if (request.Operations.Any(op => op.path.EndsWith(nameof(EditGroupRequest.IsActive), StringComparison.OrdinalIgnoreCase))
      && !request.Operations
        .Where(op => op.path.EndsWith(nameof(EditGroupRequest.IsActive), StringComparison.OrdinalIgnoreCase))
        .Select(op =>
        {
          bool.TryParse(op.value.ToString(), out bool value);
          return value;
        })
        .First())
    {
      await _questionRepository.DeactivateAsync(dbGroup.Questions, senderId);
    }

    return new OperationResultResponse<bool>(
      body: await _groupRepository.EditAsync(_mapper.Map(request), dbGroup, senderId));
  }
}

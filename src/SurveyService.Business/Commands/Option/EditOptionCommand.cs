using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Option.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option;

public class EditOptionCommand : IEditOptionCommand
{
  private readonly IPatchDbOptionMapper _mapper;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IResponseCreator _responseCreator;
  private readonly IAccessValidator _accessValidator;
  private readonly IOptionRepository _optionRepository;
  private readonly IEditOptionRequestValidator _editOptionRequestValidator;

  public EditOptionCommand(
    IPatchDbOptionMapper patchDbOptionMapper,
    IHttpContextAccessor httpContextAccessor,
    IResponseCreator responseCreator,
    IAccessValidator accessValidator,
    IOptionRepository optionRepository,
    IEditOptionRequestValidator editOptionRequestValidator)
  {
    _mapper = patchDbOptionMapper;
    _httpContextAccessor = httpContextAccessor;
    _responseCreator = responseCreator;
    _accessValidator = accessValidator;
    _optionRepository = optionRepository;
    _editOptionRequestValidator = editOptionRequestValidator;
  }
  
  public async Task<OperationResultResponse<bool>> ExecuteAsync(Guid optionId, JsonPatchDocument<EditOptionRequest> request)
  {
    Guid userId = _httpContextAccessor.HttpContext.GetUserId();
    DbOption dbOption = (await _optionRepository.GetByIdsAsync(new List<Guid> { optionId })).FirstOrDefault();

    if (!await _accessValidator.IsAdminAsync(userId) && userId != dbOption.CreatedBy)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _editOptionRequestValidator.ValidateAsync((dbOption, request));

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest,
        validationResult.Errors.Select(error => error.ErrorMessage).ToList());
    }
    
    if (request.Operations.Any(operation => operation.path.EndsWith(nameof(EditOptionRequest.IsActive),
          StringComparison.OrdinalIgnoreCase) && request.Operations.Where(operation => operation.path.EndsWith(nameof(EditOptionRequest.IsActive), StringComparison.OrdinalIgnoreCase)).Select(
          operation =>
          {
            bool.TryParse(operation.value.ToString(), out bool value);
            return value;
          })
          )
        .First())
    return new OperationResultResponse<bool>(null);
  }
}

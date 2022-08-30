using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Option.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto;
using LT.DigitalOffice.SurveyService.Validation.Option.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option;

public class CreateOptionCommand : ICreateOptionCommand
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ICreateOptionRequestValidator _validator;
  private readonly IOptionRepository _optionRepository;
  private readonly IDbOptionMapper _mapper;
  private readonly IResponseCreator _responseCreator;

  public CreateOptionCommand(
    IHttpContextAccessor httpContextAccessor,
    ICreateOptionRequestValidator validator,
    IOptionRepository optionRepository,
    IDbOptionMapper mapper,
    IResponseCreator responseCreator)
  {
    _httpContextAccessor = httpContextAccessor;
    _validator = validator;
    _optionRepository = optionRepository;
    _mapper = mapper;
    _responseCreator = responseCreator;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateOptionRequest request)
  {
    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.BadRequest,
        validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
    }

    DbOption dbOption = _mapper.Map(request);
    await _optionRepository.CreateAsync(dbOption);

    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return new OperationResultResponse<Guid?>(
      body: dbOption.Id);
  }
}

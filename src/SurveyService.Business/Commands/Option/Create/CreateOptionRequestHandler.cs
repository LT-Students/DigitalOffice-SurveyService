using FluentValidation;
using LT.DigitalOffice.SurveyService.DataLayer;
using LT.DigitalOffice.SurveyService.DataLayer.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option.Create;

public class CreateOptionRequestHandler : IRequestHandler<CreateOptionRequest, Guid>
{
  private readonly SurveyServiceDbContext _dbContext;
  private readonly ILogger<CreateOptionRequestHandler> _logger;
  private readonly IValidator<(CreateOptionRequest request, bool? hasCustomOptions)> _validator;

  public CreateOptionRequestHandler(
    IValidator<(CreateOptionRequest request, bool? hasCustomOptions)> validator,
    SurveyServiceDbContext dbContext,
    ILogger<CreateOptionRequestHandler> logger)
  {
    _validator = validator;
    _dbContext = dbContext;
    _logger = logger;
  }

  public async Task<Guid> Handle(CreateOptionRequest request, CancellationToken ct)
  {
    await _validator.ValidateAndThrowAsync((request, null), ct);

    DbOption dbOption = new() 
    {
      Id = Guid.NewGuid(),
      QuestionId = request.QuestionId,
      Content = request.Content,
      IsCustom = request.IsCustom
    };

    await _dbContext.Options.AddAsync(dbOption, ct);
    await _dbContext.SaveChangesAsync(ct);
    
    _logger.LogInformation(
      "Option '{optionId}' was created for question '{questionId}'.",
      dbOption.Id,
      request.QuestionId);
    
    return dbOption.Id;
  }
}

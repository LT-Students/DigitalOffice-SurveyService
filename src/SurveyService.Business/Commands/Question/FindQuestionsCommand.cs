using FluentValidation.Results;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Enums;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question;

public class FindQuestionsCommand : IFindQuestionsCommand
{
  private readonly IBaseFindFilterValidator _baseFindFilterValidator;
  private readonly IResponseCreator _responseCreator;
  private readonly IQuestionRepository _questionRepository;
  private readonly IGroupRepository _groupRepository;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public FindQuestionsCommand(
    IBaseFindFilterValidator baseFindFilterValidator,
    IResponseCreator responseCreator,
    IQuestionRepository questionRepository,
    IGroupRepository groupRepository,
    IHttpContextAccessor httpContextAccessor)
  {
    _baseFindFilterValidator = baseFindFilterValidator;
    _responseCreator = responseCreator;
    _questionRepository = questionRepository;
    _groupRepository = groupRepository;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<FindResultResponse<FindQuestionsResultInfo>> ExecuteAsync(FindQuestionsFilter filter)
  {
    ValidationResult validationResult = _baseFindFilterValidator.Validate(filter);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureFindResponse<FindQuestionsResultInfo>(HttpStatusCode.BadRequest,
        validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
    }

    (List<DbQuestion> dbQuestions, int totalCount) = await _questionRepository.FindByAuthorAsync(filter, _httpContextAccessor.HttpContext.GetUserId());

    FindResultResponse<FindQuestionsResultInfo> response = new(
      body: dbQuestions.Select(q => new FindQuestionsResultInfo
      {
        ItemType = ItemType.Question,
        ItemId = q.Id,
        Content = q.Content,
        IsActive = q.IsActive,
      }).ToList(),
      totalCount: totalCount);

    if (filter.IncludeGroup)
    {
      (List<DbGroup> dbGroups, int totalGroupCount) = await _groupRepository.FindByAuthorAsync(filter, _httpContextAccessor.HttpContext.GetUserId());

      response.Body.AddRange(dbGroups.Select(g => new FindQuestionsResultInfo
      {
        ItemType = ItemType.Group,
        ItemId = g.Id,
        Content = g.Subject,
        IsActive = g.IsActive,
      }).ToList());

      response.TotalCount += totalGroupCount;
    }

    return response;
  }
}

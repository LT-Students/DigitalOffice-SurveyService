using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.SurveyService.Broker.Requests.Interfaces;
using LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Group;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Group;

public class GetGroupCommand : IGetGroupCommand
{
  private readonly IGroupRepository _repository;
  private readonly IGroupResponseMapper _groupResponseMapper;
  private readonly IResponseCreator _responseCreator;
  private readonly IUserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public GetGroupCommand(
    IGroupRepository groupRepository,
    IGroupResponseMapper groupResponseMapper,
    IResponseCreator responseCreator,
    IUserService userService,
    IHttpContextAccessor httpContextAccessor)
  {
    _repository = groupRepository;
    _groupResponseMapper = groupResponseMapper;
    _responseCreator = responseCreator;
    _userService = userService;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<OperationResultResponse<GroupResponse>> ExecuteAsync(GetGroupFilter filter)
  {
    DbGroup dbGroup = await _repository.GetAsync(filter);

    if (dbGroup is null)
    {
      return _responseCreator.CreateFailureResponse<GroupResponse>(
        HttpStatusCode.NotFound,
        new List<string> { "Group not found" });
    }

    if (dbGroup.Questions.Any()
      && dbGroup.Questions.FirstOrDefault().Deadline > DateTime.UtcNow
      && !dbGroup.Questions.FirstOrDefault().HasRealTimeResult
      && filter.IncludeUserAnswers)
    {
      return _responseCreator.CreateFailureResponse<GroupResponse>(
        HttpStatusCode.Forbidden,
        new List<string> { "You cannot see the answers because the deadline has not come yet." });
    }

    List<UserData> usersData = new List<UserData>();

    foreach (DbQuestion question in dbGroup.Questions)
    {
      if (filter.IncludeUserInfo && question.IsAnonymous)
      {
        return _responseCreator.CreateFailureResponse<GroupResponse>(
          HttpStatusCode.Forbidden,
          new List<string> { "One of the questions is anonymous. User information is not available." });
      }

      if (question.IsPrivate && filter.IncludeUserAnswers
        && question.CreatedBy != _httpContextAccessor.HttpContext.GetUserId())
      {
        return _responseCreator.CreateFailureResponse<GroupResponse>(
          HttpStatusCode.Forbidden,
          new List<string> { "One of the questions is private, you are not the author of it." });
      }

      usersData = filter.IncludeUserInfo
        ? (await _userService.GetUsersDataAsync(
          dbGroup.Questions.SelectMany(q => q.Options).SelectMany(o => o.UsersAnswers).Select(ua => ua.UserId).Distinct().ToList(), new List<string>()))
        : null;
    }

    return new(body: _groupResponseMapper.Map(dbGroup, usersData));
  }
}

using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.User;
using LT.DigitalOffice.Models.Broker.Responses.User;
using LT.DigitalOffice.SurveyService.Broker.Requests.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Broker.Requests
{
  public class UserService : IUserService
  {
    private readonly IRequestClient<IGetUsersDataRequest> _rcGetUsersData;
    private readonly ILogger<UserService> _logger;

    public UserService(
      IRequestClient<IGetUsersDataRequest> rcGetUsersData,
      ILogger<UserService> logger)
    {
      _rcGetUsersData = rcGetUsersData;
      _logger = logger;
    }

    public async Task<List<UserData>> GetUsersDatasAsync(
      List<Guid> usersIds,
      List<string> errors = null,
      CancellationToken cancellationToken = default
      )
    {
      if (usersIds is null || !usersIds.Any())
      {
        return null;
      }

      object request = IGetUsersDataRequest.CreateObj(usersIds);
      
      List<UserData> usersData = (await _rcGetUsersData.ProcessRequest<IGetUsersDataRequest, IGetUsersDataResponse>(
          request,
          errors,
          _logger))?.UsersData;

      return usersData;
    }
  }
}
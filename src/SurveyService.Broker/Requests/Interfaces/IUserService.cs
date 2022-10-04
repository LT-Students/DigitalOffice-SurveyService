using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Broker.Requests.Interfaces
{
  [AutoInject]
  public interface IUserService
  {
    Task<List<UserData>> GetUsersDatasAsync(
      List<Guid> usersIds,
      List<string> errors,
      CancellationToken cancellationToken = default);
  }
}

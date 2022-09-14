using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Data.Provider;
using LT.DigitalOffice.SurveyService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data;
public class GroupRepository : IGroupRepository
{
  private readonly IDataProvider _provider;

  public GroupRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  public Task<DbGroup> GetPropertiesAsync(Guid groupId)
  {
    return _provider.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
  }
}

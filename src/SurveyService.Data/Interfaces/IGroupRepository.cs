using LT.DigitalOffice.SurveyService.Models.Db;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Data.Interfaces;

public interface IGroupRepository
{
  Task<DbGroup> GetPropertiesAsync(Guid groupId);
}
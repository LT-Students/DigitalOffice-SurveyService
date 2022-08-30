using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;

public interface ICreateGroupCommand
{
  Task<OperationResultResponse<Guid>> ExecuteAsync(CreateGroupRequest request);
}
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Group.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Group;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Group.Interfaces;

[AutoInject]
public interface IGetGroupCommand
{
  Task<OperationResultResponse<GroupResponse>> ExecuteAsync(GetGroupFilter filter);
}
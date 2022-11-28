using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.UserAnswer;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.UserAnswer.Interfaces;

[AutoInject]
public interface IRemoveUserAnswerCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(RemoveUserAnswerRequest request);
}


using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;

[AutoInject]
public interface ICreateQuestionCommand
{
  Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateSingleQuestionRequest request);
}

using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using System;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;

[AutoInject]
public interface IEditQuestionCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(Guid questionId, JsonPatchDocument<EditQuestionRequest> request);
}

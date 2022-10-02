using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;

[AutoInject]
public interface IGetQuestionCommand
{
  Task<OperationResultResponse<QuestionResponse>> ExecuteAsync(Guid questionId);
}

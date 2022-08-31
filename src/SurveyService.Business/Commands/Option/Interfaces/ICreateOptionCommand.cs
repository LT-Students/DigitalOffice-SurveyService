using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Option;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Option.Interfaces;

[AutoInject]
public interface ICreateOptionCommand
{
  Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateOptionRequest request);
}

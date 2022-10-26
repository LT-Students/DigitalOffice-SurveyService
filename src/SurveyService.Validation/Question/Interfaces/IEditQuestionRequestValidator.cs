using FluentValidation;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.SurveyService.Validation.Question.Interfaces;

[AutoInject]
public interface IEditQuestionRequestValidator : IValidator<(DbQuestion, JsonPatchDocument<EditQuestionRequest>)>
{
}

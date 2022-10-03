using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Requests.Question.Filters;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question;

public class GetQuestionCommand : IGetQuestionCommand
{
  private readonly IQuestionRepository _repository;
  private readonly IQuestionResponseMapper _mapper;
  private readonly IResponseCreator _responseCreator;

  public GetQuestionCommand(
    IQuestionRepository questionRepository, 
    IQuestionResponseMapper mapper,
    IResponseCreator responseCreator)
  {
    _repository = questionRepository;
    _mapper = mapper;
    _responseCreator = responseCreator;
  }
  
  public async Task<OperationResultResponse<QuestionResponse>> ExecuteAsync(GetQuestionFilter filter)
  {
    OperationResultResponse<QuestionResponse> response = new();

    if (filter is null || filter.QuestionId is null)
    {
      return _responseCreator.CreateFailureResponse<QuestionResponse>(
        HttpStatusCode.BadRequest,
        new List<string> { "You must enter 'questionid'" });
    }

    DbQuestion dbQuestion = await _repository.GetAsync(filter.QuestionId.Value);
    response.Body = _mapper.Map(dbQuestion);
    return response;
  }
}

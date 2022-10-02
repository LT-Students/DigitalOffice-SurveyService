using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.SurveyService.Business.Commands.Question.interfaces;
using LT.DigitalOffice.SurveyService.Data.Interfaces;
using LT.DigitalOffice.SurveyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Responses.Question;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.SurveyService.Business.Commands.Question;

public class GetQuestionCommand : IGetQuestionCommand
{
  private readonly IQuestionRepository _repository;
  private readonly IQuestionResponseMapper _mapper;

  public GetQuestionCommand(
    IQuestionRepository questionRepository, 
    IQuestionResponseMapper mapper)
  {
    _repository = questionRepository;
    _mapper = mapper;
  }
  
  public async Task<OperationResultResponse<QuestionResponse>> ExecuteAsync(Guid questionId)
  {
    OperationResultResponse<QuestionResponse> response = new();

    DbQuestion dbQuestion = await _repository.GetAsync(questionId);
    response.Body = _mapper.Map(dbQuestion);
    return response;
  }
}

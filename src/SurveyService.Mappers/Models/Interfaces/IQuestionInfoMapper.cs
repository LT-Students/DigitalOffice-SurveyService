using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.SurveyService.Models.Db;
using LT.DigitalOffice.SurveyService.Models.Dto.Models;
using System.Collections.Generic;

namespace LT.DigitalOffice.SurveyService.Mappers.Models.Interfaces;

[AutoInject]
public interface IQuestionInfoMapper
{
  QuestionInfo Map(DbQuestion dbQuestion, List<OptionInfo> optionInfos);
}

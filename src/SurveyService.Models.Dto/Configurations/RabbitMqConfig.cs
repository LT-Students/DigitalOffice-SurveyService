using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.SurveyService.Models.Dto.Configurations
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    [AutoInjectRequest(typeof(IGetUsersDataRequest))]
    public string GetUsersDataEndpoint { get; set; }
  }
}

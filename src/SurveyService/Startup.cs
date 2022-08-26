using HealthChecks.UI.Client;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Kernel.BrokerSupport.Extensions;
using LT.DigitalOffice.Kernel.BrokerSupport.Helpers;
using LT.DigitalOffice.Kernel.BrokerSupport.Middlewares.Token;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Kernel.EFSupport.Extensions;
using LT.DigitalOffice.Kernel.EFSupport.Helpers;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Middlewares.ApiInformation;
using LT.DigitalOffice.SurveyService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.SurveyService.Models.Dto.Configurations;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace LT.DigitalOffice.SurveyService
{
  public class Startup : BaseApiInfo
  {
    private readonly BaseServiceInfoConfig _serviceInfoConfig;

    private readonly RabbitMqConfig _rabbitMqConfig;

    public const string CorsPolicyName = "LtDoCorsPolicy";


    public IConfiguration Configuration { get; }

    #region private methods

    private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
      var builder = new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson()
        .Services.BuildServiceProvider();

      return builder
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value
        .InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();
    }



    #region configure masstransit

    private void ConfigureMassTransit(IServiceCollection services)
    {
      (string username, string password) = RabbitMqCredentialsHelper
        .Get(_rabbitMqConfig, _serviceInfoConfig);

      services.AddMassTransit(busConfigurator =>
      {
        busConfigurator.UsingRabbitMq((context, cfg) =>
          {
            cfg.Host(_rabbitMqConfig.Host, "/", host =>
              {
                host.Username(username);
                host.Password(password);
              });

            ConfigureEndpoints(context, cfg, _rabbitMqConfig);
          });

        ConfigureConsumers(busConfigurator);

        busConfigurator.AddRequestClients(_rabbitMqConfig);
      });

      services.AddMassTransitHostedService();
    }

    private void ConfigureConsumers(IServiceCollectionBusConfigurator x)
    {
    }

    private void ConfigureEndpoints(
        IBusRegistrationContext context,
        IRabbitMqBusFactoryConfigurator cfg,
        RabbitMqConfig rabbitMqConfig)
    {
    }

    #endregion

    #endregion

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      _serviceInfoConfig = Configuration
        .GetSection(BaseServiceInfoConfig.SectionName)
        .Get<BaseServiceInfoConfig>();

      _rabbitMqConfig = Configuration
        .GetSection(BaseRabbitMqConfig.SectionName)
        .Get<RabbitMqConfig>();

      Version = "1.0.0.0";
      Description = "SurveyService is an API that intended to work with surveies.";
      StartTime = DateTime.UtcNow;
      ApiName = $"LT Digital Office - {_serviceInfoConfig.Name}";
    }


    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy(
          CorsPolicyName,
          builder =>
          {
            builder
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
          });
      });

      services.AddHttpContextAccessor();

      string dbConnStr = ConnectionStringHandler.Get(Configuration);

      services.AddDbContext<SurveyServiceDbContext>(options =>
      {
        options.UseSqlServer(dbConnStr);
      });

      services.AddHealthChecks()
        .AddRabbitMqCheck()
        .AddSqlServer(dbConnStr);

      services.Configure<TokenConfiguration>(Configuration.GetSection("CheckTokenMiddleware"));
      services.Configure<BaseServiceInfoConfig>(Configuration.GetSection(BaseServiceInfoConfig.SectionName));
      services.Configure<BaseRabbitMqConfig>(Configuration.GetSection(BaseRabbitMqConfig.SectionName));

      services.AddBusinessObjects();

      ConfigureMassTransit(services);

      services
        .AddControllers()
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        })
        .AddNewtonsoftJson();
    }


    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
      app.UpdateDatabase<SurveyServiceDbContext>();

      app.UseForwardedHeaders();

      app.UseExceptionsHandler(loggerFactory);

      app.UseApiInformation();

      app.UseCors(CorsPolicyName);

      app.UseRouting();

      app.UseMiddleware<TokenMiddleware>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers().RequireCors(CorsPolicyName);
        endpoints.MapHealthChecks($"/{_serviceInfoConfig.Id}/hc", new HealthCheckOptions
        {
          ResultStatusCodes = new Dictionary<HealthStatus, int>
          {
            { HealthStatus.Unhealthy, 200 },
            { HealthStatus.Healthy, 200 },
            { HealthStatus.Degraded, 200 },
          },
          Predicate = check => check.Name != "masstransit-bus",
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
      });

    }
  }
}

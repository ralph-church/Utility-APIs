using integration.services.kafka.shared.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using repair.service.api.middleware;
using repair.service.repository;
using repair.service.repository.abstracts;
using repair.service.repository.config;
using repair.service.service.abstracts;
using repair.service.service.mapping;
using repair.service.service.messaging;
using repair.service.service.model;
using repair.service.shared.abstracts;
using repair.service.shared.exception.extensions;
using repair.service.shared.exception.model;
using repair.service.shared.constants;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using telematics.shared;

namespace repair.service.api
{
    public class Startup
    {
        private string _environment = "local";
        public Startup(IConfiguration configuration)
        {
            _environment = Environment.GetEnvironmentVariable(AppConstants.Environment);
            Configuration = configuration;
            var loggingLevel = Configuration["SeriLog:MinimumLevel:Default"];
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = String.IsNullOrEmpty(loggingLevel) ?
                                        LogEventLevel.Information :
                                        (LogEventLevel)Enum.Parse(typeof(LogEventLevel), loggingLevel);

            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(Configuration)
                                .MinimumLevel.ControlledBy(levelSwitch)
                                .Enrich.FromLogContext()
                                .Enrich.WithCorrelationIdHeader(AppConstants.CorrelationId)
                                .WriteTo.Console(new ExpressionTemplate(
                                 "{ {..@p , @l: if @l = 'Information' then undefined() else @l,@m,@x,@sc: SourceContext, @t:UtcDateTime(@t),  SourceContext: undefined()} }\n"))
                                .WriteTo.File(@"Logs/" + GetLogFileName())
                                .CreateLogger();

            Log.Information("RepairService API Started in the Environment" + _environment);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "repair_service_api", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddCors(c =>
            {
                c.AddPolicy("CorsPolicy", options =>
                {
                    options.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .WithExposedHeaders("*");
                });
            });
            services.AddApiVersioning(opt =>
            {
                // Provides the different api version in response header
                opt.ReportApiVersions = true;
                // this configuration will allow the api to automaticaly take api_version=1.0 in case it was not specify
                opt.AssumeDefaultVersionWhenUnspecified = true;
                // We are giving the default version of 1.0 to the api
                opt.DefaultApiVersion = ApiVersion.Default; // new ApiVersion(1, 0);
            });
            services.Configure<DatabaseSettings>(options => Configuration.GetSection("DatabaseSettings").Bind(options))
                    .AddSingleton<IDatabaseSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value)
                    .Configure<TtcServicesConfig>(options => Configuration.GetSection("TTCServicesConfig").Bind(options))
                    .AddSingleton<ITtcServicesConfig>(serviceProvider => serviceProvider.GetRequiredService<IOptions<TtcServicesConfig>>().Value)
                    .Configure<List<ProducerConnectionOptions>>(options => Configuration.GetSection("ProducerConnectionOptions").Bind(options))
                    .AddSingleton<ITopicConnectionOptions>(serviceProvider => serviceProvider.GetRequiredService<IOptions<ProducerConnectionOptions>>().Value)
                    .AddSingleton(typeof(IRepairServiceContext), typeof(RepairServiceContext))
                    .AddSingleton(AutoMapperConfiguration.Configure())
                    .AddScoped(typeof(IDataRepository<>), typeof(MongoRepository<>))                  
                    .AddHttpClient()
                    .AddTransient<IHttpService, HttpService>()
                    .AddScoped(typeof(IOutboundService), typeof(TopicService));
            RegisterService<IRepairRequestService>(services);

            // Create an OpenTracing ITracer with the default setting
            //OpenTracing.ITracer tracer = OpenTracingTracerFactory.CreateTracer();

            // Use the tracer with ASP.NET Core dependency injection
            //services.AddSingleton<OpenTracing.ITracer>(tracer);

            // Use the tracer with OpenTracing.GlobalTracer.Instance
            //GlobalTracer.Register(tracer);

        }


        private static void RegisterService<T>(IServiceCollection services) where T : class
        {
            var assembly = typeof(T).Assembly;
            var types = assembly.ExportedTypes.Where(x => x.IsClass && x.IsPublic);
            foreach (var type in types)
            {
                if (type.GetInterface($"I{type.Name}") != null)
                {
                    services.AddScoped(type.GetInterface($"I{type.Name}"), type);
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "repair_service_api v1"));
            }

            app.ConfigureCustomExceptionMiddleware(new ExceptionOption() { EnableTrace = Convert.ToBoolean(Environment.GetEnvironmentVariable(AppConstants.LogTrace)) });

            app.UseRouting();
            app.UseMiddleware<JwtMiddleware>();
            // This line should be place between the UseRouting() and UseEndpoints() in order to support cross orgin request.
            app.UseCors("CorsPolicy");
           
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This is a temporary code, should remove after mongo timeout issue resolved
        private static string GetLogFileName()
        {
            return string.Concat(
                        Path.GetFileNameWithoutExtension("app-"),
                        DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff"), ".log");
        }
    }
}

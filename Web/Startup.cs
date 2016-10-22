using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Common.Logging;
using Core;
using Data;
using GdShows.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using IConfiguration = Common.IConfiguration;

namespace GdShows
{
	public class Startup
	{
		private IContainer _applicationContainer;
		private readonly IConfiguration _configuration;
		private ILog _log;
		private readonly IConfigurationRoot _configurationRoot;

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
				.AddEnvironmentVariables();
			_configurationRoot = builder.Build();

			_configuration = new Common.Configuration(_configurationRoot);
		}

		private ILog ConfigureLogger()
		{
			var baseDir = AppDomain.CurrentDomain.BaseDirectory;

			LogEventLevel logLevel;
			if (!Enum.TryParse(_configuration.LogLevel, true, out logLevel))
				logLevel = LogEventLevel.Information;
			var logger = new LoggerConfiguration()
				.WriteTo
				.RollingFile($@"{baseDir}\logs\{{Date}}.txt", logLevel, retainedFileCountLimit: 10, shared: true)
				.WriteTo.ColoredConsole()
				.WriteTo.LiterateConsole()
				.CreateLogger();

			Log.Logger = logger;

			Log.Write(LogEventLevel.Information, "Logging has started");

			return LogProvider.For<Startup>();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			ConfigureApi(services);

			ConfigureSwagger(services);

			CreateAutofacContainer(services);

			_log = ConfigureLogger();

			// Create the IServiceProvider based on the container.
			return new AutofacServiceProvider(_applicationContainer);
		}


		private void ConfigureApi(IServiceCollection services)
		{
			//IConfiguration is not available yet
			services.AddMvc(o => o.Filters.Add(new GlobalExceptionFilter(_configuration.EnvName)))
				.AddJsonOptions(options =>
				{
					var settings = options.SerializerSettings;
					settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					settings.Converters = new JsonConverter[]
					{
						new IsoDateTimeConverter(),
						new StringEnumConverter()
					};
				});

			if (_configuration.DebugMode)
			{
				//glimpse/client/index.html
				//services.AddGlimpse().RunningAgentWeb();
			}
		}

		private void ConfigureSwagger(IServiceCollection services)
		{
			services.AddSwaggerGen();

			services.ConfigureSwaggerGen(options =>
			{
				options.OperationFilter<ExtraParametersFilter>();
				options.CustomSchemaIds(type => type.FullName);
				options.DescribeStringEnumsInCamelCase();
			});
		}

		private void CreateAutofacContainer(IServiceCollection services)
		{
			var builder = new ContainerBuilder();

			builder.Populate(services);
			builder.Register(ctx => _configurationRoot).As<IConfigurationRoot>();
			builder.RegisterModule<DataModule>();
			builder.RegisterModule<CoreModule>();
			builder.RegisterModule<CommonModule>();

			//last
			builder.RegisterModule<ServerModule>();

			_applicationContainer = builder.Build();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
		{
			loggerFactory.AddConsole();

			_log.Info("Starting application");

			ConfigureAssets(app);
			ConfigureDebug(app);
			ConfigureSwagger(app);

			app.UseMvc();

			ConfigureDatabase();

			if (_configuration.MaintenanceMode)
				app.UseWelcomePage();

			appLifetime.ApplicationStopped.Register(() => _applicationContainer.Dispose());
		}

		private void ConfigureSwagger(IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUi();
		}
		
		private void ConfigureAssets(IApplicationBuilder app)
		{
			var config = _configuration;
			if (config.MaintenanceMode)
				return;

			// Route all unknown requests to app root
			app.Use(async (context, next) =>
			{
				await next();

				// If there's no available file and the request doesn't contain an extension, we're probably trying to access a page.
				// Rewrite request to use app root
				if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
				{
					context.Request.Path = "/index.html"; // Put your Angular root page here 
					context.Response.StatusCode = 200; // Make sure we update the status code, otherwise it returns 404
					await next();
				}
			});

			app.UseFileServer();
		}

		private void ConfigureDebug(IApplicationBuilder app)
		{
			var config = _configuration;

			if (!config.DebugMode)
				return;

			app.UseDeveloperExceptionPage();
		}

		private void ConfigureDatabase()
		{
			/*
			var databaseRunner = _applicationContainer.Resolve<IDatabaseRunner>();
			var configuration = _applicationContainer.Resolve<IConfiguration>();
			RunResult result = null;
			try
			{
				var checkDb = configuration.CheckForDatabase;

				result = databaseRunner.Run(checkDb);

				if (!result.Success)
					_log.ErrorException($"Could not update database: {result}", result.Error);
			}
			catch (Exception exception)
			{
				_log.ErrorException($"Could not update database: {result}", exception);
			}
			*/
		}
	}
}

using System.Collections.Generic;
using Autofac;
using AutoMapper;
using Common.Services;
using Data;
using GdShows.API.Services;
using Models.Api;
using IConfiguration = Common.IConfiguration;

namespace GdShows
{
    public class ServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = ThisAssembly;
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(ServiceInvoker<,>)).As(typeof(IServiceInvoker<,>)).InstancePerLifetimeScope();
            builder.Register(RegisterContext).As<IGdShowContext>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly).Where(type => type.IsSubclassOf(typeof(Profile))).As<Profile>();

            builder.RegisterType<PathProvider>().As<IPathProvider>().InstancePerLifetimeScope();
            builder.RegisterType<CurrentUserContext>().AsSelf().InstancePerLifetimeScope();
			
            builder.Register(context =>
            {
                var mapperConfiguration = new MapperConfiguration(expression =>
                {
                    var profiles = context.Resolve<IEnumerable<Profile>>();

                    foreach (var profile in profiles)
                    {
                        expression.AddProfile(profile);
                    }
                });

                return mapperConfiguration;

            }).InstancePerLifetimeScope().AsSelf();

            builder.Register(context => context.Resolve<MapperConfiguration>().CreateMapper(context.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }

        private GdShowContext RegisterContext(IComponentContext context)
        {
            var configuration = context.Resolve<IConfiguration>();
            var connectionString = configuration.ContextConnectionString;
            var dbContext = new GdShowContext(connectionString);

            return dbContext;
        }

    }
}

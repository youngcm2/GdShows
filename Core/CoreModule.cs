using Autofac;

namespace Core
{
	public class CoreModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			var assembly = ThisAssembly;

			builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
		}
	}
}

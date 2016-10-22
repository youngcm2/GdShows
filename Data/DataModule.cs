using System;
using Autofac;

namespace Data
{
	public class DataModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			var assembly = ThisAssembly;
			builder.RegisterAssemblyTypes(assembly)
				.Where(type => !type.Name.StartsWith("Fake", StringComparison.OrdinalIgnoreCase))
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}

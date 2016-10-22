using Autofac;
using Common.Utilities;

namespace Common
{
	public class CommonModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();
			builder.RegisterType<Hasher>().As<IHasher>().InstancePerLifetimeScope();
			builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
		}
	}
}
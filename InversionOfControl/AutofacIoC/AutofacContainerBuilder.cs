namespace InversionOfControl.AutofacIoC;

using System.ComponentModel;
using Autofac;
using Common;
using Microsoft.Extensions.DependencyInjection;
using IContainer = Common.IContainer;

public class AutofacContainerBuilder : IContainerBuilder
{
	private readonly ContainerBuilder containerBuilder;

	public AutofacContainerBuilder()
	{
		containerBuilder = new ContainerBuilder();
	}

	public IDisposableContainer Build()
	{
		return new AutofacReadOnlyContainer(containerBuilder.Build());
	}

	public IContainerBuilder Register<T>() where T : class
	{
		containerBuilder.RegisterType<T>().InstancePerDependency();
		return this;
	}

	public IContainerBuilder Register<T>(Scope scope) where T : class
	{
		switch (scope)
		{
			case Scope.Singleton:
				containerBuilder.RegisterType<T>().SingleInstance();
				break;
			case Scope.Transient:
				containerBuilder.RegisterType<T>().InstancePerDependency();
				break;
			case Scope.Scoped:
				containerBuilder.RegisterType<T>().InstancePerLifetimeScope();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scope));
		}

		return this;
	}

	public IContainerBuilder Register<TInterface, TImplementation>()
		where TInterface : class where TImplementation : class, TInterface
	{
		containerBuilder.RegisterType<TImplementation>().As<TInterface>();
		return this;
	}

	public IContainerBuilder Register<TInterface, TImplementation>(Scope scope)
		where TInterface : class where TImplementation : class, TInterface
	{
		switch (scope)
		{
			case Scope.Singleton:
				containerBuilder.RegisterType<TImplementation>().As<TInterface>().SingleInstance();
				break;
			case Scope.Transient:
				containerBuilder.RegisterType<TImplementation>().As<TInterface>().InstancePerDependency();
				break;
			case Scope.Scoped:
				containerBuilder.RegisterType<TImplementation>().As<TInterface>().InstancePerLifetimeScope();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scope));
		}

		return this;
	}

	public IContainerBuilder Register<T>(Func<IContainer, T> factory, Scope scope) where T : class
	{
		switch (scope)
		{
			case Scope.Singleton:
				containerBuilder.Register(context => factory(new AutofacReadOnlyContainer(context))).SingleInstance();
				break;
			case Scope.Transient:
				containerBuilder.Register(context => factory(new AutofacReadOnlyContainer(context)))
								.InstancePerDependency();
				break;
			case Scope.Scoped:
				containerBuilder.Register(context => factory(new AutofacReadOnlyContainer(context)))
								.InstancePerLifetimeScope();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scope));
		}

		return this;
	}

	public IContainerBuilder Register(Type contract, Type implementation, Scope scope)
	{
		switch (scope)
		{
			case Scope.Singleton:
				containerBuilder.RegisterType(implementation).As(contract).SingleInstance();
				break;
			case Scope.Transient:
				containerBuilder.RegisterType(implementation).As(contract).InstancePerDependency();
				break;
			case Scope.Scoped:
				containerBuilder.RegisterType(implementation).As(contract).InstancePerLifetimeScope();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scope));
		}

		return this;
	}

	public IContainerBuilder PopulateServices(IServiceCollection serviceCollection)
	{
		foreach (var service in serviceCollection)
		{
			if (service.ImplementationType is null)
			{
				continue;
			}

			switch (service.Lifetime)
			{
				case ServiceLifetime.Singleton:
					containerBuilder.RegisterType(service.ImplementationType).As(service.ServiceType).SingleInstance();
					break;
				case ServiceLifetime.Transient:
					containerBuilder.RegisterType(service.ImplementationType)
									.As(service.ServiceType)
									.InstancePerDependency();
					break;
				case ServiceLifetime.Scoped:
					containerBuilder.RegisterType(service.ImplementationType)
									.As(service.ServiceType)
									.InstancePerLifetimeScope();
					break;
				default:
					throw new InvalidEnumArgumentException(nameof(service.Lifetime));
			}
		}

		return this;
	}
}
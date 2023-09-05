namespace InversionOfControl.DryIoC;

using System.ComponentModel;
using Common;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Container = DryIoc.Container;
using IContainer = Common.IContainer;
using Scope = Common.Scope;

public class DryIocContainerBuilder : IContainerBuilder
{
	private readonly Container container;

	public DryIocContainerBuilder()
	{
		container = new Container(rules => rules.WithUseInterpretation());
	}

	public IDisposableContainer Build()
	{
		return new DryIocReadOnlyContainer(container);
	}

	public IContainerBuilder Register<T>() where T : class
	{
		container.Register<T>(Reuse.Transient);
		return this;
	}

	public IContainerBuilder Register<T>(Scope scope) where T : class
	{
		switch (scope)
		{
			case Scope.Singleton:
				container.Register<T>(Reuse.Singleton);
				break;
			case Scope.Transient:
				container.Register<T>(Reuse.Transient);
				break;
			case Scope.Scoped:
				container.Register<T>(Reuse.Scoped);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scope));
		}

		return this;
	}

	public IContainerBuilder Register<TInterface, TImplementation>()
		where TInterface : class where TImplementation : class, TInterface
	{
		container.Register<TInterface, TImplementation>();
		return this;
	}

	public IContainerBuilder Register<TInterface, TImplementation>(Scope scope)
		where TInterface : class where TImplementation : class, TInterface
	{
		switch (scope)
		{
			case Scope.Singleton:
				container.Register<TInterface, TImplementation>(Reuse.Singleton);
				break;
			case Scope.Transient:
				container.Register<TInterface, TImplementation>(Reuse.Transient);
				break;
			case Scope.Scoped:
				container.Register<TInterface, TImplementation>(Reuse.Scoped);
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
				container.RegisterDelegate(context => factory(new DryIocReadOnlyContainer(context)), Reuse.Singleton);
				break;
			case Scope.Transient:
				container.RegisterDelegate(context => factory(new DryIocReadOnlyContainer(context)), Reuse.Transient);
				break;
			case Scope.Scoped:
				container.RegisterDelegate(context => factory(new DryIocReadOnlyContainer(context)), Reuse.Scoped);
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
				container.Register(contract, implementation, Reuse.Singleton);
				break;
			case Scope.Transient:
				container.Register(contract, implementation, Reuse.Transient);
				break;
			case Scope.Scoped:
				container.Register(contract, implementation, Reuse.Scoped);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scope));
		}

		return this;
	}

	public IContainerBuilder PopulateServices(IServiceCollection serviceCollection)
	{
		var lifetimeMap = new Dictionary<ServiceLifetime, IReuse>
		{
			{
				ServiceLifetime.Singleton, Reuse.Singleton
			},
			{
				ServiceLifetime.Transient, Reuse.Transient
			},
			{
				ServiceLifetime.Scoped, Reuse.Scoped
			}
		};
		foreach (var service in serviceCollection)
		{
			if (!lifetimeMap.TryGetValue(service.Lifetime, out var reuse))
			{
				throw new InvalidEnumArgumentException(nameof(service.Lifetime));
			}

			container.Register(service.ServiceType, service.ImplementationType, reuse);
		}

		return this;
	}
}
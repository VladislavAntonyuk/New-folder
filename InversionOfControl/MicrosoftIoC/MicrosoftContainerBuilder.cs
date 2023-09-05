namespace InversionOfControl.MicrosoftIoC;

using Common;
using Microsoft.Extensions.DependencyInjection;

public class MicrosoftContainerBuilder : IContainerBuilder
{
	private readonly IServiceCollection services;

	public MicrosoftContainerBuilder()
	{
		services = new ServiceCollection();
	}

	public IDisposableContainer Build()
	{
		return new MicrosoftReadOnlyContainer(services.BuildServiceProvider());
	}

	public IContainerBuilder Register<T>() where T : class
	{
		services.AddTransient<T>();
		return this;
	}

	public IContainerBuilder Register<T>(Scope scope) where T : class
	{
		switch (scope)
		{
			case Scope.Singleton:
				services.AddSingleton<T>();
				break;
			case Scope.Transient:
				services.AddTransient<T>();
				break;
			case Scope.Scoped:
				services.AddScoped<T>();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(scope));
		}

		return this;
	}

	public IContainerBuilder Register<TInterface, TImplementation>()
		where TInterface : class where TImplementation : class, TInterface
	{
		services.AddTransient<TInterface, TImplementation>();
		return this;
	}

	public IContainerBuilder Register<TInterface, TImplementation>(Scope scope)
		where TInterface : class where TImplementation : class, TInterface
	{
		switch (scope)
		{
			case Scope.Singleton:
				services.AddSingleton<TInterface, TImplementation>();
				break;
			case Scope.Transient:
				services.AddTransient<TInterface, TImplementation>();
				break;
			case Scope.Scoped:
				services.AddScoped<TInterface, TImplementation>();
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
				services.AddSingleton(context => factory(new MicrosoftReadOnlyContainer(context)));
				break;
			case Scope.Transient:
				services.AddTransient(context => factory(new MicrosoftReadOnlyContainer(context)));
				break;
			case Scope.Scoped:
				services.AddScoped(context => factory(new MicrosoftReadOnlyContainer(context)));
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
				services.AddSingleton(contract, implementation);
				break;
			case Scope.Transient:
				services.AddTransient(contract, implementation);
				break;
			case Scope.Scoped:
				services.AddScoped(contract, implementation);
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
			services.Add(service);
		}

		return this;
	}
}
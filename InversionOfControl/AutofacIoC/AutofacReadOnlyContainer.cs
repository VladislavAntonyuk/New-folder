namespace InversionOfControl.AutofacIoC;

using Autofac;

public sealed class AutofacReadOnlyContainer : Common.INativeContainer<IContainer, ILifetimeScope>
{
	private readonly IComponentContext componentContext;

	public AutofacReadOnlyContainer(IComponentContext autofacComponentContext)
	{
		componentContext = autofacComponentContext;
	}

	public ValueTask DisposeAsync()
	{
		return GetNativeContainer().DisposeAsync();
	}

	public IContainer GetNativeContainer()
	{
		return (IContainer)componentContext;
	}

	public T GetInstance<T>() where T : notnull
	{
		return componentContext.Resolve<T>();
	}

	public object GetInstance(Type type)
	{
		return componentContext.Resolve(type);
	}

	public ILifetimeScope CreateScope()
	{
		return GetNativeContainer().BeginLifetimeScope();
	}

	public T GetScopedInstance<T>(ILifetimeScope serviceScope) where T : notnull
	{
		return serviceScope.Resolve<T>();
	}

	public object GetScopedInstance(Type type, ILifetimeScope serviceScope)
	{
		return serviceScope.Resolve(type);
	}
}
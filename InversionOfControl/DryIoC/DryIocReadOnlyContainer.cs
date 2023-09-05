namespace InversionOfControl.DryIoC;

using Common;
using DryIoc;

public sealed class DryIocReadOnlyContainer : INativeContainer<IResolverContext, IResolverContext>
{
	private readonly IResolverContext container;

	public DryIocReadOnlyContainer(IResolverContext dryiocContainer)
	{
		container = dryiocContainer;
	}

	public ValueTask DisposeAsync()
	{
		GetNativeContainer().Dispose();
		return ValueTask.CompletedTask;
	}

	public IResolverContext GetNativeContainer()
	{
		return container;
	}

	public T GetInstance<T>() where T : notnull
	{
		return container.Resolve<T>();
	}

	public object GetInstance(Type type)
	{
		return container.Resolve(type);
	}

	public IResolverContext CreateScope()
	{
		return GetNativeContainer().OpenScope();
	}

	public T GetScopedInstance<T>(IResolverContext serviceScope) where T : notnull
	{
		return serviceScope.Resolve<T>();
	}

	public object GetScopedInstance(Type type, IResolverContext serviceScope)
	{
		return serviceScope.Resolve(type);
	}
}
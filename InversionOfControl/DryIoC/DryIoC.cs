namespace InversionOfControl.DryIoC;

using Common;
using DryIoc;

public static class DryIoC
{
	private static IDisposableContainer? _container;

	static DryIoC()
	{
		ContainerBuilder = new DryIocContainerBuilder();
	}

	public static IContainerBuilder ContainerBuilder { get; }

	public static IDisposableContainer GetContainer()
	{
		return _container ??= ContainerBuilder.Build();
	}

	public static IResolverContext GetNativeContainer()
	{
		return ((INativeContainer<IResolverContext, IResolverContext>)GetContainer()).GetNativeContainer();
	}
}
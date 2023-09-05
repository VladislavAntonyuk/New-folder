namespace InversionOfControl.AutofacIoC;

using Autofac;
using Common;
using IContainer = Autofac.IContainer;

public static class AutofacIoC
{
	private static IDisposableContainer? _container;

	static AutofacIoC()
	{
		ContainerBuilder = new AutofacContainerBuilder();
	}

	public static IContainerBuilder ContainerBuilder { get; }

	public static IDisposableContainer GetContainer()
	{
		return _container ??= ContainerBuilder.Build();
	}

	public static IComponentContext GetNativeContainer()
	{
		return ((INativeContainer<IContainer, ILifetimeScope>)GetContainer()).GetNativeContainer();
	}
}
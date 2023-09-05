namespace InversionOfControl.MicrosoftIoC;

using Common;
using Microsoft.Extensions.DependencyInjection;

public static class MicrosoftIoC
{
	private static IDisposableContainer? _container;

	static MicrosoftIoC()
	{
		ContainerBuilder = new MicrosoftContainerBuilder();
	}

	public static IContainerBuilder ContainerBuilder { get; }

	public static IDisposableContainer GetContainer()
	{
		return _container ??= ContainerBuilder.Build();
	}

	public static IServiceProvider GetNativeContainer()
	{
		return ((INativeContainer<IServiceProvider, IServiceScope>)GetContainer()).GetNativeContainer();
	}
}
namespace InversionOfControl;

using Common;

public static class IoC
{
	static IoC()
	{
		ContainerBuilder = MicrosoftIoC.MicrosoftIoC.ContainerBuilder;
	}

	public static IContainerBuilder ContainerBuilder { get; }

	public static IDisposableContainer GetContainer()
	{
		return MicrosoftIoC.MicrosoftIoC.GetContainer();
	}

	public static IServiceProvider GetNativeContainer()
	{
		return MicrosoftIoC.MicrosoftIoC.GetNativeContainer();
	}
}
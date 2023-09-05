namespace InversionOfControl.MicrosoftIoC;

using Common;
using Microsoft.Extensions.DependencyInjection;

public sealed class MicrosoftReadOnlyContainer : INativeContainer<IServiceProvider, IServiceScope>
{
	private readonly IServiceProvider container;

	public MicrosoftReadOnlyContainer(IServiceProvider serviceProvider)
	{
		container = serviceProvider;
	}

	public ValueTask DisposeAsync()
	{
		return ValueTask.CompletedTask;
	}

	public IServiceProvider GetNativeContainer()
	{
		return container;
	}

	public T GetInstance<T>() where T : notnull
	{
		return container.GetRequiredService<T>();
	}

	public object GetInstance(Type type)
	{
		return container.GetRequiredService(type);
	}

	public T GetScopedInstance<T>(IServiceScope serviceScope) where T : notnull
	{
		return serviceScope.ServiceProvider.GetRequiredService<T>();
	}

	public object GetScopedInstance(Type type, IServiceScope serviceScope)
	{
		return serviceScope.ServiceProvider.GetRequiredService(type);
	}

	public IServiceScope CreateScope()
	{
		return container.CreateScope();
	}
}
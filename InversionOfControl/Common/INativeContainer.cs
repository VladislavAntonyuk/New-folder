namespace InversionOfControl.Common;

public interface INativeContainer<out TContainer, TScope> : IDisposableContainer
{
	TContainer GetNativeContainer();

	TScope CreateScope();

	T GetScopedInstance<T>(TScope serviceScope) where T : notnull;

	object GetScopedInstance(Type type, TScope scope);
}
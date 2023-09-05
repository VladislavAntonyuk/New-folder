namespace InversionOfControl.Common;

public interface IContainer
{
	T GetInstance<T>() where T : notnull;
	object GetInstance(Type type);
}
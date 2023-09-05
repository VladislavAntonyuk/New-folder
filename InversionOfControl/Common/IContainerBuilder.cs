namespace InversionOfControl.Common;

using Microsoft.Extensions.DependencyInjection;

public interface IContainerBuilder
{
	/// <summary>
	///     Build the container builder
	/// </summary>
	/// <returns>Read only container</returns>
	IDisposableContainer Build();

	/// <summary>
	///     Register Type with named constructor injection support
	/// </summary>
	/// <typeparam name="T">Class type</typeparam>
	/// <returns>Container builder</returns>
	IContainerBuilder Register<T>() where T : class;

	/// <summary>
	///     Register type depend from scope
	/// </summary>
	/// <typeparam name="T">Class type</typeparam>
	/// <param name="scope">Life time</param>
	/// <returns>Container builder</returns>
	IContainerBuilder Register<T>(Scope scope) where T : class;

	/// <summary>
	///     Register type as interface
	/// </summary>
	/// <typeparam name="TInterface">Abstract interface</typeparam>
	/// <typeparam name="TImplementation">Class implementation</typeparam>
	/// <returns>Container builder</returns>
	IContainerBuilder Register<TInterface, TImplementation>()
		where TInterface : class where TImplementation : class, TInterface;

	/// <summary>
	///     Register type as interface
	/// </summary>
	/// <typeparam name="TInterface">Abstract interface</typeparam>
	/// <typeparam name="TImplementation">Class implementation</typeparam>
	/// <param name="scope">Life time</param>
	/// <returns>Container builder</returns>
	IContainerBuilder Register<TInterface, TImplementation>(Scope scope)
		where TInterface : class where TImplementation : class, TInterface;

	/// <summary>
	///     Register a delegate as component
	/// </summary>
	/// <typeparam name="T">Class type</typeparam>
	/// <param name="factory">Function factory</param>
	/// <param name="scope">Life time</param>
	/// <returns>Container builder</returns>
	IContainerBuilder Register<T>(Func<IContainer, T> factory, Scope scope) where T : class;

	IContainerBuilder Register(Type contract, Type implementation, Scope scope);

	IContainerBuilder PopulateServices(IServiceCollection serviceCollection);
}
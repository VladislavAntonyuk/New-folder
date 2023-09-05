namespace InversionOfControl.Tests.DryIoC;

using Common;
using DryIoc;
using FluentAssertions;
using HelperClasses;
using InversionOfControl.DryIoC;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Scope = Common.Scope;

public class DryIoCContainerTests : ContainerTests<IResolverContext, IResolverContext>
{
	public DryIoCContainerTests()
	{
		var builder = new DryIocContainerBuilder();
		builder.Register<ITestService, TestServiceOne>();
		var container = builder.Build();
		Target = (INativeContainer<IResolverContext, IResolverContext>)container;
	}

	[Fact]
	public async Task TargetShouldDispose()
	{
		Target.GetNativeContainer().IsDisposed.Should().BeFalse();
		await Target.DisposeAsync();
		Target.GetNativeContainer().IsDisposed.Should().BeTrue();
	}

	[Fact]
	public void ShouldRegisterLambdaAsScoped()
	{
		var builder = new DryIocContainerBuilder();
		builder.Register<ITestService>(c => new TestServiceOne(), Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IResolverContext, IResolverContext>)container;
		//
		using var scope = Target.CreateScope();
		var firstInstance = Target.GetScopedInstance<ITestService>(scope);
		var secondInstance = Target.GetScopedInstance<ITestService>(scope);
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}


	[Fact]
	public void ShouldRegisterImplementationTypeAsScoped()
	{
		var builder = new DryIocContainerBuilder();
		builder.Register<ITestService, TestServiceOne>(Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IResolverContext, IResolverContext>)container;
		//
		using var scope = Target.CreateScope();
		var firstInstance = Target.GetScopedInstance<ITestService>(scope);
		var secondInstance = Target.GetScopedInstance<ITestService>(scope);
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void ShouldRegisterTypeAsScoped()
	{
		var builder = new DryIocContainerBuilder();
		builder.Register<TestServiceOne>(Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IResolverContext, IResolverContext>)container;
		//
		using var scope = Target.CreateScope();
		var firstInstance = Target.GetScopedInstance<TestServiceOne>(scope);
		var secondInstance = Target.GetScopedInstance<TestServiceOne>(scope);
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}


	[Theory]
	[InlineData(typeof(TestServiceOne), Constants.TestServiceOne)]
	[InlineData(typeof(TestServiceTwo), Constants.TestServiceTwo)]
	public void ShouldRegisterNotGenericImplementationTypeAsSingleton(Type type, string result)
	{
		var builder = new DryIocContainerBuilder();
		builder.Register(typeof(ITestService), type, Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IResolverContext, IResolverContext>)container;
		//
		using var scope = Target.CreateScope();
		var firstInstance = (ITestService)Target.GetScopedInstance(typeof(ITestService), scope);
		var secondInstance = (ITestService)Target.GetScopedInstance(typeof(ITestService), scope);
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(result);
		secondInstance.Call().Should().BeEquivalentTo(result);
	}


	[Fact]
	public void PopulateServicesScoped()
	{
		// Arrange
		var builder = new DryIocContainerBuilder();
		var serviceCollection = new ServiceCollection();
		serviceCollection.AddScoped<IAnotherTestService, TestServiceWithConstructorInjection>();
		builder.Register<ITestService, TestServiceOne>(Scope.Scoped);
		builder.PopulateServices(serviceCollection);
		var container = builder.Build();
		Target = (INativeContainer<IResolverContext, IResolverContext>)container;

		// Act
		using var scope = Target.CreateScope();
		var firstInstance = Target.GetScopedInstance<IAnotherTestService>(scope);
		var secondInstance = Target.GetScopedInstance<IAnotherTestService>(scope);

		// Assert
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
	}
}
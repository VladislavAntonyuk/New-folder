namespace InversionOfControl.Tests.AutofacIoC;

using Autofac;
using Common;
using FluentAssertions;
using HelperClasses;
using InversionOfControl.AutofacIoC;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using IContainer = Autofac.IContainer;

public class AutofacContainerTests : ContainerTests<IContainer, ILifetimeScope>
{
	public AutofacContainerTests()
	{
		var builder = new AutofacContainerBuilder();
		builder.Register<TestServiceOne>();
		var container = builder.Build();
		Target = (Common.INativeContainer<IContainer, ILifetimeScope>)container;
	}

	[Fact]
	public async Task TargetShouldDispose()
	{
		Target.GetNativeContainer()
			  .Invoking(c => c.Resolve<TestServiceOne>())
			  .Should()
			  .NotThrow<ObjectDisposedException>();
		await Target.DisposeAsync();
		Target.GetNativeContainer()
			  .Invoking(c => c.Resolve<TestServiceOne>())
			  .Should()
			  .Throw<ObjectDisposedException>();
	}

	[Fact]
	public void ShouldRegisterLambdaAsScoped()
	{
		var builder = new AutofacContainerBuilder();
		builder.Register<ITestService>(c => new TestServiceOne(), Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IContainer, ILifetimeScope>)container;
		//
		var firstInstance = Target.GetInstance<ITestService>();
		using var scope = Target.CreateScope();
		var secondInstance = Target.GetScopedInstance<ITestService>(scope);
		var thirdInstance = Target.GetScopedInstance<ITestService>(scope);
		firstInstance.Should().NotBeSameAs(secondInstance);
		secondInstance.Should().BeSameAs(thirdInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		thirdInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void ShouldRegisterImplementationTypeAsScoped()
	{
		var builder = new AutofacContainerBuilder();
		builder.Register<ITestService, TestServiceOne>(Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IContainer, ILifetimeScope>)container;
		//
		var thirdService = Target.GetInstance<ITestService>();
		using var scope = Target.CreateScope();
		var firstInstance = Target.GetScopedInstance<ITestService>(scope);
		var secondInstance = Target.GetScopedInstance<ITestService>(scope);
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Should().NotBeSameAs(thirdService);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void ShouldRegisterTypeAsScoped()
	{
		var builder = new AutofacContainerBuilder();
		builder.Register<TestServiceOne>(Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IContainer, ILifetimeScope>)container;
		//
		var thirdService = Target.GetInstance<TestServiceOne>();
		using var scope = Target.CreateScope();
		var firstInstance = Target.GetScopedInstance<TestServiceOne>(scope);
		var secondInstance = Target.GetScopedInstance<TestServiceOne>(scope);
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Should().NotBeSameAs(thirdService);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}


	[Theory]
	[InlineData(typeof(TestServiceOne), Constants.TestServiceOne)]
	[InlineData(typeof(TestServiceTwo), Constants.TestServiceTwo)]
	public void ShouldRegisterNotGenericImplementationTypeAsScoped(Type type, string result)
	{
		var builder = new AutofacContainerBuilder();
		builder.Register(typeof(ITestService), type, Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IContainer, ILifetimeScope>)container;
		//
		var thirdService = (ITestService)Target.GetInstance(typeof(ITestService));
		using var scope = Target.CreateScope();
		var firstInstance = (ITestService)Target.GetScopedInstance(typeof(ITestService), scope);
		var secondInstance = (ITestService)Target.GetScopedInstance(typeof(ITestService), scope);
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Should().NotBeSameAs(thirdService);
		firstInstance.Call().Should().BeEquivalentTo(result);
		secondInstance.Call().Should().BeEquivalentTo(result);
	}

	[Fact]
	public void PopulateServicesScoped()
	{
		// Arrange
		var builder = new AutofacContainerBuilder();
		var serviceCollection = new ServiceCollection();
		serviceCollection.AddScoped<IAnotherTestService, TestServiceWithConstructorInjection>();
		builder.Register<ITestService, TestServiceOne>(Scope.Scoped);
		builder.PopulateServices(serviceCollection);
		var container = builder.Build();
		Target = (INativeContainer<IContainer, ILifetimeScope>)container;

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
namespace InversionOfControl.Tests.MicrosoftIoC;

using Common;
using FluentAssertions;
using HelperClasses;
using InversionOfControl.MicrosoftIoC;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class MicrosoftContainerTests : ContainerTests<IServiceProvider, IServiceScope>
{
	public MicrosoftContainerTests()
	{
		var builder = new MicrosoftContainerBuilder();
		builder.Register<TestServiceOne>();
		var container = builder.Build();
		Target = (INativeContainer<IServiceProvider, IServiceScope>)container;
	}

	[Fact]
	public void ShouldRegisterLambdaAsScoped()
	{
		var builder = new MicrosoftContainerBuilder();
		builder.Register<ITestService>(c => new TestServiceOne(), Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IServiceProvider, IServiceScope>)container;
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
		var builder = new MicrosoftContainerBuilder();
		builder.Register<ITestService, TestServiceOne>(Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IServiceProvider, IServiceScope>)container;
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
		var builder = new MicrosoftContainerBuilder();
		builder.Register<TestServiceOne>(Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IServiceProvider, IServiceScope>)container;
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
	public void ShouldRegisterNotGenericImplementationTypeAsSingleton(Type type, string result)
	{
		var builder = new MicrosoftContainerBuilder();
		builder.Register(typeof(ITestService), type, Scope.Scoped);
		var container = builder.Build();
		Target = (INativeContainer<IServiceProvider, IServiceScope>)container;
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
		var builder = new MicrosoftContainerBuilder();
		var serviceCollection = new ServiceCollection();
		serviceCollection.AddSingleton<IAnotherTestService, TestServiceWithConstructorInjection>();
		builder.Register<ITestService, TestServiceOne>();
		builder.PopulateServices(serviceCollection);
		var container = builder.Build();
		Target = (INativeContainer<IServiceProvider, IServiceScope>)container;

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
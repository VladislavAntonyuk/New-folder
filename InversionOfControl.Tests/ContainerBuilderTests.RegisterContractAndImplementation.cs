namespace InversionOfControl.Tests;

using Common;
using FluentAssertions;
using HelperClasses;
using Xunit;

public abstract partial class ContainerBuilderTests
{
	[Fact]
	public void ShouldRegisterImplementationTypeThrowExceptionAsInvalidScope()
	{
		ContainerBuilder.Invoking(t => t.Register<ITestService, TestServiceOne>((Scope)20))
						.Should()
						.Throw<ArgumentOutOfRangeException>()
						.WithParameterName("scope");
	}

	[Fact]
	public void ShouldRegisterImplementationTypeAsSingleton()
	{
		ContainerBuilder.Register<ITestService, TestServiceOne>(Scope.Singleton);
		var container = ContainerBuilder.Build();
		//
		var firstInstance = container.GetInstance<ITestService>();
		var secondInstance = container.GetInstance<ITestService>();
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void ShouldRegisterImplementationTypeAsTransient()
	{
		ContainerBuilder.Register<ITestService, TestServiceOne>(Scope.Transient);
		var container = ContainerBuilder.Build();
		//
		var firstInstance = container.GetInstance<ITestService>();
		var secondInstance = container.GetInstance<ITestService>();
		firstInstance.Should().NotBeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void ShouldRegisterImplementationTypeAsTransientByDefault()
	{
		ContainerBuilder.Register<ITestService, TestServiceOne>();
		var container = ContainerBuilder.Build();
		//
		var firstInstance = container.GetInstance<ITestService>();
		var secondInstance = container.GetInstance<ITestService>();
		firstInstance.Should().NotBeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}
}
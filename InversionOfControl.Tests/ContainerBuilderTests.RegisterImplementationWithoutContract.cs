namespace InversionOfControl.Tests;

using Common;
using FluentAssertions;
using HelperClasses;
using Xunit;

public abstract partial class ContainerBuilderTests
{
	[Fact]
	public void ShouldRegisterTypeThrowExceptionAsInvalidScope()
	{
		ContainerBuilder.Invoking(t => t.Register<TestServiceOne>((Scope)20))
						.Should()
						.Throw<ArgumentOutOfRangeException>()
						.WithParameterName("scope");
	}

	[Fact]
	public void ShouldRegisterTypeAsSingleton()
	{
		ContainerBuilder.Register<TestServiceOne>(Scope.Singleton);
		var container = ContainerBuilder.Build();
		//
		var firstInstance = container.GetInstance<TestServiceOne>();
		var secondInstance = container.GetInstance<TestServiceOne>();
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void ShouldRegisterTypeAsTransient()
	{
		ContainerBuilder.Register<TestServiceOne>(Scope.Transient);
		var container = ContainerBuilder.Build();
		//
		var firstInstance = container.GetInstance<TestServiceOne>();
		var secondInstance = container.GetInstance<TestServiceOne>();
		firstInstance.Should().NotBeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void ShouldRegisterTypeAsTransientByDefault()
	{
		ContainerBuilder.Register<TestServiceOne>();
		var container = ContainerBuilder.Build();
		//
		var firstInstance = container.GetInstance<TestServiceOne>();
		var secondInstance = container.GetInstance<TestServiceOne>();
		firstInstance.Should().NotBeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}
}
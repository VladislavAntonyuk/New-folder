namespace InversionOfControl.Tests;

using Common;
using FluentAssertions;
using HelperClasses;
using Xunit;

public abstract partial class ContainerBuilderTests
{
	[Fact]
	public void ShouldRegisterNotGenericImplementationTypeThrowExceptionAsInvalidScope()
	{
		ContainerBuilder.Invoking(t => t.Register(typeof(ITestService), typeof(TestServiceOne), (Scope)20))
						.Should()
						.Throw<ArgumentOutOfRangeException>()
						.WithParameterName("scope");
	}

	[Theory]
	[InlineData(typeof(TestServiceOne), Constants.TestServiceOne)]
	[InlineData(typeof(TestServiceTwo), Constants.TestServiceTwo)]
	public void ShouldRegisterNotGenericImplementationTypeAsSingleton(Type type, string result)
	{
		ContainerBuilder.Register(typeof(ITestService), type, Scope.Singleton);
		var container = ContainerBuilder.Build();
		//
		var firstInstance = (ITestService)container.GetInstance(typeof(ITestService));
		var secondInstance = (ITestService)container.GetInstance(typeof(ITestService));
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(result);
		secondInstance.Call().Should().BeEquivalentTo(result);
	}

	[Fact]
	public void ShouldRegisterNotGenericImplementationTypeAsTransient()
	{
		ContainerBuilder.Register(typeof(ITestService), typeof(TestServiceOne), Scope.Transient);
		var container = ContainerBuilder.Build();
		//
		var firstInstance = container.GetInstance<ITestService>();
		var secondInstance = container.GetInstance<ITestService>();
		firstInstance.Should().NotBeSameAs(secondInstance);
		firstInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.Call().Should().BeEquivalentTo(Constants.TestServiceOne);
	}
}
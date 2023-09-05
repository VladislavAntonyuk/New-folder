namespace InversionOfControl.Tests;

using FluentAssertions;
using HelperClasses;
using Xunit;

public abstract partial class ContainerBuilderTests
{
	[Fact]
	public void ShouldResolveConstructorInjection()
	{
		// Arrange
		ContainerBuilder.Register<IAnotherTestService, TestServiceWithConstructorInjection>();
		ContainerBuilder.Register<ITestService, TestServiceOne>();
		var container = ContainerBuilder.Build();

		// Act
		var firstInstance = container.GetInstance<IAnotherTestService>();
		var secondInstance = container.GetInstance<IAnotherTestService>();

		// Assert
		firstInstance.Should().NotBeSameAs(secondInstance);
		firstInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
	}
}
namespace InversionOfControl.Tests;

using Common;
using FluentAssertions;
using HelperClasses;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public abstract partial class ContainerBuilderTests
{
	[Fact]
	public void PopulateServices()
	{
		// Arrange
		var serviceCollection = new ServiceCollection();
		serviceCollection.AddTransient<IAnotherTestService, TestServiceWithConstructorInjection>();
		ContainerBuilder.Register<ITestService, TestServiceOne>();
		ContainerBuilder.PopulateServices(serviceCollection);
		var container = ContainerBuilder.Build();

		// Act
		var firstInstance = container.GetInstance<IAnotherTestService>();
		var secondInstance = container.GetInstance<IAnotherTestService>();

		// Assert
		firstInstance.Should().NotBeSameAs(secondInstance);
		firstInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
	}

	[Fact]
	public void PopulateServicesSingleton()
	{
		// Arrange
		var serviceCollection = new ServiceCollection();
		serviceCollection.AddSingleton<IAnotherTestService, TestServiceWithConstructorInjection>();
		ContainerBuilder.Register<ITestService, TestServiceOne>(Scope.Singleton);
		ContainerBuilder.PopulateServices(serviceCollection);
		var container = ContainerBuilder.Build();

		// Act
		var firstInstance = container.GetInstance<IAnotherTestService>();
		var secondInstance = container.GetInstance<IAnotherTestService>();

		// Assert
		firstInstance.Should().BeSameAs(secondInstance);
		firstInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
		secondInstance.CallChildService1().Should().BeEquivalentTo(Constants.TestServiceOne);
	}
}
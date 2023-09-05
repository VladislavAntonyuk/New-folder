namespace InversionOfControl.Tests.DryIoC;

using System.ComponentModel;
using HelperClasses;
using InversionOfControl.DryIoC;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class DryIoCContainerBuilderTests : ContainerBuilderTests
{
	public DryIoCContainerBuilderTests()
	{
		ContainerBuilder = new DryIocContainerBuilder();
	}

	[Fact]
	public void PopulateServices_InvalidScope_ThrowException()
	{
		var serviceCollection = new ServiceCollection();
		var item = new ServiceDescriptor(typeof(IAnotherTestService), typeof(TestServiceWithConstructorInjection),
										 (ServiceLifetime)10);
		serviceCollection.Insert(0, item);
		Assert.Throws<InvalidEnumArgumentException>(() => ContainerBuilder.PopulateServices(serviceCollection));
	}
}
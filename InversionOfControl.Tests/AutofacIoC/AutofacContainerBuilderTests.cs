namespace InversionOfControl.Tests.AutofacIoC;

using System.ComponentModel;
using HelperClasses;
using InversionOfControl.AutofacIoC;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class AutofacContainerBuilderTests : ContainerBuilderTests
{
	public AutofacContainerBuilderTests()
	{
		ContainerBuilder = new AutofacContainerBuilder();
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
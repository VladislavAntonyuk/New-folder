namespace InversionOfControl.Tests;

using FluentAssertions;
using Xunit;

public class IoCTests
{
	[Fact]
	public void ContainerBuilderShouldNotBeNull()
	{
		IoC.ContainerBuilder.Should().NotBeNull();
	}

	[Fact]
	public void GetContainerShouldNotBeNull()
	{
		IoC.GetContainer().Should().NotBeNull();
	}

	[Fact]
	public void GetNativeContainerShouldNotBeNull()
	{
		IoC.GetNativeContainer().Should().NotBeNull();
	}
}
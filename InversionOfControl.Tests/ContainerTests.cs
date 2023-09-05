namespace InversionOfControl.Tests;

using Common;
using FluentAssertions;
using Xunit;

public abstract class ContainerTests<TContainer, TScope>
{
	public required INativeContainer<TContainer, TScope> Target;

	[Fact]
	public void GetNativeContainerShouldNotBeNull()
	{
		Target.GetNativeContainer().Should().NotBeNull();
	}
}
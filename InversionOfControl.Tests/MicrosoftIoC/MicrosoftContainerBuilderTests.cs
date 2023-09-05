namespace InversionOfControl.Tests.MicrosoftIoC;

using InversionOfControl.MicrosoftIoC;

public class MicrosoftContainerBuilderTests : ContainerBuilderTests
{
	public MicrosoftContainerBuilderTests()
	{
		ContainerBuilder = new MicrosoftContainerBuilder();
	}
}
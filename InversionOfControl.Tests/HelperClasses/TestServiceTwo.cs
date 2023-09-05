namespace InversionOfControl.Tests.HelperClasses;

public class TestServiceTwo : ITestService
{
	public string Call()
	{
		return Constants.TestServiceTwo;
	}
}
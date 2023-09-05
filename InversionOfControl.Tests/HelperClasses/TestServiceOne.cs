namespace InversionOfControl.Tests.HelperClasses;

public class TestServiceOne : ITestService
{
	public string Call()
	{
		return Constants.TestServiceOne;
	}
}
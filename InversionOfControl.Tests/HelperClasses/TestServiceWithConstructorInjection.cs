namespace InversionOfControl.Tests.HelperClasses;

public class TestServiceWithConstructorInjection : IAnotherTestService
{
	private readonly ITestService testService;

	public TestServiceWithConstructorInjection(ITestService testService)
	{
		this.testService = testService;
	}

	public string CallChildService1()
	{
		return testService.Call();
	}
}
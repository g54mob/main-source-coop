public class HardcodedPrompt : IMKbPromptProvider
{
	private string key;

	public HardcodedPrompt(string key)
	{
		this.key = key;
	}

	public string GetPrompt()
	{
		return key;
	}
}

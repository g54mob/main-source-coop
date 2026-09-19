namespace Foundation.Tasks
{
	public enum TaskStrategy
	{
		BackgroundThread = 0,
		MainThread = 1,
		CurrentThread = 2,
		Coroutine = 3,
		Custom = 4
	}
}

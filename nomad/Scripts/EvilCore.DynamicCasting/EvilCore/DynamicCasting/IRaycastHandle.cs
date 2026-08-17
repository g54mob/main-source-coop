namespace EvilCore.DynamicCasting
{
	public interface IRaycastHandle
	{
		int Id { get; }

		bool IsValid { get; }

		bool IsEnabled { get; set; }

		CastRequest Request { get; }
	}
}

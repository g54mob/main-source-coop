namespace Rewired.ControllerExtensions
{
	public interface IHIDControllerExtension
	{
		ushort vendorId { get; }

		ushort productId { get; }

		string productName { get; }

		string manufacturer { get; }

		ushort usagePage { get; }

		ushort usage { get; }
	}
}

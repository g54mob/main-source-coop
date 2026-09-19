namespace Photon.Realtime
{
	public struct RaiseEventArgs
	{
		public static readonly RaiseEventArgs Default;

		public EventCaching CachingOption;

		public byte InterestGroup;

		public int[] TargetActors;

		public ReceiverGroup Receivers;
	}
}

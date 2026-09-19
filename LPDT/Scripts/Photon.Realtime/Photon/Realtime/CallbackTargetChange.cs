namespace Photon.Realtime
{
	internal class CallbackTargetChange
	{
		public readonly object Target;

		public readonly bool AddTarget;

		public CallbackTargetChange(object target, bool addTarget)
		{
			Target = target;
			AddTarget = addTarget;
		}
	}
}

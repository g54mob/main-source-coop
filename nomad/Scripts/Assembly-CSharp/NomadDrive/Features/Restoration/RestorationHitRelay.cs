using PaintCore;
using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class RestorationHitRelay : MonoBehaviour, IHitPoint, IHit, IHitLine
	{
		private INetworkHitRelay _relay;

		public void Initialize(INetworkHitRelay relay)
		{
			_relay = relay;
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			if (!preview && _relay != null && _relay.IsRelaySource)
			{
				_relay.RelayHitPoint(position, rotation, pressure, seed, priority);
			}
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			if (!preview && _relay != null && _relay.IsRelaySource)
			{
				_relay.RelayHitLine(position, endPosition, rotation, pressure, seed, clip, priority);
			}
		}
	}
}

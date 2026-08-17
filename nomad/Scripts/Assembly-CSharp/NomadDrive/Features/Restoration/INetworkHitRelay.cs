using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public interface INetworkHitRelay
	{
		bool IsRelaySource { get; }

		void RelayHitPoint(Vector3 position, Quaternion rotation, float pressure, int seed, int priority);

		void RelayHitLine(Vector3 position, Vector3 endPosition, Quaternion rotation, float pressure, int seed, bool clip, int priority);
	}
}

using Fusion;
using UnityEngine;

namespace Features.TeleportModule.Scripts.TeleportCommon
{
	public interface ITeleportable
	{
		NetworkObject NetworkObject { get; }

		void Teleport(Vector3 position);
	}
}

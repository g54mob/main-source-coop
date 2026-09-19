using Cysharp.Threading.Tasks;
using Features.TeleportModule.Scripts.TeleportCommon;
using UnityEngine;

namespace Features.TeleportModule.Scripts
{
	public interface ITeleportService
	{
		UniTask TeleportObject(ITeleportable teleportable, Vector3 position);
	}
}

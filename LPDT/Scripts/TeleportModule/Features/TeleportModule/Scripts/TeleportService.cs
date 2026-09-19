using System;
using Cysharp.Threading.Tasks;
using Features.TeleportModule.Scripts.TeleportCommon;
using UnityEngine;

namespace Features.TeleportModule.Scripts
{
	public class TeleportService : ITeleportService
	{
		private const int TIMEOUT_TIME = 5;

		public async UniTask TeleportObject(ITeleportable teleportable, Vector3 position)
		{
			teleportable.NetworkObject.RequestStateAuthority();
			await UniTask.WaitUntil(() => teleportable.NetworkObject.HasStateAuthority).Timeout(TimeSpan.FromSeconds(5.0));
			teleportable.Teleport(position);
		}
	}
}

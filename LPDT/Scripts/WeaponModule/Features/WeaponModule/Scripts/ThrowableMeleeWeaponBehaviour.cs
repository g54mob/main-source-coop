using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class ThrowableMeleeWeaponBehaviour : MeleeWeaponBehaviour
	{
		private bool _hasCollision;

		public override void Spawned()
		{
			base.Spawned();
			_simplePointGrabable.OnUnGrab += TryAttackOnThrow;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_simplePointGrabable.OnUnGrab -= TryAttackOnThrow;
		}

		private void OnCollisionEnter(Collision collision)
		{
			_hasCollision = true;
		}

		private void TryAttackOnThrow()
		{
			if (!_waitingForCollision)
			{
				WaitForCollisionOnThrowAsync().Forget();
			}
		}

		private async UniTaskVoid WaitForCollisionOnThrowAsync()
		{
			_waitingForCollision = true;
			_hasCollision = false;
			float elapsed = 0f;
			float timeout = 1.5f;
			while (!_hasCollision && elapsed < timeout)
			{
				elapsed += Time.deltaTime;
				await UniTask.Yield(PlayerLoopTiming.Update);
			}
			if (Physics.OverlapSphere((_attackOrigin != null) ? _attackOrigin.position : base.transform.position, _searchRange, _targetLayers).Any((Collider c) => c.gameObject != base.gameObject))
			{
				AttackLogic();
			}
			_waitingForCollision = false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}

using Features.AIModuleStateMachine.Scripts.Core;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class MimicFearDestroySystem : MonoSystem
	{
		[SerializeField]
		private SimpleEnemyDeadProcessor _deadProcessor;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		public override void Enable()
		{
			PrepareDespawn();
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
		}

		public void PrepareDespawn()
		{
			_deadProcessor.IsNeedToSpawnItem = false;
		}

		private void Update()
		{
			if (base.Initialized)
			{
				_ = _enabled;
			}
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

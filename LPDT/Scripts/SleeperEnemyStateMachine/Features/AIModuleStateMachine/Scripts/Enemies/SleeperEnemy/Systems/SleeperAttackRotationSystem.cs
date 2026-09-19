using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class SleeperAttackRotationSystem : MonoSystem
	{
		[SerializeField]
		private GameObject _bodyObject;

		[SerializeField]
		private float _bodyRotationSpeed = 8f;

		private IDetectionContext _detectionContext;

		private bool _isEnabled;

		private float _currentBodyYRotation;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(IDetectionContext detectionContext)
		{
			_detectionContext = detectionContext;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_currentBodyYRotation = _bodyObject.transform.eulerAngles.y;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		public override void Clear()
		{
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority || !base.Initialized || !_isEnabled)
			{
				return;
			}
			PlayerDataHolder priorityPlayer = _detectionContext.PriorityPlayer;
			if (priorityPlayer != null && !(priorityPlayer.NetworkObject == null))
			{
				Vector3 vector = priorityPlayer.NetworkObject.transform.position - base.transform.position;
				vector.y = 0f;
				if (!(vector.sqrMagnitude <= 0f))
				{
					vector.Normalize();
					float b = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
					float t = 1f - Mathf.Exp((0f - _bodyRotationSpeed) * base.Runner.DeltaTime);
					_currentBodyYRotation = Mathf.LerpAngle(_currentBodyYRotation, b, t);
					_bodyObject.transform.rotation = Quaternion.Euler(0f, _currentBodyYRotation, 0f);
				}
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

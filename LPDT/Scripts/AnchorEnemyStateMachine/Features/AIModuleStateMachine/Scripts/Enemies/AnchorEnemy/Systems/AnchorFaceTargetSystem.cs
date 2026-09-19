using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class AnchorFaceTargetSystem : MonoSystem
	{
		[SerializeField]
		private GameObject _bodyObject;

		[SerializeField]
		private float _rotationSpeed = 12f;

		private AnchorEnemyContext _context;

		private bool _isEnabled;

		private float _currentYaw;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(AnchorEnemyContext anchorEnemyContext)
		{
			_context = anchorEnemyContext;
		}

		public override void Enable()
		{
			_isEnabled = true;
			if (_bodyObject != null)
			{
				_currentYaw = _bodyObject.transform.eulerAngles.y;
			}
		}

		public override void Disable()
		{
			_isEnabled = false;
		}

		public override void Clear()
		{
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled && base.HasStateAuthority && !(_bodyObject == null) && _context.TryGetAttackTargetPosition(out var position))
			{
				Vector3 vector = position - base.transform.position;
				vector.y = 0f;
				if (!(vector.sqrMagnitude < 0.0001f))
				{
					float b = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
					_currentYaw = Mathf.LerpAngle(_currentYaw, b, Time.deltaTime * _rotationSpeed);
					_bodyObject.transform.rotation = Quaternion.Euler(0f, _currentYaw, 0f);
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

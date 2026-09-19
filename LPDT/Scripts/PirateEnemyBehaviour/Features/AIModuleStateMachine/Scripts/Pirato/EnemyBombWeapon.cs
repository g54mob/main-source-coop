using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;
using Features.AnimationModule.Scripts;
using Features.BombModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	[NetworkBehaviourWeaved(0)]
	public class EnemyBombWeapon : EnemyWeaponBase
	{
		[SerializeField]
		private float _sphereCastRadius;

		[SerializeField]
		private Transform _sphereCastPosition;

		[SerializeField]
		private AnimationType _defaultAnimationType;

		[SerializeField]
		private AnimationType _bottomAnimationType;

		[SerializeField]
		private bool _isWithAiming;

		[SerializeField]
		private PirateEnemyContext _pirateEnemyContext;

		[SerializeField]
		private GameObject _weaponVisual;

		[SerializeField]
		private NetworkObject _bombPrefab;

		[SerializeField]
		private Transform _throwPoint;

		[SerializeField]
		private float _throwDuration = 0.6f;

		[SerializeField]
		private float _throwHorizontalSpeed = 8f;

		[SerializeField]
		private float _selfDamageMultiplier = 0.5f;

		private bool _isAttackInProgress;

		public override bool IsWithAiming => _isWithAiming;

		public override AnimationType DefaultAnimationType => _defaultAnimationType;

		public override AnimationType BottomAnimationType => _bottomAnimationType;

		public override float SphereCastRadius => _sphereCastRadius;

		public override Transform SphereCastPosition => _sphereCastPosition;

		public override void EnableWeaponVisual(bool enable)
		{
			if (_weaponVisual != null)
			{
				_weaponVisual.SetActive(enable);
			}
		}

		public override void Attack(Vector3 position)
		{
			if (base.HasStateAuthority && !_isAttackInProgress)
			{
				if (_pirateEnemyContext != null && _pirateEnemyContext.IsSafeZoneAttackActive)
				{
					ThrowBombWithoutBallisticAsync(position).Forget();
				}
				else
				{
					ThrowBombAsync(position).Forget();
				}
			}
		}

		private async UniTask ThrowBombAsync(Vector3 targetPosition)
		{
			if (base.Runner == null || _bombPrefab == null)
			{
				return;
			}
			_isAttackInProgress = true;
			try
			{
				Transform transform = _weaponVisual.transform;
				NetworkObject networkObject = await base.Runner.SpawnAsync(_bombPrefab, transform.position, transform.rotation);
				if (networkObject == null)
				{
					return;
				}
				if (!FindComponent<ExplosionBombInteractable>(networkObject.gameObject, out var component))
				{
					networkObject.DespawnHierarchy();
					return;
				}
				InitializeBombOwnerDamage(component, networkObject);
				if (_weaponVisual != null)
				{
					_weaponVisual.SetActive(value: false);
				}
				component.Interact();
				if (!FindComponent<Rigidbody>(networkObject.gameObject, out var bombRigidbody))
				{
					return;
				}
				await UniTask.WaitForFixedUpdate();
				ThrowRigidbody(bombRigidbody, targetPosition);
			}
			finally
			{
				_isAttackInProgress = false;
			}
			_isAttackInProgress = false;
		}

		private async UniTask ThrowBombWithoutBallisticAsync(Vector3 targetPosition)
		{
			if (base.Runner == null || _bombPrefab == null)
			{
				return;
			}
			_isAttackInProgress = true;
			try
			{
				Transform transform = _weaponVisual.transform;
				NetworkObject networkObject = await base.Runner.SpawnAsync(_bombPrefab, transform.position, transform.rotation);
				if (networkObject == null)
				{
					return;
				}
				if (!FindComponent<ExplosionBombInteractable>(networkObject.gameObject, out var component))
				{
					networkObject.DespawnHierarchy();
					return;
				}
				InitializeBombOwnerDamage(component, networkObject);
				if (_weaponVisual != null)
				{
					_weaponVisual.SetActive(value: false);
				}
				component.Interact();
				if (FindComponent<Rigidbody>(networkObject.gameObject, out var bombRigidbody))
				{
					await UniTask.WaitForFixedUpdate();
					ThrowRigidbodyWithoutBallistic(bombRigidbody, targetPosition);
				}
			}
			finally
			{
				_isAttackInProgress = false;
			}
		}

		private void ThrowRigidbody(Rigidbody bombRigidbody, Vector3 targetPosition)
		{
			Vector3 position = bombRigidbody.position;
			Vector3 vector = targetPosition - position;
			float num = new Vector3(vector.x, 0f, vector.z).magnitude / Mathf.Max(0.01f, _throwHorizontalSpeed);
			float num2 = Mathf.Max(0.01f, _throwDuration, num);
			Vector3 vector2 = new Vector3(vector.x / num2, 0f, vector.z / num2);
			vector2.y = (vector.y - 0.5f * Physics.gravity.y * num2 * num2) / num2;
			bombRigidbody.isKinematic = false;
			bombRigidbody.useGravity = true;
			bombRigidbody.linearVelocity = Vector3.zero;
			bombRigidbody.angularVelocity = Vector3.zero;
			bombRigidbody.WakeUp();
			bombRigidbody.AddForce(vector2 * bombRigidbody.mass, ForceMode.Impulse);
		}

		private void ThrowRigidbodyWithoutBallistic(Rigidbody bombRigidbody, Vector3 targetPosition)
		{
			Vector3 vector = targetPosition - bombRigidbody.position;
			float num = Mathf.Max(0.01f, _throwDuration);
			Vector3 vector2 = ((vector.sqrMagnitude > 0.0001f) ? (vector / num) : (base.transform.forward * (1f / num)));
			bombRigidbody.isKinematic = false;
			bombRigidbody.useGravity = true;
			bombRigidbody.linearVelocity = Vector3.zero;
			bombRigidbody.angularVelocity = Vector3.zero;
			bombRigidbody.WakeUp();
			bombRigidbody.AddForce(vector2 * bombRigidbody.mass, ForceMode.Impulse);
		}

		private void InitializeBombOwnerDamage(ExplosionBombInteractable bomb, NetworkObject bombObject)
		{
			if (!(_pirateEnemyContext == null) && !(_pirateEnemyContext.Damageable == null))
			{
				bomb.InitializeOwnerDamage(_pirateEnemyContext.Damageable, _selfDamageMultiplier);
				bomb.InitializeDamageAttribution(DamageDataSourceExtensions.ForEnemyAttack(bombObject.transform, EnemyType.PirateBomb.ToString(), DamageType.Explosion));
			}
		}

		private static bool FindComponent<T>(GameObject source, out T component)
		{
			component = source.GetComponent<T>();
			return component != null;
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

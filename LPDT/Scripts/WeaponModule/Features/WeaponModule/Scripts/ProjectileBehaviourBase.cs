using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.ItemCollisionModule.Scripts;
using Features.ItemDamageModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class ProjectileBehaviourBase : NetworkBehaviour
	{
		[Tooltip("Speed of forward movement")]
		public float MoveSpeed = 10f;

		[Tooltip("Layers that will trigger destruction on collision")]
		public LayerMask DestructionLayers = -1;

		[SerializeField]
		private ParticleSystem _destroyParticlePrefab;

		[SerializeField]
		private float _enemiesDamage = 10f;

		[SerializeField]
		private float _itemsDamage = 10f;

		[SerializeField]
		private EventReference _destroySound;

		private EventInstance _destroySoundInstance;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _forceRange;

		[SerializeField]
		private float _forceStrength;

		[SerializeField]
		private bool _stunPlayers;

		[SerializeField]
		private float _playerThrowForce = 250f;

		[SerializeField]
		private float _playerKnockbackUpBias = 0.6f;

		[SerializeField]
		private AnimationCurve _forceFalloffCurve;

		[SerializeField]
		private AnimationCurve _damageFalloffCurve;

		[SerializeField]
		private Collider _collider;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[Header("Ricochet")]
		[SerializeField]
		private EventReference _ricochetSound;

		[SerializeField]
		private float _deflectClearance = 0.06f;

		[SerializeField]
		private float _deflectSweepMargin = 0.1f;

		[SerializeField]
		private float _deflectIgnoreTime = 0.25f;

		private const float MIN_DEFLECT_SPEED = 0.01f;

		private readonly RaycastHit[] _deflectSweepHits = new RaycastHit[16];

		private Transform _deflectorRoot;

		private Transform _deflectShieldedRoot;

		private float _deflectIgnoreUntil;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedDespawnPosition", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedDespawnPosition;

		[WeaverGenerated]
		[DefaultForProperty("HasNetworkedDespawnPosition", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasNetworkedDespawnPosition;

		private bool _hasPendingDespawn;

		private Vector3 _pendingDespawnPosition;

		private IItemCostReduceService _itemCostReduceService;

		private IScreenShakeService _screenShakeService;

		private IAudioService _audioService;

		private PlayerStatesConfiguration _playerStatesConfiguration;

		[field: SerializeField]
		public CinemachineImpulseSource CinemachineImpulseSource { get; private set; }

		[field: SerializeField]
		public ScreenShakeData ScreenShakeDataOnDestroy { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 3)]
		private unsafe Vector3 NetworkedDespawnPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ProjectileBehaviourBase.NetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ProjectileBehaviourBase.NetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe NetworkBool HasNetworkedDespawnPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ProjectileBehaviourBase.HasNetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ProjectileBehaviourBase.HasNetworkedDespawnPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = value;
			}
		}

		private float ProjectileRadius
		{
			get
			{
				Vector3 extents = _collider.bounds.extents;
				return Mathf.Max(extents.x, Mathf.Max(extents.y, extents.z));
			}
		}

		[Inject]
		public void InjectDependencies(IItemCostReduceService itemCostReduceService, IScreenShakeService screenShakeService, IAudioService audioService, PlayerStatesConfiguration playerStatesConfiguration)
		{
			_itemCostReduceService = itemCostReduceService;
			_screenShakeService = screenShakeService;
			_audioService = audioService;
			_playerStatesConfiguration = playerStatesConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			_destroySoundInstance = _audioService.CreateInstance(_destroySound);
			_rigidbody.AddForce(base.transform.forward * MoveSpeed);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			TryPlayDespawnVfx();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _hasPendingDespawn)
			{
				if (!HasNetworkedDespawnPosition)
				{
					NetworkedDespawnPosition = _pendingDespawnPosition;
					HasNetworkedDespawnPosition = true;
				}
				else
				{
					base.Object.DespawnHierarchy();
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (base.Object == null || !base.Object.IsValid || IsDeflectIgnored(other.transform) || TryDeflect(other) != ProjectileHitResponse.None)
			{
				return;
			}
			int layer = other.gameObject.layer;
			if (((1 << layer) & (int)DestructionLayers) == 0 || !base.Object.HasStateAuthority || _hasPendingDespawn)
			{
				return;
			}
			Vector3 position = base.transform.position;
			Collider[] array = Physics.OverlapSphere(position, _forceRange);
			Dictionary<GameObject, Collider> dictionary = new Dictionary<GameObject, Collider>();
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				dictionary.TryAdd(collider.gameObject, collider);
			}
			List<IDamageable> list = new List<IDamageable>();
			foreach (Collider value in dictionary.Values)
			{
				Rigidbody attachedRigidbody = value.attachedRigidbody;
				if (attachedRigidbody == _rigidbody || IsDeflectIgnored(value.transform))
				{
					continue;
				}
				Vector3 vector = value.ClosestPoint(position);
				float num = Mathf.Clamp01(Vector3.Distance(position, vector) / Mathf.Max(0.0001f, _forceRange));
				float num2 = ((_forceFalloffCurve != null) ? _forceFalloffCurve.Evaluate(1f - num) : (1f - num));
				float num3 = ((_damageFalloffCurve != null) ? _damageFalloffCurve.Evaluate(1f - num) : (1f - num));
				Vector3 vector2 = vector - position;
				if (vector2.sqrMagnitude < 1E-06f)
				{
					vector2 = value.transform.position - position;
				}
				vector2 = vector2.normalized;
				MonoItem component2;
				if (FindComponent<IDamageable>(value.gameObject, out var component) && !list.Contains(component))
				{
					float num4 = _enemiesDamage * num3;
					float force = _forceStrength * num2;
					if (component is PlayerDamageable || num4 > 0f)
					{
						DamageSource source = DamageDataSourceExtensions.ForPlayerAttack(DamageType.Projectile);
						DamageRpcSource rpcSource = DamageDataSourceExtensions.ToRpc(source);
						float stunThrowMultiplier = ((_playerStatesConfiguration != null) ? _playerStatesConfiguration.StunThrowMultiplier : 1f);
						float num5 = (_stunPlayers ? _playerThrowForce : _forceStrength);
						component.ApplyStunHit(num4, vector2, num5 * num2, base.Object.StateAuthority.PlayerId, rpcSource, source, _stunPlayers, stunThrowMultiplier, _playerKnockbackUpBias);
					}
					else if (attachedRigidbody != null)
					{
						component.AddRPCForce(force, vector2, ForceMode.Impulse);
					}
					list.Add(component);
				}
				else if (FindComponent<MonoItem>(value.gameObject, out component2))
				{
					if (!(attachedRigidbody != null))
					{
						continue;
					}
					if (_itemsDamage > 0f)
					{
						if (!_itemCostReduceService.ProcessItemCollisionData(new ItemCollisionData(component2, _itemsDamage, 3f, _collider), base.Object.StateAuthority, isIgnoreLimits: true))
						{
							component2.AddForce(_forceStrength * num2, vector2, ForceMode.Impulse);
						}
					}
					else
					{
						component2.AddForce(_forceStrength * num2, vector2, ForceMode.Impulse);
					}
				}
				else if (attachedRigidbody != null)
				{
					Vector3 force2 = vector2 * _forceStrength * num2;
					attachedRigidbody.AddForce(force2, ForceMode.Impulse);
				}
			}
			QueueDespawnAt(position);
		}

		private void QueueDespawnAt(Vector3 position)
		{
			_pendingDespawnPosition = position;
			_hasPendingDespawn = true;
			if (_collider != null)
			{
				_collider.enabled = false;
			}
			_rigidbody.isKinematic = true;
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			base.transform.position = position;
		}

		private void TryPlayDespawnVfx()
		{
			Vector3 networkedDespawnPosition = NetworkedDespawnPosition;
			_screenShakeService.TriggerScreenShake(CinemachineImpulseSource, ScreenShakeDataOnDestroy);
			if (_destroyParticlePrefab != null)
			{
				UnityEngine.Object.Instantiate(_destroyParticlePrefab, networkedDespawnPosition, Quaternion.identity).Play(withChildren: true);
			}
			_audioService.StartInstanceWith3DAttributes(_destroySoundInstance, _soundSourceBehaviour);
		}

		private ProjectileHitResponse TryDeflect(Collider other)
		{
			if (_rigidbody.isKinematic)
			{
				return ProjectileHitResponse.None;
			}
			Vector3 linearVelocity = _rigidbody.linearVelocity;
			float magnitude = linearVelocity.magnitude;
			if (magnitude < 0.01f)
			{
				return ProjectileHitResponse.None;
			}
			Vector3 direction = linearVelocity / magnitude;
			float num = Mathf.Max(Time.fixedDeltaTime, base.Runner.DeltaTime);
			float travelBack = magnitude * num + _deflectSweepMargin;
			Collider hitCollider = FindNearestSweptSurface(direction, travelBack);
			if (ResolveWithDeflector(hitCollider, direction, travelBack) == ProjectileHitResponse.Deflect)
			{
				return ProjectileHitResponse.Deflect;
			}
			return ResolveWithDeflector(other, direction, travelBack);
		}

		private ProjectileHitResponse ResolveWithDeflector(Collider hitCollider, Vector3 direction, float travelBack)
		{
			if (hitCollider == null)
			{
				return ProjectileHitResponse.None;
			}
			IProjectileDeflector componentInParent = hitCollider.GetComponentInParent<IProjectileDeflector>();
			if (componentInParent == null)
			{
				return ProjectileHitResponse.None;
			}
			ProjectileDeflection deflection;
			ProjectileHitResponse num = componentInParent.ResolveHit(hitCollider, base.transform.position, direction, travelBack, ProjectileRadius, out deflection);
			if (num == ProjectileHitResponse.Deflect)
			{
				ApplyDeflection(deflection, direction);
			}
			return num;
		}

		private void ApplyDeflection(ProjectileDeflection deflection, Vector3 direction)
		{
			Vector3 vector = Vector3.Reflect(direction, deflection.Normal);
			base.transform.SetPositionAndRotation(deflection.Point + deflection.Normal * _deflectClearance, Quaternion.LookRotation(vector));
			_rigidbody.linearVelocity = vector * _rigidbody.linearVelocity.magnitude;
			_rigidbody.angularVelocity = Vector3.zero;
			_deflectorRoot = deflection.DeflectorRoot;
			_deflectShieldedRoot = deflection.ShieldedRoot;
			_deflectIgnoreUntil = Time.time + _deflectIgnoreTime;
			if (!_ricochetSound.IsNull)
			{
				_audioService.PlayOneShot(_ricochetSound, _soundSourceBehaviour);
			}
		}

		private Collider FindNearestSweptSurface(Vector3 direction, float travelBack)
		{
			int num = Physics.SphereCastNonAlloc(base.transform.position - direction * travelBack, ProjectileRadius, direction, _deflectSweepHits, travelBack, DestructionLayers, QueryTriggerInteraction.Ignore);
			Collider result = null;
			float num2 = float.MaxValue;
			float num3 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				Collider collider = _deflectSweepHits[i].collider;
				if (collider == null || collider.attachedRigidbody == _rigidbody || IsDeflectIgnored(collider.transform))
				{
					continue;
				}
				float distance = _deflectSweepHits[i].distance;
				if (collider.GetComponentInParent<IProjectileDeflector>() != null)
				{
					if (distance < num2)
					{
						num2 = distance;
						result = collider;
					}
				}
				else if (distance > 0f && distance < num3)
				{
					num3 = distance;
				}
			}
			if (!(num2 <= num3))
			{
				return null;
			}
			return result;
		}

		private bool IsDeflectIgnored(Transform hit)
		{
			if (Time.time >= _deflectIgnoreUntil)
			{
				return false;
			}
			if (_deflectorRoot != null && hit.IsChildOf(_deflectorRoot))
			{
				return true;
			}
			if (_deflectShieldedRoot != null)
			{
				return hit.IsChildOf(_deflectShieldedRoot);
			}
			return false;
		}

		private bool FindComponent<T>(GameObject other, out T component)
		{
			component = other.GetComponent<T>();
			T val = component;
			if (val == null)
			{
				component = other.GetComponentInParent<T>();
			}
			val = component;
			if (val == null)
			{
				component = other.GetComponentInChildren<T>();
			}
			return component != null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkedDespawnPosition = _NetworkedDespawnPosition;
			HasNetworkedDespawnPosition = _HasNetworkedDespawnPosition;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_NetworkedDespawnPosition = NetworkedDespawnPosition;
			_HasNetworkedDespawnPosition = HasNetworkedDespawnPosition;
		}
	}
}

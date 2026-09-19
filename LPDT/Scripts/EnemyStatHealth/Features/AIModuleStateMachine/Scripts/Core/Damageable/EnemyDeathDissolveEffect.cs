using System;
using System.Collections.Generic;
using FMODUnity;
using Fusion;
using NetworkServices.ObjectsProvider;
using Obi;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	[NetworkBehaviourWeaved(2)]
	public class EnemyDeathDissolveEffect : NetworkBehaviour, IDeferredEnemyDespawn
	{
		[SerializeField]
		private List<Renderer> _renderers;

		[SerializeField]
		private List<ObiRopeExtrudedRenderer> _obiRopeRenderers;

		[SerializeField]
		private List<Collider> _colliders;

		[SerializeField]
		private float _dissolveDuration = 1f;

		[WeaverGenerated]
		[DefaultForProperty("DissolveStartTick", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _DissolveStartTick;

		[WeaverGenerated]
		[DefaultForProperty("DissolveReason", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private EnemyDissolveReason _DissolveReason;

		private readonly List<Collider> _disabledColliders = new List<Collider>();

		private EnemyDissolveEffectConfiguration _enemyDissolveEffectConfiguration;

		private EnemyDissolveVisualController _visualController;

		private bool _isDissolveStartHandled;

		private float _dissolveVisualStartTime;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int DissolveStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemyDeathDissolveEffect.DissolveStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemyDeathDissolveEffect.DissolveStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe EnemyDissolveReason DissolveReason
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemyDeathDissolveEffect.DissolveReason. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (EnemyDissolveReason)Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemyDeathDissolveEffect.DissolveReason. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = (int)value;
			}
		}

		public bool IsDissolving => DissolveStartTick != 0;

		public event Action OnDissolveStarted;

		[Inject]
		public void InjectDependencies(EnemyDissolveEffectConfiguration enemyDissolveEffectConfiguration)
		{
			_enemyDissolveEffectConfiguration = enemyDissolveEffectConfiguration;
		}

		private void Awake()
		{
			_visualController = new EnemyDissolveVisualController(_renderers, _obiRopeRenderers);
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_visualController?.Dispose();
		}

		public override void Spawned()
		{
			base.Spawned();
			_visualController.RestoreAuthoredEdgeColors();
			_isDissolveStartHandled = false;
			foreach (Collider disabledCollider in _disabledColliders)
			{
				if (disabledCollider != null)
				{
					disabledCollider.enabled = true;
				}
			}
			_disabledColliders.Clear();
			_visualController.SetDissolveAmount(0f);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.HasStateAuthority && DissolveStartTick != 0 && GetDissolveElapsedTime() >= _dissolveDuration)
			{
				base.Object.DespawnHierarchy();
			}
		}

		public override void Render()
		{
			if (DissolveStartTick != 0)
			{
				HandleDissolveStartOnce();
				_visualController.SetDissolveAmount(Mathf.Clamp01((Time.time - _dissolveVisualStartTime) / _dissolveDuration));
			}
		}

		public void AddRenderers(IEnumerable<Renderer> renderers)
		{
			_visualController.AddRenderers(renderers);
		}

		public void RemoveRenderers(IEnumerable<Renderer> renderers)
		{
			_visualController.RemoveRenderers(renderers);
		}

		public void AddObiRopeRenderers(IEnumerable<ObiRopeExtrudedRenderer> obiRopeRenderers)
		{
			_visualController.AddObiRopeRenderers(obiRopeRenderers);
		}

		public void RemoveObiRopeRenderers(IEnumerable<ObiRopeExtrudedRenderer> obiRopeRenderers)
		{
			_visualController.RemoveObiRopeRenderers(obiRopeRenderers);
		}

		public void RefreshMaterials()
		{
			if (IsDissolving)
			{
				_visualController.RefreshMaterials();
			}
		}

		public bool TryStartDeferredDespawn(EnemyDissolveReason reason = EnemyDissolveReason.Death)
		{
			if (!base.Object.HasStateAuthority)
			{
				return false;
			}
			if (DissolveStartTick == 0)
			{
				DissolveReason = reason;
				DissolveStartTick = base.Runner.Tick;
				this.OnDissolveStarted?.Invoke();
			}
			return true;
		}

		private void HandleDissolveStartOnce()
		{
			if (!_isDissolveStartHandled)
			{
				_isDissolveStartHandled = true;
				_visualController.RefreshMaterials();
				_dissolveVisualStartTime = Time.time - GetDissolveElapsedTime();
				if (DissolveReason == EnemyDissolveReason.Despawn)
				{
					_visualController.SetDissolveEdgeColor(_enemyDissolveEffectConfiguration.DespawnEdgeColor);
				}
				DisableColliders();
				if (!_enemyDissolveEffectConfiguration.DissolveSound.IsNull)
				{
					RuntimeManager.PlayOneShot(_enemyDissolveEffectConfiguration.DissolveSound, base.transform.position);
				}
			}
		}

		private void DisableColliders()
		{
			foreach (Collider collider in _colliders)
			{
				if (!(collider == null) && collider.enabled)
				{
					collider.enabled = false;
					_disabledColliders.Add(collider);
				}
			}
		}

		private float GetDissolveElapsedTime()
		{
			return (float)((int)base.Runner.Tick - DissolveStartTick) * base.Runner.DeltaTime;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			DissolveStartTick = _DissolveStartTick;
			DissolveReason = _DissolveReason;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_DissolveStartTick = DissolveStartTick;
			_DissolveReason = DissolveReason;
		}
	}
}

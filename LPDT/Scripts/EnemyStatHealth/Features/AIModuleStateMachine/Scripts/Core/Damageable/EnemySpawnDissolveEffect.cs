using System;
using System.Collections.Generic;
using Fusion;
using Obi;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Damageable
{
	[NetworkBehaviourWeaved(2)]
	public class EnemySpawnDissolveEffect : NetworkBehaviour
	{
		[SerializeField]
		private List<Renderer> _renderers;

		[SerializeField]
		private List<ObiRopeExtrudedRenderer> _obiRopeRenderers;

		[SerializeField]
		private float _dissolveDuration = 0.5f;

		[WeaverGenerated]
		[DefaultForProperty("AppearStartTick", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _AppearStartTick;

		[WeaverGenerated]
		[DefaultForProperty("DissolveReason", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private EnemyDissolveReason _DissolveReason;

		private EnemyDissolveEffectConfiguration _enemyDissolveEffectConfiguration;

		private EnemyDissolveVisualController _visualController;

		private bool _isAppearVisualActive;

		private bool _isAppearStartHandled;

		private float _appearVisualStartTime;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int AppearStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnDissolveEffect.AppearStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnDissolveEffect.AppearStartTick. Networked properties can only be accessed when Spawned() has been called.");
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
					throw new InvalidOperationException("Error when accessing EnemySpawnDissolveEffect.DissolveReason. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (EnemyDissolveReason)Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnDissolveEffect.DissolveReason. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = (int)value;
			}
		}

		[Inject]
		public void InjectDependencies(EnemyDissolveEffectConfiguration enemyDissolveEffectConfiguration)
		{
			_enemyDissolveEffectConfiguration = enemyDissolveEffectConfiguration;
		}

		private void Awake()
		{
			_visualController = new EnemyDissolveVisualController(_renderers, _obiRopeRenderers);
			_visualController.RefreshMaterials();
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_visualController?.Dispose();
		}

		public override void Spawned()
		{
			base.Spawned();
			_isAppearVisualActive = false;
			_isAppearStartHandled = false;
			if (AppearStartTick != 0)
			{
				_isAppearVisualActive = true;
				ApplyDissolveEdgeColor();
				_visualController.SetDissolveAmount(1f);
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.Object.HasStateAuthority && AppearStartTick != 0 && GetAppearElapsedTime() >= _dissolveDuration)
			{
				FinishAppearNetworkState();
			}
		}

		public override void Render()
		{
			if (AppearStartTick == 0)
			{
				if (_isAppearVisualActive)
				{
					FinishAppearVisual();
				}
				return;
			}
			_isAppearVisualActive = true;
			HandleAppearStartOnce();
			ApplyDissolveEdgeColor();
			float num = Mathf.Clamp01((Time.time - _appearVisualStartTime) / _dissolveDuration);
			_visualController.SetDissolveAmount(1f - num);
			if (num >= 1f)
			{
				FinishAppearVisual();
			}
		}

		public void StartAppear(EnemyDissolveReason reason = EnemyDissolveReason.Spawn)
		{
			if (base.Object.HasStateAuthority && AppearStartTick == 0)
			{
				DissolveReason = reason;
				AppearStartTick = base.Runner.Tick;
				_isAppearVisualActive = true;
				_isAppearStartHandled = false;
				ApplyDissolveEdgeColor();
				_visualController.SetDissolveAmount(1f);
			}
		}

		private void FinishAppearNetworkState()
		{
			if (base.Object.HasStateAuthority)
			{
				AppearStartTick = 0;
				FinishAppearVisual();
			}
		}

		private void FinishAppearVisual()
		{
			_isAppearVisualActive = false;
			_isAppearStartHandled = false;
			_visualController.SetDissolveAmount(0f);
			_visualController.RestoreAuthoredEdgeColors();
		}

		private void HandleAppearStartOnce()
		{
			if (!_isAppearStartHandled)
			{
				_isAppearStartHandled = true;
				_appearVisualStartTime = Time.time - GetAppearElapsedTime();
			}
		}

		private void ApplyDissolveEdgeColor()
		{
			if (DissolveReason == EnemyDissolveReason.Spawn && _enemyDissolveEffectConfiguration != null)
			{
				_visualController.SetDissolveEdgeColor(_enemyDissolveEffectConfiguration.SpawnEdgeColor);
			}
			else
			{
				_visualController.RestoreAuthoredEdgeColors();
			}
		}

		private float GetAppearElapsedTime()
		{
			return (float)((int)base.Runner.Tick - AppearStartTick) * base.Runner.DeltaTime;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			AppearStartTick = _AppearStartTick;
			DissolveReason = _DissolveReason;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_AppearStartTick = AppearStartTick;
			_DissolveReason = DissolveReason;
		}
	}
}

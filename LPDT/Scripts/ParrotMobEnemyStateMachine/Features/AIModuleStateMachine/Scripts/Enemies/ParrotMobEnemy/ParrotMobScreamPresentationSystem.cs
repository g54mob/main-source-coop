using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.ParrotMobEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class ParrotMobScreamPresentationSystem : MonoSystem
	{
		private const string BarrierSizeProperty = "BarrierSize";

		private const string BarrierDurationProperty = "BarrierDuration";

		private const string BarrierSizeCurveProperty = "BarrierSizeCurve";

		[SerializeField]
		private ParrotMobEnemy _enemy;

		[SerializeField]
		private ParrotMobEnemyContext _context;

		[SerializeField]
		private ParrotMobAudioController _audioController;

		[SerializeField]
		private Transform _vfxAnchor;

		private ParrotMobScreamSettings _screamSettings;

		private GameObject _activeInstance;

		private VisualEffect _visualEffect;

		private bool _subscribed;

		private bool _wasScreaming;

		private float _growElapsed;

		public override bool IsEnabled => true;

		[Inject]
		private void InjectDependencies(ParrotMobScreamSettings screamSettings)
		{
			_screamSettings = screamSettings;
		}

		public override void Enable()
		{
		}

		public override void Disable()
		{
		}

		public override void Clear()
		{
			StopPresentation();
		}

		public override void Spawned()
		{
			base.Spawned();
			_context.AnimationEvent.OnScreamStart += HandleScreamStart;
			_subscribed = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_subscribed)
			{
				_context.AnimationEvent.OnScreamStart -= HandleScreamStart;
				_subscribed = false;
			}
			StopPresentation();
			base.Despawned(runner, hasState);
		}

		private void Update()
		{
			if (base.Initialized && _wasScreaming)
			{
				if (_enemy.VisualState == ParrotMobVisualState.Scream)
				{
					UpdateVfxGrowth();
				}
				else
				{
					StopPresentation();
				}
			}
		}

		private void HandleScreamStart()
		{
			if (!_wasScreaming && _enemy.VisualState == ParrotMobVisualState.Scream)
			{
				_audioController.PlayScream();
				StartVfx();
				_wasScreaming = true;
			}
		}

		private void StartVfx()
		{
			StopVfx();
			Transform transform = ((_vfxAnchor != null) ? _vfxAnchor : base.transform);
			_activeInstance = UnityEngine.Object.Instantiate(_screamSettings.ScreamVfxPrefab, transform.position, transform.rotation, transform);
			_activeInstance.transform.localPosition = Vector3.zero;
			_activeInstance.transform.localRotation = Quaternion.identity;
			_visualEffect = _activeInstance.GetComponentInChildren<VisualEffect>(includeInactive: true);
			_growElapsed = 0f;
			_visualEffect.SetFloat("BarrierDuration", 3600f);
			_visualEffect.SetAnimationCurve("BarrierSizeCurve", AnimationCurve.Constant(0f, 1f, 1f));
			_visualEffect.SetFloat("BarrierSize", 0f);
			_visualEffect.Play();
		}

		private void UpdateVfxGrowth()
		{
			_growElapsed += Time.deltaTime;
			_visualEffect.SetFloat("BarrierSize", CalculateBarrierSize(_growElapsed));
		}

		private float CalculateBarrierSize(float growElapsed)
		{
			float num = _screamSettings.AttractRadius * _screamSettings.ScreamVfxSizeMultiplier;
			if (_screamSettings.ScreamVfxGrowDuration <= 0f)
			{
				return num;
			}
			float t = Mathf.Clamp01(growElapsed / _screamSettings.ScreamVfxGrowDuration);
			return Mathf.Lerp(0f, num, t);
		}

		private void StopPresentation()
		{
			_audioController.StopScream();
			StopVfx();
			_wasScreaming = false;
		}

		private void StopVfx()
		{
			if (_activeInstance != null)
			{
				UnityEngine.Object.Destroy(_activeInstance);
			}
			_activeInstance = null;
			_visualEffect = null;
			_growElapsed = 0f;
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

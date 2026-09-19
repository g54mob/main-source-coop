using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AudioServiceModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.EntitiesSoundOcclusionModule.Scripts.EnemiesSoundOcclusion
{
	[NetworkBehaviourWeaved(0)]
	public class SoundOcclusionMonoSystem : MonoSystem
	{
		[SerializeField]
		private Transform _soundTrackPoint;

		[SerializeField]
		private SoundSourceBehaviour _ownSoundSource;

		private IEnemySoundOcclusionModel _enemySoundOcclusionModel;

		private EntitiesSoundOcclusionModel _entitiesSoundOcclusionModel;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(IEnemySoundOcclusionModel enemySoundOcclusionModel, EntitiesSoundOcclusionModel entitiesSoundOcclusionModel)
		{
			_enemySoundOcclusionModel = enemySoundOcclusionModel;
			_entitiesSoundOcclusionModel = entitiesSoundOcclusionModel;
		}

		private void OnValidate()
		{
			if (_soundTrackPoint == null)
			{
				_soundTrackPoint = base.transform;
			}
		}

		public override void Enable()
		{
			_isEnabled = true;
			_entitiesSoundOcclusionModel.OnEntitiesTriggeredBySound += ProcessTriggeringBySound;
		}

		public override void Disable()
		{
			_isEnabled = false;
			_entitiesSoundOcclusionModel.OnEntitiesTriggeredBySound -= ProcessTriggeringBySound;
			Clear();
		}

		public override void Clear()
		{
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_isEnabled)
			{
				Disable();
			}
			base.Despawned(runner, hasState);
		}

		private void ProcessTriggeringBySound(Vector3 soundPosition, float soundDistance, ISoundSource soundSource, string soundPath)
		{
			if (!IsOwnSound(soundSource))
			{
				float num = soundDistance * _enemySoundOcclusionModel.HearingStrength;
				float num2 = Vector3.Distance(soundPosition, _soundTrackPoint.position);
				if (!(num2 > num))
				{
					float loudness = Mathf.Max(0f, soundDistance - num2);
					int sourceId = soundSource?.ID ?? 0;
					SoundSourceKind sourceKind = soundSource?.Kind ?? SoundSourceKind.Unknown;
					int playerId = soundSource?.AttributedPlayerId ?? (-1);
					_enemySoundOcclusionModel.TriggerEnemyBySound(new HeardSound(soundPosition, loudness, sourceId, sourceKind, playerId, soundPath));
				}
			}
		}

		private bool IsOwnSound(ISoundSource soundSource)
		{
			if (soundSource != null && _ownSoundSource != null)
			{
				return soundSource.ID == _ownSoundSource.ID;
			}
			return false;
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

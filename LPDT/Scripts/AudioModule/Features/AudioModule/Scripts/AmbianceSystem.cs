using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Features.AIModule.Scripts;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class AmbianceSystem : NetworkBehaviour
	{
		private const string INTENSITY_PARAMETER_NAME = "Intencity";

		[SerializeField]
		private EventReference _ambianceReference;

		[SerializeField]
		private EventReference _dangerAmbianceReference;

		[SerializeField]
		private LayerMask _playerLayerMask = 128;

		[SerializeField]
		private float _distanceToTriggerDangerAmbiance = 10f;

		[SerializeField]
		private float _fadeSpeed = 2f;

		private EventInstance _ambianceInstance;

		private EventInstance _dangerAmbianceInstance;

		private EnemyTransformsModel _enemyTransformsModel;

		private PlayerMovableModel _playerMovableModel;

		private float _dangerIntensity;

		private bool _playTriggered;

		[Inject]
		private void InjectDependencies(EnemyTransformsModel enemyTransformsModel, PlayerMovableModel playerMovableModel)
		{
			_enemyTransformsModel = enemyTransformsModel;
			_playerMovableModel = playerMovableModel;
		}

		private void OnTriggerEnter(Collider other)
		{
			if ((_playerLayerMask.value & (1 << other.gameObject.layer)) != 0 && !_playTriggered)
			{
				PlayerCharacterMovableBase componentInParent = other.GetComponentInParent<PlayerCharacterMovableBase>();
				if (!(componentInParent == null) && !(componentInParent.Object == null) && componentInParent.Object.IsValid && componentInParent.HasInputAuthority)
				{
					PlayAmbiance();
				}
			}
		}

		private void PlayAmbiance()
		{
			if (!_playTriggered)
			{
				_playTriggered = true;
				_ambianceInstance = RuntimeManager.CreateInstance(_ambianceReference);
				_dangerAmbianceInstance = RuntimeManager.CreateInstance(_dangerAmbianceReference);
				_ambianceInstance.setParameterByName("Intencity", 1f);
				_dangerAmbianceInstance.setParameterByName("Intencity", 0f);
				_ambianceInstance.start();
				_dangerAmbianceInstance.start();
			}
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			if (_ambianceInstance.isValid())
			{
				_ambianceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				_ambianceInstance.release();
			}
			if (_dangerAmbianceInstance.isValid())
			{
				_dangerAmbianceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				_dangerAmbianceInstance.release();
			}
		}

		private void Update()
		{
			if (_ambianceInstance.isValid() && _dangerAmbianceInstance.isValid() && !(_playerMovableModel.LocalMovable == null))
			{
				float target = ((GetNearestEnemyDistance() <= _distanceToTriggerDangerAmbiance) ? 1f : 0f);
				_dangerIntensity = Mathf.MoveTowards(_dangerIntensity, target, Time.deltaTime * _fadeSpeed);
				_dangerAmbianceInstance.setParameterByName("Intencity", _dangerIntensity);
				_ambianceInstance.setParameterByName("Intencity", 1f - _dangerIntensity);
			}
		}

		private float GetNearestEnemyDistance()
		{
			if (_enemyTransformsModel.Enemyies.Count == 0)
			{
				return float.MaxValue;
			}
			float num = float.MaxValue;
			Vector3 position = _playerMovableModel.LocalMovable.transform.position;
			foreach (KeyValuePair<EnemyType, List<Transform>> enemyie in _enemyTransformsModel.Enemyies)
			{
				foreach (Transform item in enemyie.Value)
				{
					float num2 = Vector3.Distance(item.position, position);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}

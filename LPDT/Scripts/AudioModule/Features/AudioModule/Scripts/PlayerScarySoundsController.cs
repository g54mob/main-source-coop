using System;
using System.Collections;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.LevelGatesModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AudioModule.Scripts
{
	public class PlayerScarySoundsController : MonoBehaviour
	{
		[SerializeField]
		private EventReference _scarySoundsEventReference;

		[SerializeField]
		private float _interval = 5f;

		[SerializeField]
		private float _minInterval = 10f;

		[SerializeField]
		private float _maxInterval = 30f;

		[SerializeField]
		private float _radius = 3f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _playChance = 0.5f;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private LevelGatesModel _levelGatesModel;

		private IAudioService _audioService;

		[Inject]
		private void InjectDependencies(LevelGatesModel levelGatesModel, IAudioService audioService)
		{
			_levelGatesModel = levelGatesModel;
			_audioService = audioService;
		}

		private void Start()
		{
			StartCoroutine(ScarySoundLoop());
		}

		private IEnumerator ScarySoundLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(_minInterval, _maxInterval));
				if (_levelGatesModel.IsLocalPlayerInsideGate && UnityEngine.Random.value <= _playChance)
				{
					Vector3 randomUpperHemispherePoint = GetRandomUpperHemispherePoint(_radius);
					PlaySoundOnPosition(randomUpperHemispherePoint);
				}
			}
		}

		private Vector3 GetRandomUpperHemispherePoint(float radius)
		{
			Vector3 playerPosition = GetPlayerPosition();
			float f = UnityEngine.Random.Range(0f, MathF.PI * 2f);
			float f2 = UnityEngine.Random.Range(0f, MathF.PI / 2f);
			float x = Mathf.Sin(f2) * Mathf.Cos(f);
			float y = Mathf.Cos(f2);
			float z = Mathf.Sin(f2) * Mathf.Sin(f);
			Vector3 vector = new Vector3(x, y, z);
			return playerPosition + vector * radius;
		}

		private void PlaySoundOnPosition(Vector3 position)
		{
			_audioService.PlayOneShot(_scarySoundsEventReference, new GenericSoundSource(position, _soundSourceBehaviour.ID, _soundSourceBehaviour.Kind, _soundSourceBehaviour.AttributedPlayerId));
		}

		private Vector3 GetPlayerPosition()
		{
			return base.transform.position;
		}
	}
}

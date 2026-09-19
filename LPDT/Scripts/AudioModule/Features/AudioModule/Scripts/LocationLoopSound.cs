using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Features.AudioModule.Scripts
{
	public class LocationLoopSound : MonoBehaviour
	{
		[SerializeField]
		private EventReference _beachAmbianceReference;

		[SerializeField]
		private Transform _explicitCenter;

		private EventInstance _beachAmbianceInstance;

		private void Start()
		{
			_beachAmbianceInstance = RuntimeManager.CreateInstance(_beachAmbianceReference);
			_beachAmbianceInstance.start();
			_beachAmbianceInstance.set3DAttributes(((_explicitCenter != null) ? _explicitCenter : base.transform).To3DAttributes());
		}

		private void OnDestroy()
		{
			_beachAmbianceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			_beachAmbianceInstance.release();
		}
	}
}

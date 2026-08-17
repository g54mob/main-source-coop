using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NWH.Common.ShiftingOrigin
{
	public class ShiftingOrigin : MonoBehaviour
	{
		public static ShiftingOrigin Instance;

		public float distanceThreshold = 500f;

		public UnityEvent onBeforeJump = new UnityEvent();

		public UnityEvent onAfterJump = new UnityEvent();

		private Vector3 _totalOffset;

		private Camera _cameraMain;

		private Transform _cameraTransform;

		private Vector3 _cameraPosition;

		private ParticleSystem.Particle[] _particles;

		public Vector3 TotalOffset => _totalOffset;

		private void Awake()
		{
			Instance = this;
			onBeforeJump.AddListener(BeforeJump);
			onAfterJump.AddListener(AfterJump);
		}

		private List<T> FindObjects<T>() where T : Object
		{
			return Object.FindObjectsOfType<T>().ToList();
		}

		private void BeforeJump()
		{
			foreach (Rigidbody item in FindObjects<Rigidbody>())
			{
				item.sleepThreshold = 3.4028235E+38f;
			}
		}

		private void AfterJump()
		{
			foreach (Rigidbody item in FindObjects<Rigidbody>())
			{
				item.sleepThreshold = 0.14f;
			}
			Physics.SyncTransforms();
		}

		private void LateUpdate()
		{
			_cameraMain = Camera.main;
			if (!(_cameraMain == null))
			{
				_cameraTransform = _cameraMain.transform;
				_cameraPosition = _cameraTransform.position;
				if (_cameraPosition.magnitude > distanceThreshold)
				{
					Jump();
				}
			}
		}

		private void Jump()
		{
			onBeforeJump.Invoke();
			_totalOffset += _cameraPosition;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				GameObject[] rootGameObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();
				for (int j = 0; j < rootGameObjects.Length; j++)
				{
					rootGameObjects[j].transform.position -= _cameraPosition;
				}
			}
			foreach (ParticleSystem item in FindObjects<ParticleSystem>())
			{
				ParticleSystem.MainModule main = item.main;
				if (main.simulationSpace != ParticleSystemSimulationSpace.World)
				{
					continue;
				}
				int maxParticles = main.maxParticles;
				if (maxParticles != 0)
				{
					bool isPaused = item.isPaused;
					bool isPlaying = item.isPlaying;
					if (!isPaused)
					{
						item.Pause();
					}
					if (_particles == null || _particles.Length < maxParticles)
					{
						_particles = new ParticleSystem.Particle[maxParticles];
					}
					int particles = item.GetParticles(_particles);
					for (int k = 0; k < particles; k++)
					{
						_particles[k].position -= _cameraPosition;
					}
					item.SetParticles(_particles, particles);
					if (isPlaying)
					{
						item.Play();
					}
				}
			}
			onAfterJump.Invoke();
		}
	}
}

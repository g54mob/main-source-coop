using System.Collections;
using System.Collections.Generic;
using NomadDrive.Features.FloatingOrigin;
using UnityEngine;

namespace NomadDrive.Features.FlyingBirds
{
	public class BirdSpawner : MonoBehaviour, IFloatingOriginShiftable
	{
		[Header("Bird Configuration")]
		[SerializeField]
		private GameObject _birdPrefab;

		[SerializeField]
		private int _birdNumberMax = 5;

		[Header("Speed")]
		[SerializeField]
		private float _birdSpeedMin = 10f;

		[SerializeField]
		private float _birdSpeedMax = 30f;

		[Header("Size")]
		[SerializeField]
		private float _birdSizeMin = 0.3f;

		[SerializeField]
		private float _birdSizeMax = 1f;

		[Header("Animation")]
		[SerializeField]
		private float _birdGlideAnimMin = 1f;

		[SerializeField]
		private float _birdGlideAnimMax = 3f;

		[SerializeField]
		private float _birdFlapAnimMin = 0.5f;

		[SerializeField]
		private float _birdFlapAnimMax = 1.5f;

		[Header("Flight Path")]
		[SerializeField]
		private float _birdPathCircleRandomOffset = 1f;

		[SerializeField]
		private float _birdRadiusMin = 2f;

		[SerializeField]
		private float _birdRadiusMax = 4f;

		[Header("Height")]
		[SerializeField]
		private float _birdVerticalOffsetStart = 1.5f;

		[SerializeField]
		private float _birdFlightHeightChangeMax = 1f;

		[SerializeField]
		private float _birdHeightChangeBounds = 1.5f;

		[Header("Distance Culling")]
		[SerializeField]
		private float _enableDistance = 100f;

		[SerializeField]
		private float _disableDistance = 150f;

		[SerializeField]
		private float _destroyDistance = 300f;

		[SerializeField]
		private float _checkInterval = 1f;

		private readonly List<GameObject> _spawnedBirds = new List<GameObject>();

		private Vector3 _rotationCentre;

		private Camera _mainCamera;

		private Coroutine _distanceCheckCoroutine;

		private bool _hasSpawned;

		private bool _isDestroyed;

		private const int MaxBirdsLimit = 50;

		public Vector3 RotationCentre => _rotationCentre;

		public float BirdSpeedMin => _birdSpeedMin;

		public float BirdSpeedMax => _birdSpeedMax;

		public float BirdGlideAnimMin => _birdGlideAnimMin;

		public float BirdGlideAnimMax => _birdGlideAnimMax;

		public float BirdFlapAnimMin => _birdFlapAnimMin;

		public float BirdFlapAnimMax => _birdFlapAnimMax;

		public float BirdPathCircleRandomOffset => _birdPathCircleRandomOffset;

		public float BirdFlightHeightChangeMax => _birdFlightHeightChangeMax;

		public float BirdHeightChangeBounds => _birdHeightChangeBounds;

		private void Start()
		{
			if (!_hasSpawned)
			{
				_hasSpawned = true;
				_rotationCentre = base.transform.position;
				ValidateParameters();
				SpawnAllBirds();
				if (_birdPrefab != null)
				{
					_birdPrefab.SetActive(value: false);
				}
				_distanceCheckCoroutine = StartCoroutine(DistanceCheckRoutine());
			}
		}

		private void OnEnable()
		{
			FloatingOriginManager.RegisterShiftable(this);
		}

		private void OnDisable()
		{
			FloatingOriginManager.UnregisterShiftable(this);
			if (_distanceCheckCoroutine != null)
			{
				StopCoroutine(_distanceCheckCoroutine);
				_distanceCheckCoroutine = null;
			}
		}

		private void OnDestroy()
		{
			foreach (GameObject spawnedBird in _spawnedBirds)
			{
				if (spawnedBird != null)
				{
					Object.Destroy(spawnedBird);
				}
			}
			_spawnedBirds.Clear();
		}

		public void OnOriginShift(Vector3 delta)
		{
			base.transform.position += delta;
			_rotationCentre += delta;
			foreach (GameObject spawnedBird in _spawnedBirds)
			{
				if (!(spawnedBird == null))
				{
					BirdMovement birdMovement = spawnedBird.GetComponent<BirdMovement>();
					if (birdMovement == null)
					{
						birdMovement = spawnedBird.GetComponentInChildren<BirdMovement>(includeInactive: true);
					}
					if (birdMovement != null)
					{
						birdMovement.OnOriginShift(delta);
					}
				}
			}
		}

		private void ValidateParameters()
		{
			if (_birdNumberMax > 50)
			{
				_birdNumberMax = 50;
			}
			if (_birdNumberMax <= 0)
			{
				_birdNumberMax = 1;
			}
			if (_birdHeightChangeBounds < _birdFlightHeightChangeMax)
			{
				_birdHeightChangeBounds = _birdFlightHeightChangeMax;
			}
			if (_birdFlightHeightChangeMax < _birdVerticalOffsetStart)
			{
				_birdFlightHeightChangeMax = _birdVerticalOffsetStart;
			}
			if (_birdGlideAnimMax <= 0f)
			{
				_birdGlideAnimMax = 1f;
			}
			if (_birdFlapAnimMax <= 0f)
			{
				_birdFlapAnimMax = 0.5f;
			}
		}

		private void SpawnAllBirds()
		{
			if (!(_birdPrefab == null))
			{
				for (int i = 0; i < _birdNumberMax; i++)
				{
					SpawnBird();
				}
			}
		}

		private void SpawnBird()
		{
			GameObject gameObject = Object.Instantiate(position: new Vector3(_rotationCentre.x + Random.Range(_birdRadiusMin, _birdRadiusMax), _rotationCentre.y + Random.Range(0f - _birdVerticalOffsetStart, _birdVerticalOffsetStart), _rotationCentre.z), original: _birdPrefab, rotation: Quaternion.identity, parent: base.transform);
			gameObject.SetActive(value: true);
			BirdSpawner component = gameObject.GetComponent<BirdSpawner>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			BirdSpawner[] componentsInChildren = gameObject.GetComponentsInChildren<BirdSpawner>(includeInactive: true);
			foreach (BirdSpawner birdSpawner in componentsInChildren)
			{
				if (birdSpawner != this)
				{
					Object.Destroy(birdSpawner);
				}
			}
			float num = Random.Range(_birdSizeMin, _birdSizeMax);
			gameObject.transform.localScale = new Vector3(num, num, num);
			BirdMovement birdMovement = gameObject.GetComponent<BirdMovement>();
			if (birdMovement == null)
			{
				birdMovement = gameObject.GetComponentInChildren<BirdMovement>(includeInactive: true);
			}
			if (birdMovement != null)
			{
				birdMovement.Initialize(this);
			}
			else
			{
				birdMovement = gameObject.AddComponent<BirdMovement>();
				birdMovement.Initialize(this);
			}
			_spawnedBirds.Add(gameObject);
		}

		private IEnumerator DistanceCheckRoutine()
		{
			float seconds = Mathf.Max(0.5f, _checkInterval);
			WaitForSeconds wait = new WaitForSeconds(seconds);
			while (true)
			{
				yield return wait;
				UpdateBirdVisibility();
			}
		}

		private void UpdateBirdVisibility()
		{
			_mainCamera = Camera.main;
			if (_mainCamera == null)
			{
				return;
			}
			float sqrMagnitude = (_mainCamera.transform.position - _rotationCentre).sqrMagnitude;
			float num = _enableDistance * _enableDistance;
			float num2 = _disableDistance * _disableDistance;
			float num3 = _destroyDistance * _destroyDistance;
			if (sqrMagnitude > num3)
			{
				if (!_isDestroyed)
				{
					DestroyAllBirds();
					_isDestroyed = true;
				}
				return;
			}
			if (_isDestroyed && sqrMagnitude < num2)
			{
				_isDestroyed = false;
				SpawnAllBirds();
			}
			bool flag = sqrMagnitude < num;
			bool flag2 = sqrMagnitude > num2;
			foreach (GameObject spawnedBird in _spawnedBirds)
			{
				if (!(spawnedBird == null))
				{
					if (flag && !spawnedBird.activeSelf)
					{
						spawnedBird.SetActive(value: true);
					}
					else if (flag2 && spawnedBird.activeSelf)
					{
						spawnedBird.SetActive(value: false);
					}
				}
			}
		}

		private void DestroyAllBirds()
		{
			foreach (GameObject spawnedBird in _spawnedBirds)
			{
				if (spawnedBird != null)
				{
					Object.Destroy(spawnedBird);
				}
			}
			_spawnedBirds.Clear();
		}
	}
}

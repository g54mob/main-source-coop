using System.Collections;
using UnityEngine;

namespace NomadDrive.Features.FlyingBirds
{
	public class BirdMovement : MonoBehaviour
	{
		private BirdSpawner _spawner;

		private Animator _animator;

		private float _baseSpeed;

		private float _currentSpeed;

		private float _targetSpeed;

		private Vector3 _rotationCentre;

		private Vector3 _previousPosition;

		private float _birdHeight;

		private const float FlapSpeedMultiplier = 1.3f;

		private const float GlideSpeedMultiplier = 0.7f;

		private const float SpeedLerpRate = 2f;

		private Coroutine _flapCoroutine;

		private Coroutine _heightCoroutine;

		private Coroutine _initCoroutine;

		private bool _isInitialized;

		private static readonly int AnimOffsetHash = Animator.StringToHash("animOffset");

		private static readonly int AnimSpeedHash = Animator.StringToHash("animSpeed");

		private static readonly int FlapHash = Animator.StringToHash("flap");

		public void Initialize(BirdSpawner spawner)
		{
			_spawner = spawner;
			SetupBird();
			_isInitialized = true;
			if (base.gameObject.activeInHierarchy)
			{
				_initCoroutine = StartCoroutine(DelayedStartCoroutines());
			}
		}

		public void OnOriginShift(Vector3 delta)
		{
			_rotationCentre += delta;
			_previousPosition += delta;
		}

		private void OnEnable()
		{
			if (_isInitialized)
			{
				_previousPosition = base.transform.position;
				StartCoroutines();
			}
		}

		private void OnDisable()
		{
			StopAllBirdCoroutines();
		}

		private void Update()
		{
			if (_isInitialized)
			{
				_currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, 2f * Time.deltaTime);
				base.transform.RotateAround(_rotationCentre, Vector3.up, _currentSpeed * Time.deltaTime);
				Vector3 vector = base.transform.position - _previousPosition;
				if (vector.sqrMagnitude > 0.0001f)
				{
					base.transform.rotation = Quaternion.LookRotation(-vector, Vector3.up);
				}
				_previousPosition = base.transform.position;
			}
		}

		private void SetupBird()
		{
			_animator = GetComponent<Animator>();
			_baseSpeed = Random.Range(_spawner.BirdSpeedMin, _spawner.BirdSpeedMax);
			_currentSpeed = _baseSpeed * 0.7f;
			_targetSpeed = _currentSpeed;
			float birdPathCircleRandomOffset = _spawner.BirdPathCircleRandomOffset;
			_rotationCentre = new Vector3(_spawner.RotationCentre.x + Random.Range(0f - birdPathCircleRandomOffset, birdPathCircleRandomOffset), _spawner.RotationCentre.y + Random.Range(0f - birdPathCircleRandomOffset, birdPathCircleRandomOffset), _spawner.RotationCentre.z + Random.Range(0f - birdPathCircleRandomOffset, birdPathCircleRandomOffset));
			base.transform.RotateAround(_spawner.RotationCentre, Vector3.up, Random.Range(0f, 360f));
			if (_animator != null)
			{
				_animator.SetFloat(AnimOffsetHash, Random.Range(0f, 1f));
				_animator.SetFloat(AnimSpeedHash, Random.Range(1f, 4f));
				_animator.SetBool(FlapHash, value: false);
			}
			_birdHeight = _rotationCentre.y;
			_previousPosition = base.transform.position;
		}

		private IEnumerator DelayedStartCoroutines()
		{
			yield return null;
			StartCoroutines();
			_initCoroutine = null;
		}

		private void StartCoroutines()
		{
			StopBehaviorCoroutines();
			if (base.gameObject.activeInHierarchy)
			{
				_flapCoroutine = StartCoroutine(FlappingRoutine());
				_heightCoroutine = StartCoroutine(HeightChangeRoutine());
			}
		}

		private void StopBehaviorCoroutines()
		{
			if (_flapCoroutine != null)
			{
				StopCoroutine(_flapCoroutine);
				_flapCoroutine = null;
			}
			if (_heightCoroutine != null)
			{
				StopCoroutine(_heightCoroutine);
				_heightCoroutine = null;
			}
		}

		private void StopAllBirdCoroutines()
		{
			StopBehaviorCoroutines();
			if (_initCoroutine != null)
			{
				StopCoroutine(_initCoroutine);
				_initCoroutine = null;
			}
		}

		private IEnumerator FlappingRoutine()
		{
			while (true)
			{
				_targetSpeed = _baseSpeed * 0.7f;
				float seconds = Mathf.Max(0.1f, Random.Range(_spawner.BirdGlideAnimMin, _spawner.BirdGlideAnimMax));
				yield return new WaitForSeconds(seconds);
				_targetSpeed = _baseSpeed * 1.3f;
				if (_animator != null)
				{
					_animator.SetBool(FlapHash, value: true);
					_animator.SetFloat(AnimSpeedHash, Random.Range(2f, 6f));
				}
				float seconds2 = Mathf.Max(0.1f, Random.Range(_spawner.BirdFlapAnimMin, _spawner.BirdFlapAnimMax));
				yield return new WaitForSeconds(seconds2);
				if (_animator != null)
				{
					_animator.SetFloat(AnimOffsetHash, Random.Range(0f, 1f));
					_animator.SetBool(FlapHash, value: false);
					_animator.SetFloat(AnimSpeedHash, Random.Range(2f, 4f));
				}
			}
		}

		private IEnumerator HeightChangeRoutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(Random.Range(1f, 8f));
				float duration = Random.Range(2f, 4f);
				float num = Random.Range(0f - _spawner.BirdFlightHeightChangeMax, _spawner.BirdFlightHeightChangeMax);
				float num2 = _birdHeight + num;
				if (!(Mathf.Abs(num2 - _rotationCentre.y) >= _spawner.BirdHeightChangeBounds))
				{
					_birdHeight = num2;
					float startY = base.transform.position.y;
					float elapsedTime = 0f;
					while (elapsedTime < duration)
					{
						float t = elapsedTime / duration;
						float y = Mathf.Lerp(startY, _birdHeight, t);
						Vector3 position = base.transform.position;
						position.y = y;
						base.transform.position = position;
						elapsedTime += Time.deltaTime;
						yield return null;
					}
					Vector3 position2 = base.transform.position;
					position2.y = _birdHeight;
					base.transform.position = position2;
				}
			}
		}
	}
}

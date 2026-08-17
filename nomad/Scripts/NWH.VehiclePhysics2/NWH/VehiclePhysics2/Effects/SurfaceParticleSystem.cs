using System;
using System.Collections;
using NWH.VehiclePhysics2.GroundDetection;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;

namespace NWH.VehiclePhysics2.Effects
{
	[Serializable]
	public class SurfaceParticleSystem
	{
		[Tooltip("Coefficient by which the lateral slip will be multiplied.\r\nIncrease the value to emit more particles when there is lateral slip (wheel skid / drift).")]
		public float lateralSlipCoeff = 0.5f;

		[Tooltip("Coefficient by which the longitudinal slip will be multiplied.\r\nIncrease the value to emit more particles when there is longitudinal slip (wheel spin).")]
		public float longitudinalSlipCoeff = 0.5f;

		[Tooltip("    Coefficient by which the particle start size will be multiplied.")]
		public float particleSizeCoeff = 1f;

		[Tooltip("    Coefficient by which the emission rate will be multiplied.")]
		public float emissionRateCoeff = 1f;

		[Tooltip("    Makes the particle either emit or not emit with no in-between.")]
		public bool binaryEmission;

		public ParticleSystem particlePS;

		public ParticleSystem chunkPS;

		public GameObject particlePrefab;

		public GameObject chunkPrefab;

		private ParticleSystem.MainModule _mainModule;

		private ParticleSystem.EmissionModule _emissionModule;

		private ParticleSystem.ShapeModule _shapeModule;

		private float _rateOverDistance;

		private float _rateOverTime;

		private float _smokeEmissionRateVelocity;

		private VehicleController _vc;

		private WheelComponent _wheelComponent;

		private Color _particleColor;

		private ParticleSystem.MinMaxGradient _minMaxGradient;

		[SerializeField]
		private SurfacePreset surfacePreset;

		private float _smokeEmissionRate;

		private Coroutine _chunkCoroutine;

		private Coroutine _particleCoroutine;

		public void Initialize(VehicleController vc, WheelComponent wheelComponent)
		{
			_vc = vc;
			_wheelComponent = wheelComponent;
			if (vc.groundDetection.state.isEnabled && !(vc.groundDetection.groundDetectionPreset == null))
			{
				if (_vc.groundDetection.groundDetectionPreset.particlePrefab != null)
				{
					particlePrefab = UnityEngine.Object.Instantiate(_vc.groundDetection.groundDetectionPreset.particlePrefab, wheelComponent.wheelUAPI.transform, worldPositionStays: true);
					particlePrefab.transform.localPosition = -Vector3.up * wheelComponent.wheelUAPI.SpringMaxLength;
					particlePrefab.transform.localRotation = Quaternion.identity;
					particlePS = particlePrefab.GetComponent<ParticleSystem>();
					particlePS.name = "SurfaceParticles";
					ParticleSystem.ShapeModule shape = particlePS.shape;
					shape.radius = wheelComponent.wheelUAPI.Width * 1.5f;
					_shapeModule = particlePS.shape;
					_shapeModule.radius = wheelComponent.wheelUAPI.Width;
				}
				else
				{
					Debug.LogWarning("Smoke Prefab is null, wheel slip will not produce particles.");
				}
				if (_vc.groundDetection.groundDetectionPreset.chunkPrefab != null)
				{
					chunkPrefab = UnityEngine.Object.Instantiate(_vc.groundDetection.groundDetectionPreset.chunkPrefab, wheelComponent.wheelUAPI.transform, worldPositionStays: true);
					chunkPrefab.transform.localPosition = -Vector3.up * (wheelComponent.wheelUAPI.SpringMaxLength + wheelComponent.wheelUAPI.Radius);
					chunkPrefab.transform.localRotation = Quaternion.identity;
					chunkPS = chunkPrefab.GetComponent<ParticleSystem>();
					chunkPS.name = "SurfaceChunks";
					_shapeModule = chunkPS.shape;
					_shapeModule.radius = wheelComponent.wheelUAPI.Width;
				}
				else
				{
					Debug.LogWarning("Dust Prefab is null, there will be no surface dust.");
				}
			}
		}

		public void Enable()
		{
			_particleCoroutine = _vc.StartCoroutine(UpdateParticlesCoroutine());
			_chunkCoroutine = _vc.StartCoroutine(UpdateChunksCoroutine());
			if (particlePS != null)
			{
				particlePS.Play();
			}
			if (chunkPS != null)
			{
				chunkPS.Play();
			}
		}

		public void Disable()
		{
			if (_particleCoroutine != null)
			{
				_vc.StopCoroutine(_particleCoroutine);
			}
			if (_chunkCoroutine != null)
			{
				_vc.StopCoroutine(_chunkCoroutine);
			}
			if (particlePS != null)
			{
				particlePS.Stop();
			}
			if (chunkPS != null)
			{
				chunkPS.Stop();
			}
		}

		private IEnumerator UpdateParticlesCoroutine()
		{
			float dt = 0.05f;
			while (true)
			{
				yield return new WaitForSeconds(dt);
				bool isGrounded = _wheelComponent.wheelUAPI.IsGrounded;
				surfacePreset = _wheelComponent.surfacePreset;
				if (surfacePreset == null || !surfacePreset.emitParticles || !isGrounded)
				{
					StopParticleEmission();
				}
				else
				{
					if (particlePS == null)
					{
						continue;
					}
					_mainModule = particlePS.main;
					_emissionModule = particlePS.emission;
					_mainModule.startColor = surfacePreset.particleColor;
					_mainModule.startSize = surfacePreset.particleSize * particleSizeCoeff;
					float value = surfacePreset.particleLifeDistance / _wheelComponent.wheelUAPI.LongitudinalSpeed;
					value = Mathf.Clamp(value, 2f, surfacePreset.maxParticleLifetime);
					_mainModule.startLifetime = value;
					if (surfacePreset.particleType == SurfacePreset.ParticleType.Smoke)
					{
						if (!_wheelComponent.wheelUAPI.IsSkiddingLaterally && !_wheelComponent.wheelUAPI.IsSkiddingLongitudinally)
						{
							StopParticleEmission();
							continue;
						}
						if (_vc.Speed < 0.5f && _vc.AngularVelocityMagnitude < 0.5f)
						{
							StopParticleEmission();
							continue;
						}
						if (!binaryEmission)
						{
							float num = (_wheelComponent.wheelUAPI.IsSkiddingLaterally ? (_wheelComponent.wheelUAPI.NormalizedLateralSlip * lateralSlipCoeff) : 0f);
							float num2 = (_wheelComponent.wheelUAPI.IsSkiddingLongitudinally ? (_wheelComponent.wheelUAPI.NormalizedLongitudinalSlip * longitudinalSlipCoeff) : 0f);
							float value2 = num + num2;
							value2 = Mathf.Clamp01(value2) * surfacePreset.maxParticleEmissionRateOverDistance;
							_smokeEmissionRate = Mathf.Lerp(_smokeEmissionRate, value2, dt);
						}
						else
						{
							float num3 = (_wheelComponent.wheelUAPI.IsSkiddingLaterally ? lateralSlipCoeff : 0f);
							float num4 = (_wheelComponent.wheelUAPI.IsSkiddingLongitudinally ? longitudinalSlipCoeff : 0f);
							float smokeEmissionRate = Mathf.Clamp01(num3 + num4) * surfacePreset.maxParticleEmissionRateOverDistance;
							_smokeEmissionRate = smokeEmissionRate;
						}
						_particleColor = _mainModule.startColor.color;
						_minMaxGradient = _mainModule.startColor;
						_minMaxGradient.color = new Color(_particleColor.r, _particleColor.g, _particleColor.b, Mathf.Clamp01(_smokeEmissionRate) * surfacePreset.particleMaxAlpha);
						_mainModule.startColor = _minMaxGradient;
						float num5 = Mathf.Clamp01(_vc.Speed * 0.33f);
						_rateOverDistance = num5 * _smokeEmissionRate;
						_rateOverTime = (1f - num5) * _smokeEmissionRate;
						_emissionModule.rateOverDistance = _rateOverDistance * emissionRateCoeff;
						_emissionModule.rateOverTime = _rateOverTime * emissionRateCoeff;
					}
					else
					{
						float num6 = 0f;
						if (_wheelComponent.wheelUAPI.IsGrounded)
						{
							num6 = Mathf.Clamp01(_vc.Speed * 0.125f - 0.05f) * surfacePreset.maxParticleEmissionRateOverDistance;
						}
						_particleColor = _mainModule.startColor.color;
						_minMaxGradient = _mainModule.startColor;
						_minMaxGradient.color = new Color(_particleColor.r, _particleColor.g, _particleColor.b, Mathf.Clamp01(num6 * 2f) * surfacePreset.particleMaxAlpha);
						_mainModule.startColor = _minMaxGradient;
						_emissionModule.rateOverTime = 0f;
						_emissionModule.rateOverDistance = num6 * emissionRateCoeff;
					}
				}
			}
		}

		private IEnumerator UpdateChunksCoroutine()
		{
			while (true)
			{
				yield return new WaitForSeconds(0.1f);
				bool isGrounded = _wheelComponent.wheelUAPI.IsGrounded;
				surfacePreset = _wheelComponent.surfacePreset;
				if (surfacePreset == null || !surfacePreset.emitChunks || !isGrounded)
				{
					StopChunkEmission();
				}
				else if (!(chunkPS == null))
				{
					chunkPS.gameObject.transform.localPosition = -Vector3.up * (_wheelComponent.wheelUAPI.SpringLength + _wheelComponent.wheelUAPI.Radius);
					_mainModule = chunkPS.main;
					_emissionModule = chunkPS.emission;
					float value = surfacePreset.chunkLifeDistance / _wheelComponent.wheelUAPI.LongitudinalSpeed;
					value = Mathf.Clamp(value, 0.2f, surfacePreset.maxChunkLifetime);
					_mainModule.startLifetime = value;
					float outputAngularVelocity = _wheelComponent.outputAngularVelocity;
					if (((outputAngularVelocity < 0f) ? (0f - outputAngularVelocity) : outputAngularVelocity) < 5f)
					{
						_emissionModule.rateOverTime = 0f;
						_emissionModule.rateOverDistance = 0f;
						continue;
					}
					float num = _wheelComponent.outputAngularVelocity * _wheelComponent.wheelUAPI.Radius;
					_mainModule.startSpeed = num * 0.2f;
					_emissionModule.rateOverTime = 0f;
					_emissionModule.rateOverDistance = (_wheelComponent.wheelUAPI.NormalizedLongitudinalSlip * 0.7f + _wheelComponent.wheelUAPI.NormalizedLateralSlip * 0.3f) * surfacePreset.maxChunkEmissionRateOverDistance;
				}
			}
		}

		private void StopParticleEmission()
		{
			if (!(particlePS == null))
			{
				_emissionModule = particlePS.emission;
				_emissionModule.rateOverDistance = 0f;
				_emissionModule.rateOverTime = 0f;
			}
		}

		private void StopChunkEmission()
		{
			if (!(chunkPS == null))
			{
				_emissionModule = chunkPS.emission;
				_emissionModule.rateOverDistance = 0f;
				_emissionModule.rateOverTime = 0f;
			}
		}
	}
}

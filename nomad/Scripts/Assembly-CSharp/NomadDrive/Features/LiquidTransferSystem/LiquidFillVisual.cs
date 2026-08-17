using System;
using EvilCore;
using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidFillVisual : MonoBehaviour
	{
		[SerializeField]
		private MeshRenderer fillRenderer;

		[SerializeField]
		private bool hideWhenEmpty = true;

		[Tooltip("Object-space (local) Y range of the liquid mesh: x = interior bottom, y = brim. Pushed per-object via MaterialPropertyBlock so one shared material serves every container. Use the context-menu 'Auto Fill Y From Mesh Bounds' to read it from the assigned mesh.")]
		[SerializeField]
		private Vector2 fillYMinMax = new Vector2(-0.5f, 0.5f);

		[Tooltip("Object-space XZ center of the liquid column; the world-level fill plane pivots around this axis. Captured by 'Auto Fill Y From Mesh Bounds'.")]
		[SerializeField]
		private Vector2 fillCenterXZ = Vector2.zero;

		[SerializeField]
		private SerializableDictionary<LiquidType, LiquidVisualPreset> liquidTypePresets = new SerializableDictionary<LiquidType, LiquidVisualPreset>
		{
			{
				LiquidType.Water,
				new LiquidVisualPreset
				{
					shallowColor = new Color(0.22f, 0.5f, 0.65f),
					deepColor = new Color(0.04f, 0.22f, 0.38f),
					opacity = 0.25f,
					absorption = 1.5f,
					smoothness = 0.95f,
					specIntensity = 1f,
					foamColor = new Color(0.9f, 0.95f, 1f),
					foamCoverage = 0f,
					bubbleAmount = 0.12f
				}
			},
			{
				LiquidType.Gasoline,
				new LiquidVisualPreset
				{
					shallowColor = new Color(0.85f, 0.62f, 0.15f),
					deepColor = new Color(0.45f, 0.25f, 0.04f),
					opacity = 0.45f,
					absorption = 3f,
					smoothness = 0.9f,
					specIntensity = 1.1f,
					foamColor = new Color(1f, 0.92f, 0.6f),
					foamCoverage = 0f,
					bubbleAmount = 0.05f
				}
			},
			{
				LiquidType.Oil,
				new LiquidVisualPreset
				{
					shallowColor = new Color(0.06f, 0.05f, 0.04f),
					deepColor = new Color(0.02f, 0.02f, 0.02f),
					opacity = 0.95f,
					absorption = 8f,
					smoothness = 1f,
					specIntensity = 1.6f,
					foamColor = new Color(0.12f, 0.12f, 0.12f),
					foamCoverage = 0f,
					bubbleAmount = 0f
				}
			},
			{
				LiquidType.Coffee,
				new LiquidVisualPreset
				{
					shallowColor = new Color(0.3f, 0.16f, 0.07f),
					deepColor = new Color(0.08f, 0.04f, 0.02f),
					opacity = 0.9f,
					absorption = 6f,
					smoothness = 0.7f,
					specIntensity = 0.7f,
					foamColor = new Color(0.72f, 0.52f, 0.32f),
					foamCoverage = 0.55f,
					bubbleAmount = 0.3f
				}
			},
			{
				LiquidType.Milk,
				new LiquidVisualPreset
				{
					shallowColor = new Color(0.95f, 0.93f, 0.88f),
					deepColor = new Color(0.78f, 0.75f, 0.68f),
					opacity = 0.97f,
					absorption = 5f,
					smoothness = 0.35f,
					specIntensity = 0.35f,
					foamColor = new Color(0.98f, 0.97f, 0.92f),
					foamCoverage = 0.22f,
					bubbleAmount = 0.18f
				}
			},
			{
				LiquidType.Coolant,
				new LiquidVisualPreset
				{
					shallowColor = new Color(0.2f, 0.85f, 0.45f),
					deepColor = new Color(0.05f, 0.4f, 0.2f),
					opacity = 0.5f,
					absorption = 2.5f,
					smoothness = 0.9f,
					specIntensity = 1f,
					foamColor = new Color(0.75f, 1f, 0.88f),
					foamCoverage = 0f,
					bubbleAmount = 0.08f
				}
			}
		};

		[Header("Wobble")]
		[SerializeField]
		private bool enableWobble = true;

		[Tooltip("Maximum fill-plane tilt (tangent of the slosh angle).")]
		[SerializeField]
		private float maxWobble = 0.06f;

		[SerializeField]
		private float wobbleFrequency = 4f;

		[SerializeField]
		private float wobbleRecovery = 1.5f;

		[SerializeField]
		private float positionImpulseScale = 0.12f;

		[SerializeField]
		private float rotationImpulseScale = 0.4f;

		private static readonly int FillAmountId = Shader.PropertyToID("_FillAmount");

		private static readonly int LiquidColorId = Shader.PropertyToID("_LiquidColor");

		private static readonly int FillYMinMaxId = Shader.PropertyToID("_FillYMinMax");

		private static readonly int DeepColorId = Shader.PropertyToID("_DeepColor");

		private static readonly int SurfaceAlphaId = Shader.PropertyToID("_SurfaceAlpha");

		private static readonly int AbsorptionId = Shader.PropertyToID("_Absorption");

		private static readonly int SmoothnessId = Shader.PropertyToID("_Smoothness");

		private static readonly int SpecIntensityId = Shader.PropertyToID("_SpecIntensity");

		private static readonly int FoamColorId = Shader.PropertyToID("_FoamColor");

		private static readonly int FoamCoverageId = Shader.PropertyToID("_FoamCoverage");

		private static readonly int BubbleAmountId = Shader.PropertyToID("_BubbleAmount");

		private static readonly int WobbleXId = Shader.PropertyToID("_WobbleX");

		private static readonly int WobbleZId = Shader.PropertyToID("_WobbleZ");

		private static readonly int LiquidSunDirId = Shader.PropertyToID("_LiquidSunDir");

		private static readonly int LiquidSunColorId = Shader.PropertyToID("_LiquidSunColor");

		private const float TeleportSqrDistance = 25f;

		private static int _sunPushedFrame = -1;

		private MaterialPropertyBlock _mpb;

		private LiquidContainerComponent _container;

		private float _wobbleAddX;

		private float _wobbleAddZ;

		private Vector3 _lastPos;

		private Quaternion _lastRot;

		private bool _hasLastTransform;

		private bool _wobbleSleeping = true;

		public void Bind(LiquidContainerComponent container)
		{
			if (fillRenderer == null)
			{
				return;
			}
			if (_container == container)
			{
				Refresh();
				return;
			}
			Unbind();
			_container = container;
			if (_container != null)
			{
				_container.OnLiquidAmountChangedEvent.AddListener(OnLiquidAmountChanged);
			}
			Refresh();
		}

		public void Unbind()
		{
			if (_container != null)
			{
				_container.OnLiquidAmountChangedEvent.RemoveListener(OnLiquidAmountChanged);
				_container = null;
			}
		}

		public void Refresh()
		{
			if (fillRenderer == null)
			{
				return;
			}
			float num = ((_container != null) ? Mathf.Clamp01(_container.FillRatio) : 0f);
			if (_mpb == null)
			{
				_mpb = new MaterialPropertyBlock();
			}
			fillRenderer.GetPropertyBlock(_mpb);
			_mpb.SetFloat(FillAmountId, num);
			_mpb.SetVector(FillYMinMaxId, new Vector4(fillYMinMax.x, fillYMinMax.y, fillCenterXZ.x, fillCenterXZ.y));
			if (_container != null)
			{
				LiquidType currentLiquidType = _container.CurrentLiquidType;
				if (currentLiquidType != LiquidType.Empty && liquidTypePresets.TryGetValue(currentLiquidType, out var value))
				{
					ApplyPreset(value);
				}
			}
			fillRenderer.SetPropertyBlock(_mpb);
			if (hideWhenEmpty)
			{
				fillRenderer.enabled = num > 0f;
			}
		}

		private void ApplyPreset(LiquidVisualPreset preset)
		{
			_mpb.SetColor(LiquidColorId, preset.shallowColor);
			_mpb.SetColor(DeepColorId, preset.deepColor);
			_mpb.SetFloat(SurfaceAlphaId, preset.opacity);
			_mpb.SetFloat(AbsorptionId, preset.absorption);
			_mpb.SetFloat(SmoothnessId, preset.smoothness);
			_mpb.SetFloat(SpecIntensityId, preset.specIntensity);
			_mpb.SetFloat(FoamCoverageId, preset.foamCoverage);
			_mpb.SetFloat(BubbleAmountId, preset.bubbleAmount);
			if (preset.foamCoverage > 0f || preset.bubbleAmount > 0f)
			{
				_mpb.SetColor(FoamColorId, preset.foamColor);
			}
		}

		private void OnLiquidAmountChanged(float oldAmount, float newAmount)
		{
			Refresh();
		}

		private void OnEnable()
		{
			_hasLastTransform = false;
			_wobbleAddX = 0f;
			_wobbleAddZ = 0f;
			_wobbleSleeping = true;
		}

		private void OnDisable()
		{
			Unbind();
		}

		private void Update()
		{
			PushSunGlobals(force: false);
			if (!enableWobble || fillRenderer == null || !fillRenderer.enabled || !fillRenderer.isVisible)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			if (deltaTime <= 0f)
			{
				return;
			}
			Transform obj = fillRenderer.transform;
			Vector3 position = obj.position;
			Quaternion rotation = obj.rotation;
			if (!_hasLastTransform)
			{
				_lastPos = position;
				_lastRot = rotation;
				_hasLastTransform = true;
				return;
			}
			Vector3 vector = position - _lastPos;
			if (vector.sqrMagnitude > 25f)
			{
				_lastPos = position;
				_lastRot = rotation;
				return;
			}
			Vector3 vector2 = vector / deltaTime;
			(rotation * Quaternion.Inverse(_lastRot)).ToAngleAxis(out var angle, out var axis);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			Vector3 vector3 = axis * (angle * ((float)Math.PI / 180f) / deltaTime);
			_lastPos = position;
			_lastRot = rotation;
			_wobbleAddX = Mathf.Lerp(_wobbleAddX, 0f, deltaTime * wobbleRecovery);
			_wobbleAddZ = Mathf.Lerp(_wobbleAddZ, 0f, deltaTime * wobbleRecovery);
			_wobbleAddX = Mathf.Clamp(_wobbleAddX + (vector2.x * positionImpulseScale + vector3.z * rotationImpulseScale) * maxWobble, 0f - maxWobble, maxWobble);
			_wobbleAddZ = Mathf.Clamp(_wobbleAddZ + (vector2.z * positionImpulseScale + vector3.x * rotationImpulseScale) * maxWobble, 0f - maxWobble, maxWobble);
			bool flag = Mathf.Abs(_wobbleAddX) < 0.0001f && Mathf.Abs(_wobbleAddZ) < 0.0001f;
			if (!flag || !_wobbleSleeping)
			{
				_wobbleSleeping = flag;
				float f = (float)Math.PI * 2f * wobbleFrequency * Time.time;
				float value = (flag ? 0f : (_wobbleAddX * Mathf.Sin(f)));
				float value2 = (flag ? 0f : (_wobbleAddZ * Mathf.Cos(f)));
				if (_mpb == null)
				{
					_mpb = new MaterialPropertyBlock();
				}
				fillRenderer.GetPropertyBlock(_mpb);
				_mpb.SetFloat(WobbleXId, value);
				_mpb.SetFloat(WobbleZId, value2);
				fillRenderer.SetPropertyBlock(_mpb);
			}
		}

		private static void PushSunGlobals(bool force)
		{
			if (force || Time.frameCount != _sunPushedFrame)
			{
				_sunPushedFrame = Time.frameCount;
				Light sun = RenderSettings.sun;
				Vector3 vector = ((sun != null) ? (-sun.transform.forward) : new Vector3(0.3f, 0.8f, 0.4f).normalized);
				Color value = ((sun != null) ? sun.color : Color.white) * Mathf.Clamp01(vector.y);
				Shader.SetGlobalVector(LiquidSunDirId, vector);
				Shader.SetGlobalColor(LiquidSunColorId, value);
			}
		}

		private void AutoFillYFromMeshBounds()
		{
			if (!(fillRenderer == null) && fillRenderer.TryGetComponent<MeshFilter>(out var component) && !(component.sharedMesh == null))
			{
				Bounds bounds = component.sharedMesh.bounds;
				fillYMinMax = new Vector2(bounds.min.y, bounds.max.y);
				fillCenterXZ = new Vector2(bounds.center.x, bounds.center.z);
			}
		}
	}
}

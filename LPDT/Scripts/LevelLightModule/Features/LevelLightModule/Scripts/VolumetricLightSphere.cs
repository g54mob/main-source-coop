using UnityEngine;
using UnityEngine.Rendering;

namespace Features.LevelLightModule.Scripts
{
	[ExecuteAlways]
	public class VolumetricLightSphere : MonoBehaviour
	{
		private const string SHADER_NAME = "Custom/VolumetricLightSphereShader";

		private const string GEOMETRY_NAME = "Volumetric Light Sphere Geometry";

		private const float MIN_RADIUS = 0.01f;

		private static readonly int _colorId = Shader.PropertyToID("_Color");

		private static readonly int _intensityId = Shader.PropertyToID("_Intensity");

		private static readonly int _radiusId = Shader.PropertyToID("_Radius");

		private static readonly int _stepCountId = Shader.PropertyToID("_StepCount");

		private static readonly int _shadowMapId = Shader.PropertyToID("_ShadowMap");

		private static readonly int _shadowStrengthId = Shader.PropertyToID("_ShadowStrength");

		private const string SHADOW_KEYWORD = "_VOLUMETRIC_SHADOW_ON";

		[Header("Volume")]
		[SerializeField]
		private Color _color = Color.white;

		[SerializeField]
		[Min(0f)]
		private float _intensity = 1f;

		[SerializeField]
		[Min(0.01f)]
		private float _radius = 2f;

		[SerializeField]
		[Range(2f, 24f)]
		private int _stepCount = 5;

		[Header("Attached Light")]
		[Tooltip("Take the color from the Point Light on this GameObject.")]
		[SerializeField]
		private bool _colorFromLight = true;

		[Tooltip("Multiplier applied to the light's intensity. Negative value uses the manual intensity instead.")]
		[SerializeField]
		private float _intensityMultiplier = 1f;

		[Tooltip("Multiplier applied to the light's range. Negative value uses the manual radius instead.")]
		[SerializeField]
		private float _radiusMultiplier = 1f;

		[SerializeField]
		[HideInInspector]
		private Shader _shader;

		private GameObject _geometry;

		private MeshRenderer _meshRenderer;

		private Material _material;

		private Mesh _proxyMesh;

		private Light _lightAttachedCached;

		private bool _isLightAttachedCached;

		private Texture2D _shadowMap;

		private float _shadowStrength;

		private Texture2D _appliedShadowMap;

		private float _appliedShadowStrength = -1f;

		private Material _appliedMaterial;

		public float Radius => _radius;

		public Light LightAttached
		{
			get
			{
				if (!_isLightAttachedCached)
				{
					InitLightAttachedCached();
				}
				return _lightAttachedCached;
			}
		}

		public bool UseIntensityFromAttachedLight
		{
			get
			{
				if (_intensityMultiplier >= 0f)
				{
					return LightAttached != null;
				}
				return false;
			}
		}

		public bool UseRadiusFromAttachedLight
		{
			get
			{
				if (_radiusMultiplier >= 0f)
				{
					return LightAttached != null;
				}
				return false;
			}
		}

		public bool UseColorFromAttachedLight
		{
			get
			{
				if (_colorFromLight)
				{
					return LightAttached != null;
				}
				return false;
			}
		}

		public void SetShadowMap(Texture2D shadowMap, float strength)
		{
			_shadowMap = shadowMap;
			_shadowStrength = strength;
			if (_material != null)
			{
				ApplyShadowProperties();
			}
		}

		public void AssignPropertiesFromAttachedLight()
		{
			Light lightAttached = LightAttached;
			if (!(lightAttached == null))
			{
				if (UseIntensityFromAttachedLight)
				{
					_intensity = Mathf.Max(lightAttached.intensity * _intensityMultiplier, 0f);
				}
				if (UseRadiusFromAttachedLight)
				{
					_radius = Mathf.Max(lightAttached.range * _radiusMultiplier, 0.01f);
				}
				if (UseColorFromAttachedLight)
				{
					_color = lightAttached.color;
				}
			}
		}

		private void OnEnable()
		{
			InitLightAttachedCached();
			EnsureGeometry();
			AssignPropertiesFromAttachedLight();
			ApplyProperties();
		}

		private void OnDisable()
		{
			if (_geometry != null)
			{
				_geometry.SetActive(value: false);
			}
		}

		private void OnDestroy()
		{
			DestroyImmediateSafe(_material);
			DestroyImmediateSafe(_proxyMesh);
			DestroyImmediateSafe(_geometry);
		}

		private void OnValidate()
		{
			if (_shader == null)
			{
				_shader = Shader.Find("Custom/VolumetricLightSphereShader");
			}
			InitLightAttachedCached();
			if (!(_geometry == null) && !(_material == null))
			{
				AssignPropertiesFromAttachedLight();
				ApplyProperties();
			}
		}

		private void Update()
		{
			if (_geometry == null || _material == null)
			{
				EnsureGeometry();
			}
			AssignPropertiesFromAttachedLight();
			ApplyProperties();
		}

		private void InitLightAttachedCached()
		{
			_isLightAttachedCached = true;
			_lightAttachedCached = GetComponent<Light>();
			if (!(_lightAttachedCached == null) && _lightAttachedCached.type != LightType.Point)
			{
				Debug.LogWarningFormat(this, "Light attached to {0} '{1}' must be a Point light, ignoring it", "VolumetricLightSphere", base.name);
				_lightAttachedCached = null;
			}
		}

		private void EnsureGeometry()
		{
			if (_shader == null)
			{
				_shader = Shader.Find("Custom/VolumetricLightSphereShader");
			}
			if (_geometry == null)
			{
				_geometry = new GameObject("Volumetric Light Sphere Geometry")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				_geometry.transform.SetParent(base.transform, worldPositionStays: false);
			}
			_geometry.SetActive(value: true);
			if (_proxyMesh == null)
			{
				_proxyMesh = CreateProxyCube();
			}
			MeshFilter meshFilter = _geometry.GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = _geometry.AddComponent<MeshFilter>();
			}
			meshFilter.sharedMesh = _proxyMesh;
			if (_meshRenderer == null)
			{
				_meshRenderer = _geometry.GetComponent<MeshRenderer>();
				if (_meshRenderer == null)
				{
					_meshRenderer = _geometry.AddComponent<MeshRenderer>();
				}
			}
			_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			_meshRenderer.receiveShadows = false;
			_meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			_meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			_meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
			_meshRenderer.allowOcclusionWhenDynamic = false;
			if (_material == null)
			{
				_material = new Material(_shader)
				{
					name = "Volumetric Light Sphere",
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			_meshRenderer.sharedMaterial = _material;
		}

		private void ApplyProperties()
		{
			_geometry.transform.localPosition = Vector3.zero;
			_geometry.transform.localRotation = Quaternion.identity;
			_geometry.transform.localScale = Vector3.one * (_radius * 2f);
			_material.SetColor(_colorId, _color);
			_material.SetFloat(_intensityId, _intensity);
			_material.SetFloat(_radiusId, _radius);
			_material.SetInt(_stepCountId, _stepCount);
			ApplyShadowProperties();
		}

		private void ApplyShadowProperties()
		{
			if (!(_appliedShadowMap == _shadowMap) || !Mathf.Approximately(_appliedShadowStrength, _shadowStrength) || !(_appliedMaterial == _material))
			{
				_appliedShadowMap = _shadowMap;
				_appliedShadowStrength = _shadowStrength;
				_appliedMaterial = _material;
				if (_shadowMap == null)
				{
					_material.DisableKeyword("_VOLUMETRIC_SHADOW_ON");
					return;
				}
				_material.SetTexture(_shadowMapId, _shadowMap);
				_material.SetFloat(_shadowStrengthId, _shadowStrength);
				_material.EnableKeyword("_VOLUMETRIC_SHADOW_ON");
			}
		}

		private static Mesh CreateProxyCube()
		{
			Mesh mesh = new Mesh
			{
				name = "Volumetric Light Sphere Proxy",
				hideFlags = HideFlags.HideAndDontSave
			};
			Vector3[] vertices = new Vector3[8]
			{
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f)
			};
			int[] triangles = new int[36]
			{
				0, 2, 1, 0, 3, 2, 5, 6, 4, 4,
				6, 7, 4, 3, 0, 4, 7, 3, 1, 6,
				5, 1, 2, 6, 3, 6, 2, 3, 7, 6,
				4, 1, 5, 4, 0, 1
			};
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.RecalculateBounds();
			return mesh;
		}

		private static void DestroyImmediateSafe(Object target)
		{
			if (!(target == null))
			{
				if (Application.isPlaying)
				{
					Object.Destroy(target);
				}
				else
				{
					Object.DestroyImmediate(target);
				}
			}
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.LevelLightModule.Scripts
{
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightSphere))]
	public class VolumetricLightSphereShadow : MonoBehaviour
	{
		[Tooltip("How dark the occluded part of the volume becomes.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float _strength = 1f;

		[Tooltip("Only colliders on these layers occlude the volume. Keep it as restrictive as possible.")]
		[SerializeField]
		private LayerMask _layerMask = -1;

		[Tooltip("Vertical resolution of the baked map. Horizontal resolution is twice this value.")]
		[SerializeField]
		[Range(4f, 64f)]
		private int _resolution = 16;

		private VolumetricLightSphere _volumetricLightSphere;

		private Texture2D _shadowMap;

		private Color[] _shadowPixels;

		private static bool HasGraphicsDevice => SystemInfo.graphicsDeviceType != GraphicsDeviceType.Null;

		public void BakeShadow()
		{
			if (!HasGraphicsDevice)
			{
				return;
			}
			int num = Mathf.Max(_resolution, 1);
			int num2 = num * 2;
			EnsureShadowMap(num2, num);
			Vector3 position = base.transform.position;
			float radius = _volumetricLightSphere.Radius;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					Vector3 direction = DirectionFromTexel(j, i, num2, num);
					float r = 1f;
					if (Physics.Raycast(position, direction, out var hitInfo, radius, _layerMask, QueryTriggerInteraction.Ignore))
					{
						r = Mathf.Clamp01(hitInfo.distance / radius);
					}
					_shadowPixels[i * num2 + j] = new Color(r, 0f, 0f, 1f);
				}
			}
			_shadowMap.SetPixels(_shadowPixels);
			_shadowMap.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			_volumetricLightSphere.SetShadowMap(_shadowMap, _strength);
		}

		private static Vector3 DirectionFromTexel(int x, int y, int width, int height)
		{
			float num = ((float)x + 0.5f) / (float)width;
			float num2 = ((float)y + 0.5f) / (float)height;
			float f = (num - 0.5f) * 2f * MathF.PI;
			float f2 = num2 * MathF.PI;
			float num3 = Mathf.Sin(f2);
			return new Vector3(num3 * Mathf.Cos(f), Mathf.Cos(f2), num3 * Mathf.Sin(f));
		}

		private void Awake()
		{
			_volumetricLightSphere = GetComponent<VolumetricLightSphere>();
		}

		private void OnEnable()
		{
			if (_volumetricLightSphere == null)
			{
				_volumetricLightSphere = GetComponent<VolumetricLightSphere>();
			}
			BakeShadow();
		}

		private void OnDisable()
		{
			if (_volumetricLightSphere != null)
			{
				_volumetricLightSphere.SetShadowMap(null, 0f);
			}
		}

		private void OnDestroy()
		{
			if (!(_shadowMap == null))
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(_shadowMap);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(_shadowMap);
				}
			}
		}

		private void OnValidate()
		{
		}

		private void EnsureShadowMap(int width, int height)
		{
			if (_shadowMap != null && (_shadowMap.width != width || _shadowMap.height != height))
			{
				UnityEngine.Object.DestroyImmediate(_shadowMap);
				_shadowMap = null;
			}
			if (_shadowMap == null)
			{
				_shadowMap = new Texture2D(width, height, TextureFormat.R8, mipChain: false, linear: true)
				{
					name = "Volumetric Light Sphere Shadow",
					hideFlags = HideFlags.HideAndDontSave,
					wrapModeU = TextureWrapMode.Repeat,
					wrapModeV = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
			}
			if (_shadowPixels == null || _shadowPixels.Length != width * height)
			{
				_shadowPixels = new Color[width * height];
			}
		}
	}
}

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace EvilCore.CustomPass
{
	public class CustomPassPropertyAccessor
	{
		private readonly CustomPassHandle _handle;

		internal CustomPassPropertyAccessor(CustomPassHandle handle)
		{
			_handle = handle;
		}

		public CustomPassPropertyAccessor SetFloat(string property, float value)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (fullScreenMaterial != null)
			{
				fullScreenMaterial.SetFloat(property, value);
			}
			return this;
		}

		public float GetFloat(string property)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (!(fullScreenMaterial != null))
			{
				return 0f;
			}
			return fullScreenMaterial.GetFloat(property);
		}

		public CustomPassPropertyAccessor SetColor(string property, Color value)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (fullScreenMaterial != null)
			{
				fullScreenMaterial.SetColor(property, value);
			}
			return this;
		}

		public Color GetColor(string property)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (!(fullScreenMaterial != null))
			{
				return Color.clear;
			}
			return fullScreenMaterial.GetColor(property);
		}

		public CustomPassPropertyAccessor SetInt(string property, int value)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (fullScreenMaterial != null)
			{
				fullScreenMaterial.SetInt(property, value);
			}
			return this;
		}

		public int GetInt(string property)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (!(fullScreenMaterial != null))
			{
				return 0;
			}
			return fullScreenMaterial.GetInt(property);
		}

		public CustomPassPropertyAccessor SetVector(string property, Vector4 value)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (fullScreenMaterial != null)
			{
				fullScreenMaterial.SetVector(property, value);
			}
			return this;
		}

		public Vector4 GetVector(string property)
		{
			Material fullScreenMaterial = GetFullScreenMaterial();
			if (!(fullScreenMaterial != null))
			{
				return Vector4.zero;
			}
			return fullScreenMaterial.GetVector(property);
		}

		public CustomPassPropertyAccessor SetLayerMask(LayerMask mask)
		{
			DrawRenderersCustomPass drawRenderersCustomPass = _handle?.FindDrawRenderersPass();
			if (drawRenderersCustomPass != null)
			{
				drawRenderersCustomPass.layerMask = mask;
			}
			return this;
		}

		public LayerMask GetLayerMask()
		{
			return (_handle?.FindDrawRenderersPass())?.layerMask ?? default(LayerMask);
		}

		private Material GetFullScreenMaterial()
		{
			return _handle?.FindFullScreenPass()?.fullscreenPassMaterial;
		}
	}
}

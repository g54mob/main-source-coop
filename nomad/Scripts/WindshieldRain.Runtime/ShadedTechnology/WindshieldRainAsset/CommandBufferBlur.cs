using UnityEngine;
using UnityEngine.Rendering;

namespace ShadedTechnology.WindshieldRainAsset
{
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	public class CommandBufferBlur : MonoBehaviour
	{
		public int m_Iterations = 3;

		public float m_BlurStrength = 2f;

		private Shader _Shader;

		private Material _Material;

		private Camera _Camera;

		private CommandBuffer _CommandBuffer;

		private const int _minResolution = 10;

		private Vector2 _ScreenResolution = Vector2.zero;

		private RenderTextureFormat _TextureFormat;

		public bool Initialized => _CommandBuffer != null;

		public void Cleanup()
		{
			if (Initialized)
			{
				_Camera.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, _CommandBuffer);
				_CommandBuffer = null;
				Object.DestroyImmediate(_Material);
			}
		}

		public void OnEnable()
		{
			Cleanup();
			Initialize();
		}

		public void OnDisable()
		{
			Cleanup();
		}

		private void Initialize()
		{
			if (Initialized)
			{
				return;
			}
			if (!_Shader)
			{
				_Shader = Shader.Find("Hidden/SeparableGlassBlur");
				if (!_Shader)
				{
					throw new MissingReferenceException("Unable to find required shader \"Hidden/SeparableGlassBlur\"");
				}
			}
			if (!_Material)
			{
				_Material = new Material(_Shader);
				_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			_Camera = GetComponent<Camera>();
			if (_Camera.allowHDR && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR))
			{
				_TextureFormat = RenderTextureFormat.DefaultHDR;
			}
			_CommandBuffer = new CommandBuffer();
			_CommandBuffer.name = "Blur screen";
			Vector2[] array = new Vector2[m_Iterations];
			int num = 2;
			for (int i = 0; i < m_Iterations; i++)
			{
				array[i] = new Vector2(Screen.width / num, Screen.height / num);
				if (Screen.width / (num * 2) > 10 && Screen.height / (num * 2) > 10)
				{
					num *= 2;
				}
			}
			for (int j = 0; j < m_Iterations; j++)
			{
				int num2 = Shader.PropertyToID("_ScreenCopyTexture");
				_CommandBuffer.GetTemporaryRT(num2, -1, -1, 0, FilterMode.Bilinear, _TextureFormat);
				_CommandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, num2);
				int num3 = Shader.PropertyToID("_Grab" + j + "_Temp1");
				int num4 = Shader.PropertyToID("_Grab" + j + "_Temp2");
				_CommandBuffer.GetTemporaryRT(num3, (int)array[j].x, (int)array[j].y, 0, FilterMode.Bilinear, _TextureFormat);
				_CommandBuffer.GetTemporaryRT(num4, (int)array[j].x, (int)array[j].y, 0, FilterMode.Bilinear, _TextureFormat);
				_CommandBuffer.Blit(num2, num3);
				_CommandBuffer.ReleaseTemporaryRT(num2);
				_CommandBuffer.SetGlobalVector("offsets", new Vector4(m_BlurStrength / array[j].x, 0f, 0f, 0f));
				_CommandBuffer.Blit(num3, num4, _Material);
				_CommandBuffer.SetGlobalVector("offsets", new Vector4(0f, m_BlurStrength / array[j].y, 0f, 0f));
				_CommandBuffer.Blit(num4, num3, _Material);
				_CommandBuffer.SetGlobalTexture("_GrabBlurTexture_" + j, num3);
			}
			_Camera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, _CommandBuffer);
			_ScreenResolution = new Vector2(Screen.width, Screen.height);
		}

		private void OnPreRender()
		{
			if (_ScreenResolution != new Vector2(Screen.width, Screen.height))
			{
				Cleanup();
			}
			Initialize();
		}
	}
}

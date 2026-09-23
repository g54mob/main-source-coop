using System;
using UnityEngine;

namespace Mimicraft.Customization
{
	public abstract class PortraitStudio : MonoBehaviour
	{
		[Tooltip("Portreyi çeken kamera. Boş bırakılırsa bu objenin altında aranır. Ekrana hiç çizmiyor - kapalı tutuluyor ve kare kare isteniyor.")]
		[SerializeField]
		private Camera portraitCamera;

		[Header("Görüntü")]
		[Tooltip("Portrenin piksel boyutu. Kare olması şart değil ama tarayıcıdaki kartlar kare.")]
		[SerializeField]
		[Min(32f)]
		private int width = 256;

		[SerializeField]
		[Min(32f)]
		private int height = 256;

		[Tooltip("Kenar yumuşatma. 1 = kapalı. Küçük bir portrede 4 gözle görülür bir fark yapar ve yalnızca çekim anında maliyeti var.")]
		[SerializeField]
		[Range(1f, 8f)]
		private int antiAliasing = 4;

		[Tooltip("Açıksa kameranın arka planı aşağıdaki renkle silinir. Kapalıysa kameranın kendi ayarı kullanılır - skybox'ı arka plan yapmak istiyorsan bunu kapat.")]
		[SerializeField]
		private bool overrideBackground = true;

		[Tooltip("Arka plan rengi. Alfası 0 ise portre şeffaf çıkar; bunun için kameranın da skybox değil Solid Color ile silmesi gerekir, ki üstteki anahtar bunu yapıyor.")]
		[SerializeField]
		private Color background = new Color(0f, 0f, 0f, 0f);

		[Header("Kadraj")]
		[Tooltip("Modelin kendi ölçüsüne göre kadrajlanması. Kapalıysa kamera bıraktığın yerde kalır.")]
		[SerializeField]
		private bool autoFrame = true;

		[Tooltip("Modelin etrafında bırakılan boşluk. 1 = tam sığar, 1.2 = kenarlarda biraz hava.")]
		[SerializeField]
		[Min(1f)]
		private float framePadding = 1.18f;

		[Tooltip("Kadraj merkezinin dikeyde kaydırılması, model yüksekliğinin oranı olarak. Pozitif değer yukarı bakar - yüzü kadrajın ortasına almak için.")]
		[SerializeField]
		[Range(-0.5f, 0.5f)]
		private float frameHeightBias;

		protected abstract Component Stage { get; }

		protected abstract string MissingStageMessage { get; }

		public bool IsUsable
		{
			get
			{
				Component stage = Stage;
				if (stage == null || portraitCamera == null)
				{
					Debug.LogWarning("[" + GetType().Name + "] '" + base.name + "': " + ((stage == null) ? (MissingStageMessage + " ") : "") + ((portraitCamera == null) ? "Portrait Camera yok. " : "") + "Portre cekilemez.", this);
					return false;
				}
				return true;
			}
		}

		protected virtual void Awake()
		{
			if (portraitCamera == null)
			{
				portraitCamera = GetComponentInChildren<Camera>(includeInactive: true);
			}
			if (portraitCamera != null)
			{
				portraitCamera.enabled = false;
			}
			AudioListener[] componentsInChildren = GetComponentsInChildren<AudioListener>(includeInactive: true);
			foreach (AudioListener audioListener in componentsInChildren)
			{
				if (audioListener != null && audioListener.enabled)
				{
					audioListener.enabled = false;
				}
			}
		}

		public Texture2D Shoot()
		{
			if (!IsUsable)
			{
				return null;
			}
			if (autoFrame)
			{
				Frame();
			}
			RenderTextureDescriptor desc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 24);
			desc.msaaSamples = Mathf.Max(1, antiAliasing);
			desc.sRGB = true;
			RenderTexture temporary = RenderTexture.GetTemporary(desc);
			CameraClearFlags clearFlags = portraitCamera.clearFlags;
			Color backgroundColor = portraitCamera.backgroundColor;
			RenderTexture targetTexture = portraitCamera.targetTexture;
			if (overrideBackground)
			{
				portraitCamera.clearFlags = CameraClearFlags.Color;
				portraitCamera.backgroundColor = background;
			}
			portraitCamera.targetTexture = temporary;
			portraitCamera.Render();
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: false);
			texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			portraitCamera.targetTexture = targetTexture;
			portraitCamera.clearFlags = clearFlags;
			portraitCamera.backgroundColor = backgroundColor;
			RenderTexture.ReleaseTemporary(temporary);
			return texture2D;
		}

		private void Frame()
		{
			if (TryMeasure(out var bounds))
			{
				Vector3 vector = bounds.center + Vector3.up * (bounds.size.y * frameHeightBias);
				float num = bounds.extents.magnitude * framePadding;
				if (portraitCamera.orthographic)
				{
					portraitCamera.orthographicSize = Mathf.Max(0.01f, num);
					portraitCamera.transform.position = vector - portraitCamera.transform.forward * (num * 3f);
					return;
				}
				float num2 = portraitCamera.fieldOfView * (MathF.PI / 180f) * 0.5f;
				float b = Mathf.Atan(Mathf.Tan(num2) * portraitCamera.aspect);
				float num3 = num / Mathf.Sin(Mathf.Min(num2, b));
				portraitCamera.transform.position = vector - portraitCamera.transform.forward * num3;
				portraitCamera.nearClipPlane = Mathf.Max(0.01f, num3 - num * 2f);
				portraitCamera.farClipPlane = num3 + num * 4f;
			}
		}

		private bool TryMeasure(out Bounds bounds)
		{
			bounds = default(Bounds);
			bool flag = false;
			Renderer[] componentsInChildren = Stage.GetComponentsInChildren<Renderer>(includeInactive: false);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (!(renderer == null) && renderer.enabled)
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (flag)
			{
				return bounds.size.sqrMagnitude > 1E-07f;
			}
			return false;
		}
	}
}

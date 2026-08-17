using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwGraduallyFade")]
	[AddComponentMenu("CW/Paint Core/CW Gradually Fade")]
	public class CwGraduallyFade : MonoBehaviour
	{
		[SerializeField]
		private CwPaintableTexture paintableTexture;

		[Range(0f, 1f)]
		[SerializeField]
		private float threshold = 0.02f;

		[SerializeField]
		private float speed = 1f;

		[SerializeField]
		private CwBlendMode blendMode = CwBlendMode.ReplaceOriginal(Vector4.one);

		[SerializeField]
		private Texture blendTexture;

		[SerializeField]
		private CwPaintableTexture blendPaintableTexture;

		[SerializeField]
		private Color blendColor = Color.white;

		[SerializeField]
		private Texture maskTexture;

		[SerializeField]
		private CwPaintableTexture maskPaintableTexture;

		[SerializeField]
		private CwChannel maskChannel;

		[SerializeField]
		private float counter;

		public CwPaintableTexture PaintableTexture
		{
			get
			{
				return paintableTexture;
			}
			set
			{
				paintableTexture = value;
			}
		}

		public float Threshold
		{
			get
			{
				return threshold;
			}
			set
			{
				threshold = value;
			}
		}

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		public CwBlendMode BlendMode
		{
			get
			{
				return blendMode;
			}
			set
			{
				blendMode = value;
			}
		}

		public Texture BlendTexture
		{
			get
			{
				return blendTexture;
			}
			set
			{
				blendTexture = value;
			}
		}

		public CwPaintableTexture BlendPaintableTexture
		{
			get
			{
				return blendPaintableTexture;
			}
			set
			{
				blendPaintableTexture = value;
			}
		}

		public Color BlendColor
		{
			get
			{
				return blendColor;
			}
			set
			{
				blendColor = value;
			}
		}

		public Texture MaskTexture
		{
			get
			{
				return maskTexture;
			}
			set
			{
				maskTexture = value;
			}
		}

		public CwPaintableTexture MaskPaintableTexture
		{
			get
			{
				return maskPaintableTexture;
			}
			set
			{
				maskPaintableTexture = value;
			}
		}

		public CwChannel MaskChannel
		{
			get
			{
				return maskChannel;
			}
			set
			{
				maskChannel = value;
			}
		}

		protected virtual void Update()
		{
			if (!(paintableTexture != null) || !paintableTexture.Activated)
			{
				return;
			}
			if (speed > 0f)
			{
				counter += speed * Time.deltaTime;
			}
			if (!(counter >= threshold))
			{
				return;
			}
			int num = Mathf.FloorToInt(counter * 255f);
			if (num > 0)
			{
				float num2 = (float)num / 255f;
				Texture texture = null;
				Texture texture2 = null;
				counter -= num2;
				if (blendPaintableTexture != null && blendPaintableTexture.Activated)
				{
					texture2 = blendPaintableTexture.Current;
				}
				else if (blendTexture != null)
				{
					texture2 = blendTexture;
				}
				CwCommandFill.Instance.SetState(preview: false, 0);
				CwCommandFill.Instance.SetMaterial(blendMode, texture2, blendColor, Mathf.Min(num2, 1f), Mathf.Min(num2, 1f));
				CwCommand cwCommand = CwPaintableManager.Submit(CwCommandFill.Instance, paintableTexture.Model, paintableTexture);
				if (maskPaintableTexture != null && maskPaintableTexture.Activated)
				{
					texture = maskPaintableTexture.Current;
				}
				else if (maskTexture != null)
				{
					texture = maskTexture;
				}
				cwCommand.LocalMaskTexture = texture;
				cwCommand.LocalMaskChannel = CwCommon.IndexToVector((int)maskChannel);
			}
		}
	}
}

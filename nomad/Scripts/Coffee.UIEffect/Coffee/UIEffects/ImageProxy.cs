using UnityEngine;
using UnityEngine.UI;

namespace Coffee.UIEffects
{
	internal sealed class ImageProxy : GraphicProxy
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeOnLoad()
		{
			GraphicProxy.Register(new ImageProxy());
		}

		protected override bool IsValid(Graphic graphic)
		{
			if (!graphic)
			{
				return false;
			}
			if (graphic is Image)
			{
				return true;
			}
			return false;
		}

		public override bool IsText(Graphic graphic)
		{
			return false;
		}

		public override Vector4 ModifyExpandSize(Graphic graphic, Vector4 expandSize)
		{
			if (graphic is Image { type: Image.Type.Filled, fillMethod: Image.FillMethod.Radial360, fillAmount: <=0.5f } image)
			{
				expandSize[(image.fillOrigin + (image.fillClockwise ? 2 : 0)) % 4] = 0f;
				if (image.fillAmount <= 0.25f)
				{
					expandSize[(image.fillOrigin + 3) % 4] = 0f;
				}
			}
			return expandSize;
		}
	}
}

using UnityEngine;

namespace CW.Common
{
	[ExecuteInEditMode]
	[DefaultExecutionOrder(1000)]
	[HelpURL("https://carloswilkes.com/Documentation/Common#CwRenderTextureManager")]
	[AddComponentMenu("Common/CW Render Texture Manager")]
	public class CwRenderTextureManager : MonoBehaviour
	{
		[SerializeField]
		private int lifetime = 3;

		public int Lifetime
		{
			get
			{
				return lifetime;
			}
			set
			{
				lifetime = value;
			}
		}

		public static RenderTexture GetTemporary(RenderTextureDescriptor desc, string title)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(desc);
			if (temporary.useMipMap != desc.useMipMap)
			{
				temporary.Release();
				temporary.descriptor = desc;
				temporary.Create();
			}
			return temporary;
		}

		public static RenderTexture ReleaseTemporary(RenderTexture renderTexture)
		{
			if (renderTexture != null)
			{
				renderTexture.DiscardContents();
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			return null;
		}
	}
}

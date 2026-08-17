using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwPaintReplaceChannels")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Paint Replace Channels")]
	public class CwPaintReplaceChannels : MonoBehaviour, IHitCoord, IHit
	{
		[SerializeField]
		private CwGroup group;

		[SerializeField]
		private Texture textureR;

		[SerializeField]
		private Texture textureG;

		[SerializeField]
		private Texture textureB;

		[SerializeField]
		private Texture textureA;

		[SerializeField]
		private Vector4 channelR = new Vector4(1f, 0f, 0f, 0f);

		[SerializeField]
		private Vector4 channelG = new Vector4(1f, 0f, 0f, 0f);

		[SerializeField]
		private Vector4 channelB = new Vector4(1f, 0f, 0f, 0f);

		[SerializeField]
		private Vector4 channelA = new Vector4(1f, 0f, 0f, 0f);

		public CwGroup Group
		{
			get
			{
				return group;
			}
			set
			{
				group = value;
			}
		}

		public Texture TextureR
		{
			get
			{
				return textureR;
			}
			set
			{
				textureR = value;
			}
		}

		public Texture TextureG
		{
			get
			{
				return textureG;
			}
			set
			{
				textureG = value;
			}
		}

		public Texture TextureB
		{
			get
			{
				return textureB;
			}
			set
			{
				textureB = value;
			}
		}

		public Texture TextureA
		{
			get
			{
				return textureA;
			}
			set
			{
				textureA = value;
			}
		}

		public Vector4 ChannelR
		{
			get
			{
				return channelR;
			}
			set
			{
				channelR = value;
			}
		}

		public Vector4 ChannelG
		{
			get
			{
				return channelG;
			}
			set
			{
				channelR = value;
			}
		}

		public Vector4 ChannelB
		{
			get
			{
				return channelB;
			}
			set
			{
				channelR = value;
			}
		}

		public Vector4 ChannelA
		{
			get
			{
				return channelA;
			}
			set
			{
				channelR = value;
			}
		}

		public void HandleHitCoord(bool preview, int priority, float pressure, int seed, CwHit hit, Quaternion rotation)
		{
			CwModel componentInParent = hit.Transform.GetComponentInParent<CwModel>();
			if (!(componentInParent != null))
			{
				return;
			}
			List<CwPaintableTexture> list = componentInParent.FindPaintableTextures(group);
			if (list.Count > 0)
			{
				CwCommandReplaceChannels.Instance.SetState(preview, priority);
				CwCommandReplaceChannels.Instance.SetMaterial(textureR, textureG, textureB, textureA, channelR, channelG, channelB, channelA);
				for (int num = list.Count - 1; num >= 0; num--)
				{
					CwPaintableTexture paintableTexture = list[num];
					CwPaintableManager.Submit(CwCommandReplaceChannels.Instance, componentInParent, paintableTexture);
				}
			}
		}
	}
}

using UnityEngine;

namespace PaintCore
{
	[DefaultExecutionOrder(-200)]
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwTextureHash")]
	[AddComponentMenu("CW/Paint Core/CW Texture Hash")]
	public class CwTextureHash : MonoBehaviour
	{
		[SerializeField]
		private Texture texture;

		[SerializeField]
		private CwHash hash;

		public Texture Texture
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
			}
		}

		public CwHash Hash
		{
			get
			{
				return hash;
			}
			set
			{
				hash = value;
				CwSerialization.TryRegister(texture, hash);
			}
		}

		protected virtual void OnEnable()
		{
			CwSerialization.TryRegister(texture, hash);
		}

		protected virtual void OnDestroy()
		{
			CwSerialization.TryRegister(texture, default(CwHash));
		}
	}
}

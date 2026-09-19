using System;
using UnityEngine;

namespace CartoonFX
{
	public class CFXR_ParticleTextFontAsset : ScriptableObject
	{
		public enum LetterCase
		{
			Both = 0,
			Upper = 1,
			Lower = 2
		}

		[Serializable]
		public class Kerning
		{
			public string name = "A";

			public float pre;

			public float post;
		}

		public string CharSequence = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?-.#@$ ";

		public LetterCase letterCase = LetterCase.Upper;

		public Sprite[] CharSprites;

		public Kerning[] CharKerningOffsets;

		private void OnValidate()
		{
			base.hideFlags = HideFlags.None;
			if (CharKerningOffsets == null || CharKerningOffsets.Length != CharSequence.Length)
			{
				CharKerningOffsets = new Kerning[CharSequence.Length];
				for (int i = 0; i < CharKerningOffsets.Length; i++)
				{
					CharKerningOffsets[i] = new Kerning
					{
						name = CharSequence[i].ToString()
					};
				}
			}
		}

		public bool IsValid()
		{
			int num;
			if (!string.IsNullOrEmpty(CharSequence) && CharSprites != null && CharSprites.Length == CharSequence.Length && CharKerningOffsets != null)
			{
				num = ((CharKerningOffsets.Length == CharSprites.Length) ? 1 : 0);
				if (num != 0)
				{
					goto IL_0060;
				}
			}
			else
			{
				num = 0;
			}
			Debug.LogError($"Invalid ParticleTextFontAsset: '{base.name}'\n", this);
			goto IL_0060;
			IL_0060:
			return (byte)num != 0;
		}
	}
}

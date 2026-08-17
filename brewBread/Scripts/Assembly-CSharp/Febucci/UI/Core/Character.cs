using UnityEngine;

namespace Febucci.UI.Core
{
	internal struct Character
	{
		internal bool initialized;

		public float disappearancesMaxDuration;

		public bool isDisappearing;

		public bool wantsToDisappear;

		public float appearancesMaxDuration;

		public int[] indexBehaviorEffects;

		public int[] indexAppearanceEffects;

		public int[] indexDisappearanceEffects;

		public CharacterSourceData sources;

		public CharacterData data;

		public void ResetVertices()
		{
			for (byte b = 0; b < sources.vertices.Length; b++)
			{
				data.vertices[b] = sources.vertices[b];
			}
		}

		public void ResetColors()
		{
			for (byte b = 0; b < sources.colors.Length; b++)
			{
				data.colors[b] = sources.colors[b];
			}
		}

		public void Hide()
		{
			for (byte b = 0; b < sources.vertices.Length; b++)
			{
				data.vertices[b] = Vector3.zero;
			}
		}
	}
}

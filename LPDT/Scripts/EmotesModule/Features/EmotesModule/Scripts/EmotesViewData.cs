using System;
using UnityEngine;

namespace Features.EmotesModule.Scripts
{
	[Serializable]
	public class EmotesViewData<TEnum> where TEnum : Enum
	{
		public TEnum Type;

		public Sprite EmoteSprite;
	}
}

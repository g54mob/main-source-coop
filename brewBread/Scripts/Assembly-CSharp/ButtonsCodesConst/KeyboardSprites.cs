using System;
using Rewired;
using UnityEngine;

namespace ButtonsCodesConst
{
	[CreateAssetMenu(fileName = "KeyboardSprites", menuName = "KeyboardSprites", order = 1)]
	public class KeyboardSprites : ScriptableObject
	{
		[Serializable]
		public struct Key
		{
			public Sprite Sprite;

			public KeyboardKeyCode code;
		}

		public Key[] Keys;

		public Sprite GetSpriteWithCode(KeyboardKeyCode code)
		{
			Key[] keys = Keys;
			for (int i = 0; i < keys.Length; i++)
			{
				Key key = keys[i];
				if (key.code == code)
				{
					return key.Sprite;
				}
			}
			return null;
		}
	}
}

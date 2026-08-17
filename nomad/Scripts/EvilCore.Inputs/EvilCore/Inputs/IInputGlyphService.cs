using System;
using UnityEngine;

namespace EvilCore.Inputs
{
	public interface IInputGlyphService
	{
		InputDeviceFamily ActiveDeviceFamily { get; }

		event Action OnGlyphsChanged;

		Sprite GetSpriteForAction(string rewiredActionName);

		Sprite GetSpriteForElement(string elementIdentifierName);

		void InvalidateCache();
	}
}

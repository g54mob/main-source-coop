using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zorro.ControllerSupport;
using Zorro.Core;

[CreateAssetMenu(fileName = "ControllerGlyphs", menuName = "Landfall/ControllerGlyphs", order = 1)]
public class ControllerGlyphs : SingletonAsset<ControllerGlyphs>
{
	public enum GlyphType
	{
		Interact = 0,
		Drop = 1,
		UseItem = 2,
		SecondaryUseItem = 3,
		SelfieMode = 4,
		ZoomIn = 5,
		ZoomOut = 6,
		SelectItem1 = 7,
		SelectItem2 = 8,
		SelectItem3 = 9,
		Jump = 10,
		PushToTalk = 11
	}

	[Serializable]
	public class Glyph
	{
		public Texture2D xboxGlyph;

		public Texture2D dualsenseGlyph;

		public Texture2D dualshockGlyph;

		public Texture2D steamDeckGlyph;

		public Texture2D switchGlyph;

		public Texture2D switch2Glyph;

		public IEnumerable<Texture2D> GetAllTextures()
		{
			yield return xboxGlyph;
			yield return dualsenseGlyph;
			yield return dualshockGlyph;
			yield return steamDeckGlyph;
			yield return switchGlyph;
			yield return switch2Glyph;
		}
	}

	public Texture2D[] ExtraGlyphs;

	public Glyph InteractGlyph;

	public Glyph DropGlyph;

	public Glyph UseItemGlyph;

	public Glyph SecondaryUseItemGlyph;

	public Glyph SelfieModeGlyph;

	[FormerlySerializedAs("ZoomGlyph")]
	public Glyph ZoomInGlyph;

	public Glyph ZoomOutGlyph;

	public Glyph SelectItem1Glyph;

	public Glyph SelectItem2Glyph;

	public Glyph SelectItem3Glyph;

	public Glyph JumpGlyph;

	public Glyph PushToTalk;

	public int GetGlyphIndex(GlyphType type)
	{
		int num = 6;
		int controllerTypeOffset = GetControllerTypeOffset();
		return (int)type * num + controllerTypeOffset + ExtraGlyphs.Length;
	}

	private int GetControllerTypeOffset()
	{
		return InputHandler.GetGamepadType() switch
		{
			GamepadType.Xbox => 0, 
			GamepadType.Dualsense => 1, 
			GamepadType.Dualshock => 2, 
			GamepadType.SteamDeck => 3, 
			GamepadType.Switch => 4, 
			GamepadType.SwitchPro => 4, 
			GamepadType.Switch2 => 5, 
			_ => 0, 
		};
	}

	public string GetGlyphText(IMKbPromptProvider promptProvider, GlyphType glyphType)
	{
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			int glyphIndex = GetGlyphIndex(glyphType);
			return $"<sprite index={glyphIndex} color=#FFD79B>";
		}
		return promptProvider.GetPrompt();
	}

	public static string GetSprite(int index)
	{
		return $"<sprite index={index} color=#FFD79B>";
	}
}

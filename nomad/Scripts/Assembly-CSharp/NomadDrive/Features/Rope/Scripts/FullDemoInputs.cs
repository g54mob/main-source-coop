using UnityEngine;
using UnityEngine.InputSystem;

namespace NomadDrive.Features.Rope.Scripts
{
	public class FullDemoInputs
	{
		public enum DemoInputs
		{
			Shoot = 0,
			ShootRay = 1,
			Patch = 2,
			RemoveImpact = 3,
			ChangeFOVEnabled = 4,
			LookAroundEnabled = 5,
			IncreaseRopeLength = 6,
			DecreaseRopeLength = 7,
			MoveLeft = 8,
			MoveRight = 9,
			MouseLookEnabled = 10,
			Quit = 11,
			ShowControls = 12,
			MoveRopesEnabled = 13
		}

		public static Vector2 GetMousePosition()
		{
			return Mouse.current.position.ReadValue();
		}

		public static float GetMouseDelta(string dir)
		{
			string text = dir.ToLower();
			if (!(text == "x"))
			{
				if (text == "y")
				{
					return Mouse.current.delta.y.ReadValue();
				}
				return 0f;
			}
			return Mouse.current.delta.x.ReadValue();
		}

		public static float GetMouseScrollVertical()
		{
			return Mouse.current.scroll.ReadValue().normalized.y;
		}

		public static bool GetInput(DemoInputs input)
		{
			switch (input)
			{
			case DemoInputs.Shoot:
				return Mouse.current.leftButton.wasPressedThisFrame;
			case DemoInputs.ShootRay:
				return Keyboard.current.spaceKey.wasPressedThisFrame;
			case DemoInputs.Patch:
				return Mouse.current.rightButton.wasPressedThisFrame;
			case DemoInputs.RemoveImpact:
				return Keyboard.current.rKey.wasPressedThisFrame;
			case DemoInputs.ChangeFOVEnabled:
				return Keyboard.current.vKey.wasPressedThisFrame;
			case DemoInputs.LookAroundEnabled:
				return Mouse.current.leftButton.isPressed;
			case DemoInputs.IncreaseRopeLength:
				return Keyboard.current.oem2Key.wasPressedThisFrame;
			case DemoInputs.DecreaseRopeLength:
				return Keyboard.current.oem1Key.wasPressedThisFrame;
			case DemoInputs.MoveLeft:
				if (!Keyboard.current.leftArrowKey.wasPressedThisFrame)
				{
					return Keyboard.current.lKey.wasPressedThisFrame;
				}
				return true;
			case DemoInputs.MoveRight:
				if (!Keyboard.current.rightArrowKey.wasPressedThisFrame)
				{
					return Keyboard.current.dKey.wasPressedThisFrame;
				}
				return true;
			case DemoInputs.MouseLookEnabled:
				return Keyboard.current.f4Key.wasPressedThisFrame;
			case DemoInputs.Quit:
				return Keyboard.current.escapeKey.wasPressedThisFrame;
			case DemoInputs.ShowControls:
				return Keyboard.current.f1Key.wasPressedThisFrame;
			case DemoInputs.MoveRopesEnabled:
				return Keyboard.current.mKey.isPressed;
			default:
				return false;
			}
		}
	}
}

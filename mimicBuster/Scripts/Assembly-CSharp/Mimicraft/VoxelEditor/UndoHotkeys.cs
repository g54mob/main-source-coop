using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	public class UndoHotkeys : MonoBehaviour
	{
		private void Update()
		{
			if (Keyboard.current == null || (!Keyboard.current.leftCtrlKey.isPressed && !Keyboard.current.rightCtrlKey.isPressed) || VoxelFocusManager.IsAnyGestureActive())
			{
				return;
			}
			if (Keyboard.current.zKey.wasPressedThisFrame)
			{
				if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
				{
					UndoManager.Redo();
				}
				else
				{
					UndoManager.Undo();
				}
			}
			else if (Keyboard.current.yKey.wasPressedThisFrame)
			{
				UndoManager.Redo();
			}
		}
	}
}

using Mimicraft.UI;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class CursorDriver : MonoBehaviour
	{
		private EyedropperPreviewView eyedropperPreview;

		private bool warnedAboutMissingCanvas;

		private void Update()
		{
			if (VoxelEditorSettings.IsMovementMode)
			{
				HideEyedropperPreview();
				return;
			}
			if (GameMenuState.IsMenuOpen)
			{
				CursorManager.ResetToDefault();
				HideEyedropperPreview();
				return;
			}
			VoxelEditorController focused = VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera);
			if (focused == null)
			{
				CursorManager.ResetToDefault();
				HideEyedropperPreview();
				return;
			}
			if (focused.IsEyedropperActive)
			{
				CursorManager.Set(CursorManager.Kind.Eyedropper);
				UpdateEyedropperPreview(focused);
				return;
			}
			HideEyedropperPreview();
			switch (focused.State)
			{
			case EditorState.Transform:
				UpdateTransformCursor(focused);
				break;
			case EditorState.Extrude:
				CursorManager.Set(CursorManager.Kind.Extrude);
				break;
			case EditorState.Paint:
				CursorManager.Set(CursorManager.Kind.Paint, focused.PaintColor);
				break;
			case EditorState.LoopCut:
				CursorManager.Set(CursorManager.Kind.LoopCut);
				break;
			default:
				CursorManager.ResetToDefault();
				break;
			}
		}

		private void OnDisable()
		{
			CursorManager.ResetToDefault();
			HideEyedropperPreview();
		}

		private void UpdateEyedropperPreview(VoxelEditorController focused)
		{
			if (ResolveEyedropperPreview())
			{
				if (focused.TryPeekEyedropperColor(out var color) && PointerScreenPosition.TryGet(out var position))
				{
					eyedropperPreview.Show(focused.PaintColor, color, position);
				}
				else
				{
					eyedropperPreview.Hide();
				}
			}
		}

		private void HideEyedropperPreview()
		{
			if (eyedropperPreview != null)
			{
				eyedropperPreview.Hide();
			}
		}

		private bool ResolveEyedropperPreview()
		{
			if (eyedropperPreview != null)
			{
				return true;
			}
			Canvas canvas = FindEditorCanvas();
			if (canvas == null)
			{
				if (!warnedAboutMissingCanvas)
				{
					warnedAboutMissingCanvas = true;
					Debug.LogWarning("[CursorDriver] Editor UI canvas'i bulunamadi - eyedropper renk onizlemesi olusturulamiyor. Editor sahnesi yuklu mu?", this);
				}
				return false;
			}
			eyedropperPreview = EyedropperPreviewView.Create(canvas.transform);
			return true;
		}

		private static Canvas FindEditorCanvas()
		{
			PaintPanelView paintPanelView = Object.FindFirstObjectByType<PaintPanelView>(FindObjectsInactive.Include);
			if (paintPanelView != null)
			{
				Canvas componentInParent = paintPanelView.GetComponentInParent<Canvas>(includeInactive: true);
				if (componentInParent != null)
				{
					return componentInParent;
				}
			}
			return Object.FindFirstObjectByType<Canvas>();
		}

		private static void UpdateTransformCursor(VoxelEditorController focused)
		{
			if (focused.IsTransformDragging)
			{
				CursorManager.Set(CursorManager.Kind.TransformDrag);
			}
			else if (focused.IsTransformHoveringEdge)
			{
				CursorManager.Set(CursorManager.Kind.TransformEdge);
			}
			else if (focused.IsTransformHoveringAxis)
			{
				CursorManager.Set(CursorManager.Kind.TransformAxis, focused.TransformHoveredAxisTint);
			}
			else
			{
				CursorManager.Set(CursorManager.Kind.TransformIdle);
			}
		}
	}
}

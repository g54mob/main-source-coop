using System;
using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class TemplateSession
	{
		public static bool SupportsTemplates(VoxelEditorController controller)
		{
			if (!VoxelEditorSettings.SinglePieceEditing)
			{
				return CategoryOf(controller) != null;
			}
			return false;
		}

		public static string CategoryOf(VoxelEditorController controller)
		{
			if (controller == null || controller.Model == null)
			{
				return null;
			}
			IVoxelEditBounds editBounds = controller.Model.EditBounds;
			if (editBounds == null)
			{
				return "";
			}
			if (!string.IsNullOrEmpty(editBounds.CategoryId))
			{
				return editBounds.CategoryId;
			}
			return null;
		}

		public static bool IsSlot(string category)
		{
			return !string.IsNullOrEmpty(category);
		}

		public static bool HasOpenTemplate(string category)
		{
			if (TemplateStorage.Exists(VoxelEditorSettings.CurrentTemplateFilePath))
			{
				return string.Equals(VoxelEditorSettings.CurrentTemplateCategory ?? "", category ?? "", StringComparison.Ordinal);
			}
			return false;
		}

		public static bool SaveCurrent(string name, string tag, bool asNew = false)
		{
			VoxelEditorController focused = VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera);
			string text = CategoryOf(focused);
			List<TemplatePiece> list = GatherFor(focused);
			if (list.Count == 0)
			{
				return false;
			}
			string filePath = ((!asNew && HasOpenTemplate(text)) ? VoxelEditorSettings.CurrentTemplateFilePath : null);
			float voxelSize = ((focused != null && focused.Model != null) ? focused.Model.VoxelSize : 1f);
			VoxelEditorSettings.CurrentTemplateFilePath = TemplateStorage.SaveTemplate(filePath, name, list, text, tag, voxelSize);
			VoxelEditorSettings.CurrentTemplateName = name;
			VoxelEditorSettings.CurrentTemplateTag = tag ?? "";
			VoxelEditorSettings.CurrentTemplateCategory = text ?? "";
			TemplatePortraitService.RequestFor(VoxelEditorSettings.CurrentTemplateFilePath);
			return true;
		}

		public static bool LoadEntry(TemplateListEntry entry, Camera cam)
		{
			TemplateModel templateModel = TemplateStorage.LoadTemplate(entry.FilePath);
			if (templateModel == null)
			{
				return false;
			}
			if (!LoadFor(VoxelFocusManager.GetFocused(cam), templateModel, cam))
			{
				return false;
			}
			UndoManager.Clear();
			VoxelEditorSettings.CurrentTemplateFilePath = entry.FilePath;
			VoxelEditorSettings.CurrentTemplateName = entry.ModelName;
			VoxelEditorSettings.CurrentTemplateTag = entry.Tag;
			VoxelEditorSettings.CurrentTemplateCategory = entry.Category;
			return true;
		}

		public static void DeleteEntry(string filePath)
		{
			SavedThumbnails.Delete(filePath);
			TemplateStorage.DeleteTemplate(filePath);
			if (filePath == VoxelEditorSettings.CurrentTemplateFilePath)
			{
				ForgetOpenTemplate();
			}
		}

		public static void ForgetOpenTemplate()
		{
			VoxelEditorSettings.CurrentTemplateFilePath = null;
			VoxelEditorSettings.CurrentTemplateName = "";
			VoxelEditorSettings.CurrentTemplateTag = "";
			VoxelEditorSettings.CurrentTemplateCategory = "";
		}

		public static List<TemplatePiece> GatherFor(VoxelEditorController subject)
		{
			string text = CategoryOf(subject);
			if (text == null)
			{
				return new List<TemplatePiece>();
			}
			if (!IsSlot(text))
			{
				return Gather();
			}
			return new List<TemplatePiece>
			{
				new TemplatePiece(subject.Model.DisplayName, subject.Model.Grid)
			};
		}

		public static List<TemplatePiece> Gather()
		{
			List<TemplatePiece> list = new List<TemplatePiece>();
			Transform transform = null;
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (!(CategoryOf(item) != ""))
				{
					Transform transform2 = item.transform;
					if (transform == null)
					{
						transform = transform2;
					}
					Quaternion quaternion = Quaternion.Inverse(transform.localRotation);
					Vector3 localPosition = quaternion * (transform2.localPosition - transform.localPosition);
					Quaternion localRotation = quaternion * transform2.localRotation;
					list.Add(new TemplatePiece(item.Model.DisplayName, item.Model.Grid, localPosition, localRotation));
				}
			}
			return list;
		}

		public static bool LoadFor(VoxelEditorController subject, TemplateModel data, Camera cam)
		{
			string text = CategoryOf(subject);
			if (text == null || data == null)
			{
				return false;
			}
			if (!IsSlot(text))
			{
				return Load(data, cam);
			}
			return LoadIntoSlot(subject, data);
		}

		private static bool LoadIntoSlot(VoxelEditorController slot, TemplateModel model)
		{
			if (model.Pieces.Count == 0)
			{
				return false;
			}
			TemplateStorage.ApplyPiece(model.Pieces[0], slot.Model.Grid);
			slot.Model.RebuildMesh();
			return true;
		}

		public static bool Load(TemplateModel data, Camera cam)
		{
			if (data == null)
			{
				return false;
			}
			VoxelEditorController original = VoxelFocusManager.GetOriginal();
			if (!SupportsTemplates(original))
			{
				return false;
			}
			List<TemplatePiece> pieces = data.Pieces;
			if (pieces.Count == 0)
			{
				return false;
			}
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (item != original)
				{
					item.gameObject.SetActive(value: false);
				}
			}
			original.gameObject.SetActive(value: true);
			TemplateStorage.ApplyPiece(pieces[0], original.Model.Grid);
			original.Model.DisplayName = (string.IsNullOrWhiteSpace(pieces[0].Name) ? null : pieces[0].Name);
			original.Model.RebuildMesh();
			for (int i = 1; i < pieces.Count; i++)
			{
				Rebuild(pieces[i], original.Model, cam);
			}
			VoxelFocusManager.SetStickyFocus(original);
			return true;
		}

		private static void Rebuild(TemplatePiece piece, VoxelModel prototype, Camera cam)
		{
			VoxelGrid voxelGrid = new VoxelGrid();
			TemplateStorage.ApplyPiece(piece, voxelGrid);
			GameObject gameObject = VoxelSplitFactory.CreateSplitPiece(prototype, voxelGrid.Voxels, cam, voxelGrid.FaceColors);
			VoxelModel component = gameObject.GetComponent<VoxelModel>();
			if (component != null && !string.IsNullOrWhiteSpace(piece.Name))
			{
				component.DisplayName = piece.Name;
			}
			Transform transform = prototype.transform;
			Transform transform2 = gameObject.transform;
			transform2.localPosition = transform.localPosition + transform.localRotation * piece.LocalPosition;
			transform2.localRotation = transform.localRotation * piece.LocalRotation;
		}
	}
}

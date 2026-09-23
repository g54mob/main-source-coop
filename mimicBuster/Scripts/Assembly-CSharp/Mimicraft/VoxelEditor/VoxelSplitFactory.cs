using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class VoxelSplitFactory
	{
		public static GameObject CreateSplitPiece(VoxelModel sourceModel, IEnumerable<KeyValuePair<Vector3Int, VoxelData>> voxels, Camera cam, IEnumerable<KeyValuePair<(Vector3Int Position, int Face), Color32>> faceColors = null)
		{
			GameObject gameObject = new GameObject(sourceModel.gameObject.name + " (Split)");
			gameObject.layer = sourceModel.gameObject.layer;
			Transform transform = gameObject.transform;
			transform.SetParent(sourceModel.transform.parent, worldPositionStays: false);
			transform.position = sourceModel.transform.position;
			transform.rotation = sourceModel.transform.rotation;
			transform.localScale = sourceModel.transform.localScale;
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			gameObject.AddComponent<MeshCollider>();
			VoxelModel voxelModel = gameObject.AddComponent<VoxelModel>();
			voxelModel.ConfigureVoxelSize(sourceModel.VoxelSize);
			voxelModel.ConfigureMaterials(sourceModel.UnlitMaterial, sourceModel.LitMaterial);
			voxelModel.SetUnlit(VoxelEditorSettings.UnlitMode);
			voxelModel.Grid.Clear();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in voxels)
			{
				voxelModel.Grid.Set(voxel.Key, voxel.Value);
			}
			if (faceColors != null)
			{
				foreach (KeyValuePair<(Vector3Int, int), Color32> faceColor in faceColors)
				{
					voxelModel.Grid.SetFaceColor(faceColor.Key.Item1, faceColor.Key.Item2, faceColor.Value);
				}
			}
			voxelModel.RebuildMesh();
			VoxelEditorController voxelEditorController = gameObject.AddComponent<VoxelEditorController>();
			voxelEditorController.SetCamera(cam);
			VoxelEditorController component = sourceModel.GetComponent<VoxelEditorController>();
			if (component != null)
			{
				voxelEditorController.GizmoSize = component.GizmoSize;
				voxelEditorController.ExtrudeGizmoSize = component.ExtrudeGizmoSize;
				voxelEditorController.GizmoThickness = component.GizmoThickness;
				voxelEditorController.ShowGridOverlay = component.ShowGridOverlay;
			}
			return gameObject;
		}

		public static GameObject CreateDuplicate(VoxelModel sourceModel, Camera cam)
		{
			GameObject gameObject = CreateSplitPiece(sourceModel, sourceModel.Grid.Voxels, cam, sourceModel.Grid.FaceColors);
			if (GridBounds.TryComputeExtent(sourceModel.Grid, out var extent))
			{
				Vector3 vector = new Vector3(extent.x + 1, 0f, 0f);
				gameObject.transform.position += sourceModel.transform.TransformVector(vector);
			}
			gameObject.name = sourceModel.gameObject.name + " (Copy)";
			gameObject.GetComponent<VoxelModel>().DisplayName = sourceModel.DisplayName + " (Copy)";
			return gameObject;
		}
	}
}

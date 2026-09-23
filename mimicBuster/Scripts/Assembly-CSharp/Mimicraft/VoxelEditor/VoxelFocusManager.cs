using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mimicraft.VoxelEditor
{
	public static class VoxelFocusManager
	{
		private const float MaxRayDistance = 1000f;

		private const float GrabThreshold = 6f;

		private static readonly List<VoxelEditorController> registered = new List<VoxelEditorController>();

		private static int cachedFrame = -1;

		private static VoxelEditorController frameResult;

		private static VoxelEditorController stickyFocused;

		public static void Register(VoxelEditorController controller)
		{
			if (!registered.Contains(controller))
			{
				registered.Add(controller);
			}
		}

		public static void Unregister(VoxelEditorController controller)
		{
			registered.Remove(controller);
			if (stickyFocused == controller)
			{
				stickyFocused = null;
			}
		}

		public static IEnumerable<VoxelEditorController> GetAllUsable()
		{
			foreach (VoxelEditorController item in registered)
			{
				if (IsUsable(item))
				{
					yield return item;
				}
			}
		}

		public static List<VoxelBodyPiece> GatherBody()
		{
			return GatherBody(new List<VoxelBodyPiece>());
		}

		public static List<VoxelBodyPiece> GatherBody(List<VoxelBodyPiece> pieces)
		{
			return GatherBody(pieces, null);
		}

		public static List<VoxelBodyPiece> GatherBody(List<VoxelBodyPiece> pieces, List<VoxelEditorController> controllers)
		{
			pieces.Clear();
			controllers?.Clear();
			Transform transform = null;
			foreach (VoxelEditorController item in GetAllUsable())
			{
				if (!(item.Model == null))
				{
					if (transform == null)
					{
						transform = item.transform;
					}
					pieces.Add(VoxelBodyPiece.FromGrid(item.Model.Grid, transform.worldToLocalMatrix * item.transform.localToWorldMatrix));
					controllers?.Add(item);
				}
			}
			return pieces;
		}

		public static float NearestOtherObjectDistance(VoxelEditorController self, Ray ray, float maxDistance)
		{
			float num = float.MaxValue;
			foreach (VoxelEditorController item in registered)
			{
				if (!(item == self) && IsUsable(item) && !(item.ModelCollider == null) && item.ModelCollider.Raycast(ray, out var hitInfo, maxDistance) && hitInfo.distance < num)
				{
					num = hitInfo.distance;
				}
			}
			return num;
		}

		public static bool HasOtherUsableModel(VoxelModel self)
		{
			foreach (VoxelEditorController item in registered)
			{
				if (IsUsable(item) && item.Model != null && item.Model != self)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsCellOccupiedByOther(VoxelModel self, Vector3Int selfLocalCell)
		{
			if (self == null)
			{
				return false;
			}
			Vector3 position = self.transform.TransformPoint(selfLocalCell + new Vector3(0.5f, 0.5f, 0.5f));
			foreach (VoxelEditorController item in registered)
			{
				if (IsUsable(item) && !(item.Model == null) && !(item.Model == self))
				{
					Vector3 vector = item.transform.InverseTransformPoint(position);
					Vector3Int position2 = new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), Mathf.FloorToInt(vector.z));
					if (item.Model.Grid.Contains(position2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static VoxelEditorController GetOriginal()
		{
			if (registered.Count <= 0)
			{
				return null;
			}
			return registered[0];
		}

		public static void SetStickyFocus(VoxelEditorController controller)
		{
			if (IsUsable(controller))
			{
				stickyFocused = controller;
			}
		}

		public static bool IsAnyGestureActive()
		{
			foreach (VoxelEditorController item in registered)
			{
				if (IsUsable(item) && item.IsGestureActive)
				{
					return true;
				}
			}
			return false;
		}

		public static bool TryPickAnyVoxelColor(Ray ray, float maxDistance, out Color32 color)
		{
			color = default(Color32);
			VoxelEditorController voxelEditorController = null;
			float num = float.MaxValue;
			foreach (VoxelEditorController item in registered)
			{
				if (IsUsable(item) && !(item.ModelCollider == null) && item.ModelCollider.Raycast(ray, out var hitInfo, maxDistance) && hitInfo.distance < num)
				{
					num = hitInfo.distance;
					voxelEditorController = item;
				}
			}
			if (voxelEditorController != null && FacePicker.TryPick(ray, voxelEditorController.Model, voxelEditorController.ModelCollider, maxDistance, out var voxelPosition, out var faceNormal) && voxelEditorController.Model.Grid.Contains(voxelPosition))
			{
				color = voxelEditorController.Model.Grid.GetFaceColor(voxelPosition, FaceAxes.IndexOf(faceNormal));
				return true;
			}
			return TryPickEnvironmentColor(ray, maxDistance, out color);
		}

		private static bool TryPickEnvironmentColor(Ray ray, float maxDistance, out Color32 color)
		{
			color = default(Color32);
			if (!Physics.Raycast(ray, out var hitInfo, maxDistance, -1, QueryTriggerInteraction.Ignore))
			{
				return false;
			}
			VoxelModel componentInParent = hitInfo.collider.GetComponentInParent<VoxelModel>();
			if (componentInParent != null)
			{
				if (FacePicker.TryPick(ray, componentInParent, hitInfo.collider, maxDistance, out var voxelPosition, out var faceNormal) && componentInParent.Grid.Contains(voxelPosition))
				{
					color = componentInParent.Grid.GetFaceColor(voxelPosition, FaceAxes.IndexOf(faceNormal));
					return true;
				}
				return false;
			}
			return SurfaceColorSampler.TrySample(hitInfo, out color);
		}

		private static bool IsUsable(VoxelEditorController c)
		{
			if (c != null)
			{
				return c.isActiveAndEnabled;
			}
			return false;
		}

		public static VoxelEditorController GetFocused(Camera cam)
		{
			if (Time.frameCount == cachedFrame)
			{
				return frameResult;
			}
			cachedFrame = Time.frameCount;
			frameResult = Compute(cam);
			if (frameResult != null)
			{
				stickyFocused = frameResult;
			}
			return frameResult;
		}

		private static VoxelEditorController Compute(Camera cam)
		{
			foreach (VoxelEditorController item in registered)
			{
				if (IsUsable(item) && item.IsFocusLocked)
				{
					return item;
				}
			}
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			{
				return FallbackFocus();
			}
			if (cam == null || !PointerScreenPosition.TryGet(out var position))
			{
				return FallbackFocus();
			}
			Ray ray = cam.ScreenPointToRay(position);
			VoxelEditorController voxelEditorController = null;
			float num = float.MaxValue;
			foreach (VoxelEditorController item2 in registered)
			{
				if (IsUsable(item2))
				{
					float? gizmoHoverDistance = item2.GetGizmoHoverDistance(cam);
					if (gizmoHoverDistance.HasValue && gizmoHoverDistance.Value < num)
					{
						num = gizmoHoverDistance.Value;
						voxelEditorController = item2;
					}
				}
			}
			if (voxelEditorController != null && num <= 6f)
			{
				return voxelEditorController;
			}
			VoxelEditorController voxelEditorController2 = null;
			float num2 = float.MaxValue;
			foreach (VoxelEditorController item3 in registered)
			{
				if (IsUsable(item3) && item3.ModelCollider != null && item3.ModelCollider.Raycast(ray, out var hitInfo, 1000f) && hitInfo.distance < num2)
				{
					num2 = hitInfo.distance;
					voxelEditorController2 = item3;
				}
			}
			if (voxelEditorController2 != null)
			{
				return voxelEditorController2;
			}
			if (voxelEditorController != null)
			{
				return voxelEditorController;
			}
			return FallbackFocus();
		}

		private static VoxelEditorController FallbackFocus()
		{
			if (IsUsable(stickyFocused) && registered.Contains(stickyFocused))
			{
				return stickyFocused;
			}
			VoxelEditorController voxelEditorController = null;
			foreach (VoxelEditorController item in registered)
			{
				if (IsUsable(item))
				{
					if (voxelEditorController != null)
					{
						return null;
					}
					voxelEditorController = item;
				}
			}
			return voxelEditorController;
		}
	}
}

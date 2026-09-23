using Mimicraft.Cameras;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Tutorial
{
	public class TutorialContext
	{
		private VoxelEditorController controller;

		private Transform player;

		public bool DidTransform;

		public bool DidExtrude;

		public bool DidPaint;

		public bool DidUndo;

		public bool DidCreate;

		public bool DidLoopCut;

		public bool DidBevel;

		public bool DidPattern;

		public bool SawOverLimit;

		public VoxelEditorController Controller => controller;

		public VoxelModel Model
		{
			get
			{
				if (!(controller != null))
				{
					return null;
				}
				return controller.Model;
			}
		}

		public Transform Player => player;

		public TutorialGhostTarget Ghost { get; private set; }

		public TutorialFaceGlow Glow { get; private set; }

		public TutorialWorldMarker Marker { get; private set; }

		public Camera Camera => VoxelEditorSettings.ActiveCamera;

		public OrbitCamera Orbit
		{
			get
			{
				Camera camera = Camera;
				if (!(camera != null))
				{
					return null;
				}
				return camera.GetComponent<OrbitCamera>();
			}
		}

		public void ClearLatches()
		{
			DidTransform = (DidExtrude = (DidPaint = (DidUndo = false)));
			DidCreate = (DidLoopCut = (DidBevel = (DidPattern = false)));
			SawOverLimit = false;
		}

		public bool TryResolve()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			NetworkObject networkObject = ((singleton != null && singleton.LocalClient != null) ? singleton.LocalClient.PlayerObject : null);
			PlayerVoxelBody playerVoxelBody = ((networkObject != null) ? networkObject.GetComponent<PlayerVoxelBody>() : null);
			VoxelEditorController voxelEditorController = ((playerVoxelBody != null) ? playerVoxelBody.EditorController : VoxelFocusManager.GetOriginal());
			if (voxelEditorController == null || !voxelEditorController.isActiveAndEnabled || voxelEditorController.Model == null)
			{
				return false;
			}
			controller = voxelEditorController;
			player = ((networkObject != null) ? networkObject.transform : voxelEditorController.transform);
			if (Ghost == null)
			{
				Ghost = TutorialGhostTarget.Create(voxelEditorController.Model);
				Glow = TutorialFaceGlow.Create(voxelEditorController.Model);
				Marker = TutorialWorldMarker.Create();
			}
			return true;
		}

		public void HideMarkers()
		{
			if (Ghost != null)
			{
				Ghost.Clear();
			}
			if (Glow != null)
			{
				Glow.Hide();
			}
			if (Marker != null)
			{
				Marker.Hide();
			}
		}

		public void Destroy()
		{
			if (Ghost != null)
			{
				Object.Destroy(Ghost.gameObject);
			}
			if (Glow != null)
			{
				Object.Destroy(Glow.gameObject);
			}
			if (Marker != null)
			{
				Object.Destroy(Marker.gameObject);
			}
			Ghost = null;
			Glow = null;
			Marker = null;
		}

		public Bounds WorldBounds()
		{
			VoxelModel model = Model;
			if (model == null)
			{
				return new Bounds((player != null) ? player.position : Vector3.zero, Vector3.one);
			}
			if (!GridBounds.TryCompute(model.Grid, out var min, out var max))
			{
				return new Bounds(model.transform.position, Vector3.one * model.VoxelSize);
			}
			Vector3 vector = min;
			Vector3 vector2 = max + Vector3.one;
			Bounds result = new Bounds(model.transform.TransformPoint((vector + vector2) * 0.5f), Vector3.zero);
			for (int i = 0; i < 8; i++)
			{
				Vector3 position = new Vector3(((i & 1) == 0) ? vector.x : vector2.x, ((i & 2) == 0) ? vector.y : vector2.y, ((i & 4) == 0) ? vector.z : vector2.z);
				result.Encapsulate(model.transform.TransformPoint(position));
			}
			return result;
		}

		public Vector3 WorldCenter()
		{
			return WorldBounds().center;
		}

		public float WorldRadius()
		{
			return WorldBounds().extents.magnitude;
		}

		public Vector3 FloorPoint()
		{
			Bounds bounds = WorldBounds();
			return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
		}

		public static Vector3 SnappedAxis(Vector3 direction)
		{
			direction.y = 0f;
			if (direction.sqrMagnitude < 0.0001f)
			{
				return Vector3.forward;
			}
			if (!(Mathf.Abs(direction.x) >= Mathf.Abs(direction.z)))
			{
				return new Vector3(0f, 0f, Mathf.Sign(direction.z));
			}
			return new Vector3(Mathf.Sign(direction.x), 0f, 0f);
		}

		public Vector3 ClearDirection(Vector3 preferred, float distance)
		{
			Vector3 vector = SnappedAxis(preferred);
			Vector3 vector2 = -vector;
			Vector3 vector3 = new Vector3(vector.z, 0f, 0f - vector.x);
			Vector3[] obj = new Vector3[4]
			{
				vector,
				vector2,
				vector3,
				-vector3
			};
			float num = WorldRadius();
			Vector3 vector4 = WorldCenter();
			Vector3[] array = obj;
			foreach (Vector3 vector5 in array)
			{
				if (!Physics.Raycast(vector4 + vector5 * (num + 0.05f), vector5, distance + num, -5, QueryTriggerInteraction.Ignore))
				{
					return vector5;
				}
			}
			return vector;
		}

		public void FrameModel()
		{
			OrbitCamera orbit = Orbit;
			if (!(orbit == null))
			{
				orbit.Frame(WorldCenter(), WorldRadius());
				orbit.SettleIntoClearSpace();
			}
		}
	}
}

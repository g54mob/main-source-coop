using System.Collections.Generic;
using Mimicraft.Cameras;
using Mimicraft.Settings;
using Mimicraft.UI;
using Mimicraft.VoxelEditor;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PlayModeManager : MonoBehaviour
	{
		private static readonly string[] UIRootNames = new string[5] { "Toolbar", "SidePanel", "TopBar", "ObjectHierarchy", "EditHistory" };

		private readonly List<GameObject> editorUIRoots = new List<GameObject>();

		private readonly Dictionary<Transform, Transform> originalParents = new Dictionary<Transform, Transform>();

		private OrbitCamera orbitCamera;

		private ThirdPersonCamera thirdPersonCamera;

		private VoxelEditorController activePrimary;

		public bool IsMovementMode { get; private set; }

		public static PlayModeManager Create(Camera cam, IEnumerable<GameObject> editorUIRoots)
		{
			GameObject gameObject = GameObject.Find("PlayModeManager");
			if (gameObject == null)
			{
				gameObject = new GameObject("PlayModeManager");
			}
			PlayModeManager playModeManager = gameObject.GetComponent<PlayModeManager>();
			if (playModeManager == null)
			{
				playModeManager = gameObject.AddComponent<PlayModeManager>();
			}
			if (cam != null && cam.GetComponent<ThirdPersonCamera>() == null)
			{
				cam.gameObject.AddComponent<ThirdPersonCamera>().enabled = false;
			}
			return playModeManager;
		}

		public void AddUIRoot(GameObject root)
		{
		}

		private void Awake()
		{
			ResolveReferences();
		}

		private Camera OwnSceneCamera()
		{
			Camera main = Camera.main;
			if (main != null && main.gameObject.scene == base.gameObject.scene)
			{
				return main;
			}
			Camera[] array = Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (Camera camera in array)
			{
				if (camera.gameObject.scene == base.gameObject.scene)
				{
					return camera;
				}
			}
			return null;
		}

		private void ResolveReferences()
		{
			Camera camera = OwnSceneCamera();
			if (camera != null)
			{
				orbitCamera = camera.GetComponent<OrbitCamera>();
				thirdPersonCamera = camera.GetComponent<ThirdPersonCamera>();
				if (thirdPersonCamera == null)
				{
					thirdPersonCamera = camera.gameObject.AddComponent<ThirdPersonCamera>();
				}
				thirdPersonCamera.enabled = false;
				if (orbitCamera != null)
				{
					orbitCamera.enabled = true;
				}
			}
			editorUIRoots.Clear();
			GameObject gameObject = GameObject.Find("VoxelEditorCanvas");
			if (gameObject != null && gameObject.scene != base.gameObject.scene)
			{
				gameObject = null;
			}
			if (!(gameObject != null))
			{
				return;
			}
			string[] uIRootNames = UIRootNames;
			foreach (string n in uIRootNames)
			{
				Transform transform = gameObject.transform.Find(n);
				if (transform != null)
				{
					editorUIRoots.Add(transform.gameObject);
				}
			}
		}

		private IEnumerable<VoxelEditorController> OwnSceneControllers()
		{
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (item != null && item.gameObject.scene == base.gameObject.scene)
				{
					yield return item;
				}
			}
		}

		private VoxelEditorController OwnScenePrimary()
		{
			VoxelEditorController original = VoxelFocusManager.GetOriginal();
			if (original != null && original.gameObject.scene == base.gameObject.scene)
			{
				return original;
			}
			using (IEnumerator<VoxelEditorController> enumerator = OwnSceneControllers().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		private void Update()
		{
			if (GameInput.EditorToggle.WasPressedThisFrame() && !(OwnScenePrimary() == null))
			{
				SetMovementMode(!IsMovementMode);
			}
		}

		public void SetMovementMode(bool active)
		{
			if (active != IsMovementMode)
			{
				if (active)
				{
					EnterMovementMode();
				}
				else
				{
					ExitMovementMode();
				}
			}
		}

		private void EnterMovementMode()
		{
			VoxelEditorController voxelEditorController = OwnScenePrimary();
			if (voxelEditorController == null)
			{
				return;
			}
			CharacterController component = voxelEditorController.GetComponent<CharacterController>();
			if (component != null && StartsInsideSolid(voxelEditorController, component, out var collider))
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show("Hata!\nObje bir duvarın içinde. Hareket Moduna geçmeden önce konumunu düzelt.");
				}
				if ((bool)collider)
				{
					Debug.LogWarning($"PlayModeManager: {voxelEditorController.name} starts inside solid {collider.name} ({collider.GetType().Name}) at {collider.transform.position}");
				}
				else
				{
					Debug.LogWarning("PlayModeManager: " + voxelEditorController.name + " starts inside solid (no collider found)");
				}
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.editDeniedClip);
				return;
			}
			activePrimary = voxelEditorController;
			originalParents.Clear();
			foreach (VoxelEditorController item in OwnSceneControllers())
			{
				if (!(item == voxelEditorController))
				{
					originalParents[item.transform] = item.transform.parent;
					item.transform.SetParent(voxelEditorController.transform, worldPositionStays: true);
				}
			}
			VoxelEditorSettings.IsMovementMode = true;
			StrandedPieceVisibility.Apply(UsableModels(), hideStranded: true);
			PropMovementController component2 = voxelEditorController.GetComponent<PropMovementController>();
			Transform cameraTransform = ((orbitCamera != null) ? orbitCamera.transform : Camera.main.transform);
			component2.Activate(cameraTransform);
			if (thirdPersonCamera != null)
			{
				thirdPersonCamera.Target = voxelEditorController.transform;
				if (BodyCapsule.TryMeasure(voxelEditorController.transform, out var boundsLocal))
				{
					thirdPersonCamera.PivotOffsetLocal = boundsLocal.center;
				}
				thirdPersonCamera.enabled = true;
			}
			if (orbitCamera != null)
			{
				orbitCamera.enabled = false;
			}
			SetUIRootsActive(active: false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			IsMovementMode = true;
		}

		private void ExitMovementMode()
		{
			if (activePrimary != null)
			{
				activePrimary.GetComponent<PropMovementController>().Deactivate();
			}
			StrandedPieceVisibility.Apply(UsableModels(), hideStranded: false);
			foreach (KeyValuePair<Transform, Transform> originalParent in originalParents)
			{
				if (originalParent.Key != null)
				{
					originalParent.Key.SetParent(originalParent.Value, worldPositionStays: true);
				}
			}
			originalParents.Clear();
			VoxelEditorSettings.IsMovementMode = false;
			if (thirdPersonCamera != null)
			{
				thirdPersonCamera.enabled = false;
			}
			if (orbitCamera != null)
			{
				if (activePrimary != null)
				{
					orbitCamera.SetFocusPoint(activePrimary.transform.TransformPoint(activePrimary.Model.GetCurrentBoundsCenterLocal()));
				}
				orbitCamera.enabled = true;
			}
			activePrimary = null;
			SetUIRootsActive(active: true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			IsMovementMode = false;
		}

		private static bool StartsInsideSolid(VoxelEditorController primary, CharacterController controller, out Collider collider)
		{
			Transform transform = primary.transform;
			float num = Mathf.Max(primary.Model.VoxelSize, 0.0001f);
			Vector3 currentBoundsCenterLocal = primary.Model.GetCurrentBoundsCenterLocal();
			return SolidSpaceProbe.IsTouchingOrInsideSolid(transform.TransformPoint(new Vector3(currentBoundsCenterLocal.x, controller.height * 0.5f, currentBoundsCenterLocal.z)), transform.up, controller.radius * num, controller.height * num, transform, out collider);
		}

		private IEnumerable<VoxelModel> UsableModels()
		{
			foreach (VoxelEditorController item in OwnSceneControllers())
			{
				if (item.Model != null)
				{
					yield return item.Model;
				}
			}
		}

		private void SetUIRootsActive(bool active)
		{
			foreach (GameObject editorUIRoot in editorUIRoots)
			{
				if (editorUIRoot != null)
				{
					editorUIRoot.SetActive(active);
				}
			}
		}
	}
}

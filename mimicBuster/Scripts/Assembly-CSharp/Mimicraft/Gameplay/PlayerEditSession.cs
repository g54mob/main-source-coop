using Mimicraft.Cameras;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.VoxelEditor;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerEditSession : NetworkBehaviour
	{
		[SerializeField]
		private PlayerMovement movement;

		[SerializeField]
		private PlayerVoxelBody voxelBody;

		[SerializeField]
		private OrbitCamera orbitCamera;

		[SerializeField]
		private ThirdPersonCamera thirdPersonCamera;

		private GameModeController roundManager;

		private bool applied;

		private bool lastCanEdit;

		private bool? unlitBeforeMovement;

		public bool IsEditing { get; private set; }

		public void SetReferences(PlayerMovement movement, PlayerVoxelBody voxelBody, OrbitCamera orbitCamera, ThirdPersonCamera thirdPersonCamera)
		{
			this.movement = movement;
			this.voxelBody = voxelBody;
			this.orbitCamera = orbitCamera;
			this.thirdPersonCamera = thirdPersonCamera;
		}

		private void Update()
		{
			if (!base.IsOwner)
			{
				return;
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
			}
			if (roundManager == null)
			{
				return;
			}
			bool canLocalPlayerEditModel = roundManager.CanLocalPlayerEditModel;
			GameMenuState.SetPointerCaptured(this, roundManager.LocalPlayerNeedsCursor);
			if (!applied || canLocalPlayerEditModel != lastCanEdit)
			{
				lastCanEdit = canLocalPlayerEditModel;
				Apply(canLocalPlayerEditModel);
				return;
			}
			if (!GameMenuState.IsMenuOpen && Cursor.lockState != DesiredLockState())
			{
				RefreshCursorState();
			}
			if (canLocalPlayerEditModel && !GameMenuState.IsMenuOpen && roundManager.LocalPlayerMayLeaveEditMode && GameInput.EditorToggle.WasPressedThisFrame())
			{
				Apply(!IsEditing);
			}
		}

		public override void OnNetworkDespawn()
		{
			if (base.IsOwner)
			{
				VoxelEditorSettings.IsMovementMode = false;
				if (unlitBeforeMovement.HasValue)
				{
					VoxelEditorSettings.UnlitMode = unlitBeforeMovement.Value;
					unlitBeforeMovement = null;
				}
				GameMenuState.SetPointerCaptured(this, captured: false);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		private void ApplyLighting(bool editing)
		{
			if (!editing)
			{
				bool valueOrDefault = unlitBeforeMovement == true;
				if (!unlitBeforeMovement.HasValue)
				{
					valueOrDefault = VoxelEditorSettings.UnlitMode;
					unlitBeforeMovement = valueOrDefault;
				}
				VoxelEditorSettings.UnlitMode = false;
			}
			else
			{
				if (!unlitBeforeMovement.HasValue)
				{
					return;
				}
				VoxelEditorSettings.UnlitMode = unlitBeforeMovement.Value;
				unlitBeforeMovement = null;
			}
			if (voxelBody != null)
			{
				voxelBody.ApplyUnlitSetting();
			}
		}

		private void Apply(bool editing)
		{
			if (applied && editing == IsEditing)
			{
				return;
			}
			IsEditing = editing;
			applied = true;
			VoxelEditorSettings.IsMovementMode = !editing;
			ApplyLighting(editing);
			if (movement != null)
			{
				movement.InputEnabled = !editing;
			}
			if (!editing && voxelBody != null && voxelBody.AlignCharacterToBody() && movement != null)
			{
				movement.NotifyRepositioned();
			}
			if (voxelBody != null)
			{
				voxelBody.SetStrandedPiecesHidden(!editing);
			}
			if (thirdPersonCamera != null)
			{
				thirdPersonCamera.enabled = !editing;
			}
			if (orbitCamera != null)
			{
				orbitCamera.enabled = editing;
				if (editing)
				{
					orbitCamera.SetFocusPoint(CurrentModelCenter());
					orbitCamera.IgnoreRoot = base.transform;
					orbitCamera.CollisionEnabled = false;
					orbitCamera.SettleIntoClearSpace();
				}
			}
			if (!GameMenuState.IsMenuOpen)
			{
				RefreshCursorState();
			}
		}

		private CursorLockMode DesiredLockState()
		{
			if (!IsEditing && !GameMenuState.PointerCaptured && !GameMenuState.OverlayOpen)
			{
				return CursorLockMode.Locked;
			}
			return CursorLockMode.None;
		}

		public void RefreshCursorState()
		{
			if (base.IsOwner)
			{
				Cursor.visible = (Cursor.lockState = DesiredLockState()) == CursorLockMode.None;
			}
		}

		public void RefocusEditorCamera()
		{
			if (base.IsOwner && IsEditing && !(orbitCamera == null))
			{
				orbitCamera.SetFocusPoint(CurrentModelCenter());
				orbitCamera.SettleIntoClearSpace();
			}
		}

		private Vector3 CurrentModelCenter()
		{
			VoxelEditorController voxelEditorController = ((voxelBody != null) ? voxelBody.EditorController : null);
			if (voxelEditorController == null || voxelEditorController.Model == null)
			{
				return base.transform.position;
			}
			return voxelEditorController.transform.TransformPoint(voxelEditorController.Model.GetCurrentBoundsCenterLocal());
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "PlayerEditSession";
		}
	}
}

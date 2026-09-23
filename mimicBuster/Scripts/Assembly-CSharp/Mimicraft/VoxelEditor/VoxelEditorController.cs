using Mimicraft.Gameplay;
using Mimicraft.Settings;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor
{
	[RequireComponent(typeof(VoxelModel))]
	public class VoxelEditorController : MonoBehaviour
	{
		[SerializeField]
		private Camera targetCamera;

		[SerializeField]
		private float maxPickDistance = 100f;

		[SerializeField]
		private Texture2D eyedropperCursorIcon;

		[Header("Visualization")]
		[Tooltip("The small voxel-boundary previews under the cursor in Extrude/Paint/Loop Cut. The G key is a separate thing: it draws the WHOLE model's grid (VoxelEditorSettings.ShowGrid).")]
		[SerializeField]
		private bool showGridOverlay = true;

		private VoxelGridOverlay wholeGrid;

		[Tooltip("Overall length of the Move arrow gizmos + rotate ring, independent of Voxel Size. Tune this if the arrows look too big or too small for your model's Voxel Size.")]
		[SerializeField]
		[Min(0.01f)]
		private float gizmoSize = 1f;

		[Tooltip("Overall length of the Extrude arrow gizmo, independent of Voxel Size. Kept separate from Gizmo Size since the Extrude arrow's base length is unrelated to the model's bounding box.")]
		[SerializeField]
		[Min(0.01f)]
		private float extrudeGizmoSize = 1f;

		[Tooltip("Shaft/head thickness of the Move and Extrude arrow gizmos, independent of their length.")]
		[SerializeField]
		[Min(0.01f)]
		private float gizmoThickness = 1f;

		[Tooltip("Size of the yellow centre marker sphere, independent of every other gizmo. Its world diameter is authored for a Modelci's model; a character part needs it far smaller.")]
		[SerializeField]
		[Min(0.01f)]
		private float centerMarkerSize = 1f;

		private VoxelModel model;

		private Collider modelCollider;

		private FaceHighlight highlight;

		private ExtrudeTool extrudeTool;

		private BevelTool bevelTool;

		private LoopCutTool loopCutTool;

		private TransformTool transformTool;

		private PaintTool paintTool;

		private ObjectHoverHighlight objectHoverHighlight;

		private CenterMarkerGizmo centerMarker;

		private bool gizmosHidden;

		private EditorState lastAppliedState = EditorState.Transform;

		public bool AllowTransform { get; set; } = true;

		public bool AllowTransformGizmos { get; set; } = true;

		private bool TransformGizmosLive
		{
			get
			{
				if (State == EditorState.Transform)
				{
					return AllowTransformGizmos;
				}
				return false;
			}
		}

		public EditorState State
		{
			get
			{
				EditorState currentTool = VoxelEditorSettings.CurrentTool;
				if (currentTool == EditorState.Transform && !AllowTransform)
				{
					return EditorState.View;
				}
				if (!VoxelEditorSettings.IsToolAvailable(currentTool))
				{
					return EditorState.View;
				}
				return currentTool;
			}
		}

		public bool HasHover { get; private set; }

		public Vector3Int HoveredVoxel { get; private set; }

		public Vector3Int HoveredFaceNormal { get; private set; }

		public Collider ModelCollider => modelCollider;

		public VoxelModel Model => model;

		public bool IsGestureActive
		{
			get
			{
				if (!extrudeTool.IsActive && !bevelTool.IsActive)
				{
					if (AllowTransformGizmos)
					{
						return transformTool.IsActive;
					}
					return false;
				}
				return true;
			}
		}

		public bool IsFocusLocked
		{
			get
			{
				if (!extrudeTool.IsDragging && !bevelTool.IsDragging && !paintTool.IsStroking)
				{
					if (AllowTransformGizmos)
					{
						return transformTool.IsActive;
					}
					return false;
				}
				return true;
			}
		}

		public Color PaintColor
		{
			get
			{
				return VoxelEditorSettings.PaintColor;
			}
			set
			{
				VoxelEditorSettings.PaintColor = value;
			}
		}

		public float BrushRadius
		{
			get
			{
				return VoxelEditorSettings.BrushRadius;
			}
			set
			{
				VoxelEditorSettings.BrushRadius = Mathf.Clamp(value, 0f, 10f);
			}
		}

		public PaintMode PaintMode
		{
			get
			{
				return VoxelEditorSettings.PaintMode;
			}
			set
			{
				VoxelEditorSettings.PaintMode = value;
			}
		}

		public float PaintThreshold
		{
			get
			{
				return VoxelEditorSettings.PaintThreshold;
			}
			set
			{
				VoxelEditorSettings.PaintThreshold = Mathf.Clamp01(value);
			}
		}

		public int ExtrudeCurrentSteps => extrudeTool.CurrentSteps;

		public bool IsExtrudeDragging
		{
			get
			{
				if (State == EditorState.Extrude)
				{
					return extrudeTool.IsActive;
				}
				return false;
			}
		}

		public bool IsExtrudeOverLimit => extrudeTool.IsOverLimit;

		public ShapeSubMode ShapeSubMode
		{
			get
			{
				return VoxelEditorSettings.ShapeSubModeSetting;
			}
			set
			{
				VoxelEditorSettings.ShapeSubModeSetting = value;
			}
		}

		public BevelProfile BevelProfile
		{
			get
			{
				return VoxelEditorSettings.BevelProfileSetting;
			}
			set
			{
				VoxelEditorSettings.BevelProfileSetting = value;
			}
		}

		public int BevelCurrentRadius
		{
			get
			{
				if (bevelTool == null)
				{
					return 0;
				}
				return bevelTool.CurrentRadius;
			}
		}

		public bool IsBevelDragging
		{
			get
			{
				if (State == EditorState.Extrude && ShapeSubMode == ShapeSubMode.Bevel)
				{
					return bevelTool.IsActive;
				}
				return false;
			}
		}

		public ExtrudeMode ExtrudeMode
		{
			get
			{
				return VoxelEditorSettings.ExtrudeModeSetting;
			}
			set
			{
				if (VoxelEditorSettings.IsExtrudeModeAvailable(value))
				{
					VoxelEditorSettings.ExtrudeModeSetting = value;
				}
			}
		}

		public ExtrudeSelectionMode ExtrudeSelectionMode
		{
			get
			{
				return VoxelEditorSettings.ExtrudeSelectionMode;
			}
			set
			{
				VoxelEditorSettings.ExtrudeSelectionMode = value;
			}
		}

		public float GizmoSize
		{
			get
			{
				return gizmoSize;
			}
			set
			{
				gizmoSize = Mathf.Max(0.01f, value);
			}
		}

		public float ExtrudeGizmoSize
		{
			get
			{
				return extrudeGizmoSize;
			}
			set
			{
				extrudeGizmoSize = Mathf.Max(0.01f, value);
			}
		}

		public float GizmoThickness
		{
			get
			{
				return gizmoThickness;
			}
			set
			{
				gizmoThickness = Mathf.Max(0.01f, value);
			}
		}

		public float CenterMarkerSize
		{
			get
			{
				return centerMarkerSize;
			}
			set
			{
				centerMarkerSize = Mathf.Max(0.01f, value);
			}
		}

		public bool ShowGridOverlay
		{
			get
			{
				return showGridOverlay;
			}
			set
			{
				showGridOverlay = value;
			}
		}

		public bool UseGlobalSpace
		{
			get
			{
				return VoxelEditorSettings.UseGlobalSpace;
			}
			set
			{
				VoxelEditorSettings.UseGlobalSpace = value;
			}
		}

		public float SnapMoveIncrement
		{
			get
			{
				return VoxelEditorSettings.SnapMoveIncrement;
			}
			set
			{
				VoxelEditorSettings.SnapMoveIncrement = value;
			}
		}

		public float SnapRotateIncrement
		{
			get
			{
				return VoxelEditorSettings.SnapRotateIncrement;
			}
			set
			{
				VoxelEditorSettings.SnapRotateIncrement = value;
			}
		}

		public TransformMode TransformMode
		{
			get
			{
				return VoxelEditorSettings.TransformMode;
			}
			set
			{
				VoxelEditorSettings.TransformMode = value;
			}
		}

		public bool IsTransformHoveringAxis
		{
			get
			{
				if (AllowTransformGizmos)
				{
					return transformTool.IsHoveringAxis;
				}
				return false;
			}
		}

		public bool IsTransformHoveringEdge
		{
			get
			{
				if (AllowTransformGizmos)
				{
					return transformTool.IsHoveringEdge;
				}
				return false;
			}
		}

		public bool IsTransformDragging
		{
			get
			{
				if (AllowTransformGizmos)
				{
					return transformTool.IsDragging;
				}
				return false;
			}
		}

		public Color? TransformHoveredAxisTint
		{
			get
			{
				if (!AllowTransformGizmos)
				{
					return null;
				}
				return transformTool.HoveredAxisTint;
			}
		}

		public bool IsEyedropperActive
		{
			get
			{
				if (State == EditorState.Paint)
				{
					return paintTool.IsEyedropperActive;
				}
				return false;
			}
		}

		public static bool IsTypingInField
		{
			get
			{
				GameObject gameObject = ((EventSystem.current != null) ? EventSystem.current.currentSelectedGameObject : null);
				if (gameObject != null)
				{
					return gameObject.GetComponent<TMP_InputField>() != null;
				}
				return false;
			}
		}

		public float? GetGizmoHoverDistance(Camera cam)
		{
			if (TransformGizmosLive)
			{
				return transformTool.GetHoverScreenDistance(cam);
			}
			if (State == EditorState.Extrude)
			{
				return extrudeTool.GetArrowHoverScreenDistance(cam);
			}
			return null;
		}

		public void ArmPaintEyedropper()
		{
			paintTool.ArmEyedropper();
		}

		public void SetEyedropperCursorIcon(Texture2D icon)
		{
			eyedropperCursorIcon = icon;
		}

		public bool TryPeekEyedropperColor(out Color32 color)
		{
			color = default(Color32);
			if (!IsEyedropperActive || targetCamera == null || !PointerScreenPosition.TryGet(out var position))
			{
				return false;
			}
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			{
				return false;
			}
			Ray ray = targetCamera.ScreenPointToRay(position);
			if (FacePicker.TryPick(ray, model, modelCollider, maxPickDistance, out var voxelPosition, out var faceNormal) && model.Grid.Contains(voxelPosition))
			{
				color = model.Grid.GetFaceColor(voxelPosition, FaceAxes.IndexOf(faceNormal));
				return true;
			}
			return VoxelFocusManager.TryPickAnyVoxelColor(ray, maxPickDistance, out color);
		}

		private void Awake()
		{
			model = GetComponent<VoxelModel>();
			modelCollider = GetComponent<Collider>();
			if (eyedropperCursorIcon != null)
			{
				CursorManager.EyedropperIcon = eyedropperCursorIcon;
			}
			highlight = FaceHighlight.Create(base.transform);
			extrudeTool = new ExtrudeTool(model, base.transform);
			bevelTool = new BevelTool(model, base.transform);
			loopCutTool = new LoopCutTool(model, modelCollider, base.transform, delegate
			{
				SetState(EditorState.Transform);
			});
			transformTool = new TransformTool(model, base.transform);
			paintTool = new PaintTool(model, base.transform);
			wholeGrid = VoxelGridOverlay.Create(base.transform);
			objectHoverHighlight = ObjectHoverHighlight.Create(base.transform, GetComponent<MeshFilter>());
			centerMarker = CenterMarkerGizmo.Create(base.transform);
			if (GetComponent<PropMovementController>() == null)
			{
				base.gameObject.AddComponent<PropMovementController>();
			}
			VoxelFocusManager.Register(this);
		}

		private void OnDestroy()
		{
			VoxelFocusManager.Unregister(this);
		}

		private void OnDisable()
		{
			HideAllGizmos();
			HideWholeGrid();
		}

		private void UpdateWholeGrid()
		{
			if (!(wholeGrid == null))
			{
				if (!VoxelEditorSettings.ShowGrid)
				{
					HideWholeGrid();
				}
				else
				{
					wholeGrid.ShowWholeGrid(model.Grid, EditorPalette.GridSoft);
				}
			}
		}

		private void HideWholeGrid()
		{
			if (wholeGrid != null && wholeGrid.gameObject.activeSelf)
			{
				wholeGrid.Hide();
			}
		}

		public void AbandonGesture()
		{
			HideAllGizmos();
		}

		private void HideAllGizmos()
		{
			if (extrudeTool != null)
			{
				SetHover(hasHover: false, default(Vector3Int), default(Vector3Int));
				extrudeTool.Cancel();
				bevelTool?.Cancel();
				transformTool.Hide();
				loopCutTool.HideAll();
				paintTool.Hide();
				objectHoverHighlight.SetShown(shown: false);
				centerMarker.Hide();
				gizmosHidden = true;
			}
		}

		private void Update()
		{
			if (VoxelEditorSettings.IsMovementMode || GameMenuState.IsMenuOpen)
			{
				if (!gizmosHidden)
				{
					HideAllGizmos();
				}
				HideWholeGrid();
				return;
			}
			model.SetUnlit(VoxelEditorSettings.UnlitMode);
			SyncGlobalToolState();
			UpdateWholeGrid();
			if (VoxelFocusManager.GetFocused(targetCamera) != this)
			{
				if (!gizmosHidden)
				{
					HideAllGizmos();
				}
				return;
			}
			gizmosHidden = false;
			HandleToolSwitch();
			objectHoverHighlight.SetShown(State == EditorState.Transform);
			centerMarker.Show(model, centerMarkerSize);
			extrudeTool.ShowGridOverlay = showGridOverlay;
			paintTool.ShowGridOverlay = showGridOverlay;
			loopCutTool.ShowGridOverlay = showGridOverlay;
			extrudeTool.GizmoSizeMultiplier = extrudeGizmoSize;
			extrudeTool.GizmoThicknessMultiplier = gizmoThickness;
			bevelTool.GizmoSizeMultiplier = extrudeGizmoSize;
			bevelTool.GizmoThicknessMultiplier = gizmoThickness;
			transformTool.GizmoSizeMultiplier = gizmoSize;
			transformTool.GizmoThicknessMultiplier = gizmoThickness;
			paintTool.Mode = VoxelEditorSettings.PaintMode;
			extrudeTool.Mode = ExtrudeMode;
			extrudeTool.SelectionMode = VoxelEditorSettings.ExtrudeSelectionMode;
			bevelTool.Profile = VoxelEditorSettings.BevelProfileSetting;
			transformTool.UseGlobalSpace = VoxelEditorSettings.UseGlobalSpace;
			transformTool.Mode = VoxelEditorSettings.TransformMode;
			transformTool.SnapMoveIncrement = VoxelEditorSettings.SnapMoveIncrement;
			transformTool.SnapRotateIncrement = VoxelEditorSettings.SnapRotateIncrement;
			if (targetCamera == null || !PointerScreenPosition.TryGet(out var position))
			{
				SetHover(hasHover: false, default(Vector3Int), default(Vector3Int));
				return;
			}
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			{
				SetHover(hasHover: false, default(Vector3Int), default(Vector3Int));
				if (State == EditorState.Transform)
				{
					if (AllowTransformGizmos)
					{
						transformTool.RenderWithoutInput();
					}
					else
					{
						transformTool.Hide();
					}
				}
				return;
			}
			if (State == EditorState.Transform)
			{
				SetHover(hasHover: false, default(Vector3Int), default(Vector3Int));
				if (AllowTransformGizmos)
				{
					transformTool.UpdateInput(targetCamera);
				}
				else
				{
					transformTool.Hide();
				}
				return;
			}
			if (State == EditorState.LoopCut)
			{
				SetHover(hasHover: false, default(Vector3Int), default(Vector3Int));
				loopCutTool.UpdateInput(targetCamera);
				return;
			}
			if (State == EditorState.View)
			{
				SetHover(hasHover: false, default(Vector3Int), default(Vector3Int));
				return;
			}
			Ray ray = targetCamera.ScreenPointToRay(position);
			Vector3Int voxelPosition;
			Vector3Int faceNormal;
			float hitDistance;
			bool flag = FacePicker.TryPick(ray, model, modelCollider, maxPickDistance, out voxelPosition, out faceNormal, out hitDistance);
			if (flag && !IsFocusLocked && hitDistance > VoxelFocusManager.NearestOtherObjectDistance(this, ray, maxPickDistance))
			{
				flag = false;
			}
			if (State == EditorState.Extrude)
			{
				if (ShapeSubMode == ShapeSubMode.Bevel)
				{
					extrudeTool.Cancel();
					SetHover(hasHover: false, voxelPosition, faceNormal);
					bevelTool.UpdateInput(targetCamera);
				}
				else
				{
					bevelTool.Cancel();
					SetHover(flag && !extrudeTool.IsActive, voxelPosition, faceNormal);
					extrudeTool.UpdateInput(targetCamera, flag, voxelPosition, faceNormal);
				}
			}
			else if (State == EditorState.Paint)
			{
				SetHover(flag, voxelPosition, faceNormal);
				var (color, num) = paintTool.UpdateInput(flag, voxelPosition, faceNormal, VoxelEditorSettings.PaintColor, VoxelEditorSettings.BrushRadius, VoxelEditorSettings.PatternTexture, VoxelEditorSettings.PaintThreshold);
				if (color.HasValue)
				{
					VoxelEditorSettings.PaintColor = color.Value;
				}
				if (num.HasValue)
				{
					BrushRadius = num.Value;
				}
				if (!flag && paintTool.IsEyedropperActive && Mouse.current.leftButton.wasPressedThisFrame && VoxelFocusManager.TryPickAnyVoxelColor(ray, maxPickDistance, out var color2))
				{
					VoxelEditorSettings.PaintColor = color2;
					paintTool.ConsumeEyedropperArm();
				}
			}
			else
			{
				SetHover(flag, voxelPosition, faceNormal);
			}
		}

		private void HandleToolSwitch()
		{
			if (Keyboard.current == null || IsTypingInField)
			{
				return;
			}
			bool flag = State == EditorState.Extrude && extrudeTool.IsActive;
			bool flag2 = TransformGizmosLive && transformTool.IsActive;
			if (GameInput.ToolTransform.WasPressedThisFrame())
			{
				SetState(EditorState.Transform);
			}
			else if (GameInput.ToolExtrude.WasPressedThisFrame())
			{
				SetState(EditorState.Extrude);
			}
			else if (GameInput.ToolPaint.WasPressedThisFrame())
			{
				SetState(EditorState.Paint);
			}
			else if (GameInput.ToolLoopCut.WasPressedThisFrame())
			{
				SetState(EditorState.LoopCut);
			}
			else if (GameInput.ToolView.WasPressedThisFrame())
			{
				SetState(EditorState.View);
			}
			else if (Keyboard.current.escapeKey.wasPressedThisFrame && !flag && !flag2)
			{
				GameMenuState.RequestEscape(10, delegate
				{
					SetState(EditorState.Transform);
				});
			}
			if (GameInput.ToggleGrid.WasPressedThisFrame())
			{
				VoxelEditorSettings.ShowGrid = !VoxelEditorSettings.ShowGrid;
			}
			if (GameInput.ToggleDimensions.WasPressedThisFrame())
			{
				VoxelEditorSettings.ShowDimensions = !VoxelEditorSettings.ShowDimensions;
			}
			if (GameInput.ToggleHistory.WasPressedThisFrame())
			{
				VoxelEditorSettings.ShowHistory = !VoxelEditorSettings.ShowHistory;
			}
			if (GameInput.ToggleMode.WasPressedThisFrame())
			{
				switch (State)
				{
				case EditorState.Transform:
					if (EditorModeOptions.IsSwitchOffered("Transform.AxisSpace"))
					{
						VoxelEditorSettings.UseGlobalSpace = !VoxelEditorSettings.UseGlobalSpace;
					}
					break;
				case EditorState.Extrude:
					if (!IsFocusLocked)
					{
						ShapeSubMode = EditorModeOptions.Next(ShapeSubMode);
					}
					break;
				case EditorState.Paint:
					VoxelEditorSettings.PaintMode = EditorModeOptions.Next(VoxelEditorSettings.PaintMode);
					break;
				}
			}
			if (GameInput.ToggleSubMode.WasPressedThisFrame())
			{
				if (State == EditorState.Extrude)
				{
					if (!IsFocusLocked)
					{
						if (ShapeSubMode == ShapeSubMode.Bevel)
						{
							BevelProfile = EditorModeOptions.Next(BevelProfile);
						}
						else
						{
							VoxelEditorSettings.ExtrudeSelectionMode = EditorModeOptions.Next(VoxelEditorSettings.ExtrudeSelectionMode);
						}
					}
				}
				else if (State == EditorState.Transform)
				{
					VoxelEditorSettings.TransformMode = EditorModeOptions.Next(VoxelEditorSettings.TransformMode);
				}
			}
			if (GameInput.ToggleExtraMode.WasPressedThisFrame() && State == EditorState.Extrude && ShapeSubMode == ShapeSubMode.Extrude && !IsFocusLocked)
			{
				ExtrudeMode = EditorModeOptions.Next(ExtrudeMode);
			}
			if (GameInput.ToggleUnlit.WasPressedThisFrame())
			{
				VoxelEditorSettings.UnlitMode = !VoxelEditorSettings.UnlitMode;
			}
		}

		public void SetState(EditorState newState)
		{
			VoxelEditorSettings.CurrentTool = newState;
			SyncGlobalToolState();
		}

		private void SyncGlobalToolState()
		{
			EditorState state = State;
			if (lastAppliedState != state)
			{
				EditorState num = lastAppliedState;
				if (num == EditorState.Extrude && state != EditorState.Extrude)
				{
					extrudeTool.Cancel();
					bevelTool.Cancel();
				}
				if (num == EditorState.LoopCut && state != EditorState.LoopCut)
				{
					loopCutTool.HideAll();
				}
				if (num == EditorState.Transform && state != EditorState.Transform)
				{
					transformTool.Hide();
				}
				if (num == EditorState.Paint && state != EditorState.Paint)
				{
					paintTool.Hide();
				}
				lastAppliedState = state;
				if (state == EditorState.View)
				{
					SetHover(hasHover: false, default(Vector3Int), default(Vector3Int));
				}
			}
		}

		public void SetCamera(Camera camera)
		{
			targetCamera = camera;
		}

		private void SetHover(bool hasHover, Vector3Int voxelPos, Vector3Int faceNormal)
		{
			HasHover = hasHover;
			HoveredVoxel = voxelPos;
			HoveredFaceNormal = faceNormal;
			if (hasHover)
			{
				highlight.Show(voxelPos, faceNormal);
			}
			else
			{
				highlight.Hide();
			}
		}
	}
}

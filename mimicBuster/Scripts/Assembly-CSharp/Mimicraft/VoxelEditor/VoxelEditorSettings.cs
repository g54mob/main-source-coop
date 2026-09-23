using System;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class VoxelEditorSettings
	{
		public static EditorState CurrentTool = EditorState.Transform;

		public static Color PaintColor = new Color(0.8f, 0.2f, 0.2f);

		public static float BrushRadius;

		public static PaintMode PaintMode = PaintMode.Brush;

		public static float PaintThreshold;

		public static ExtrudeMode ExtrudeModeSetting = ExtrudeMode.Extrude;

		public static ExtrudeSelectionMode ExtrudeSelectionMode = ExtrudeSelectionMode.FreeSelect;

		public static ShapeSubMode ShapeSubModeSetting = ShapeSubMode.Extrude;

		public static BevelProfile BevelProfileSetting = BevelProfile.Rounded;

		public static bool EyedropperArmed;

		public static TransformMode TransformMode = TransformMode.Move;

		public static bool StandaloneEditing;

		public static bool MenuEditing;

		public static bool SinglePieceEditing;

		public static Func<EditorState, bool> TutorialGate;

		public static bool TutorialLocksExtras;

		public static Camera EditorCamera;

		public static bool IsMovementMode;

		public static Texture2D PatternTexture;

		public static string CurrentTemplateFilePath;

		public static string CurrentTemplateName = "";

		public static string CurrentTemplateTag = "";

		public static string CurrentTemplateCategory = "";

		public static Func<Vector3Int, int, bool> PaintMask;

		public const int DefaultMinBoundExtent = 4;

		public const int DefaultSlimBoundExtent = 2;

		public static bool SlimAxisAllowed = true;

		public const int DefaultMinIslandVoxels = 16;

		public static int MaxBoundExtent = 48;

		public static float MaxPieceGapVoxels = 4f;

		public static float MaxPieceOverlapFraction = 0.3f;

		public static int MaxPieces = 16;

		public static bool UseGlobalSpace
		{
			get
			{
				GameSettings.Load();
				return GameSettings.EditorGlobalSpace;
			}
			set
			{
				GameSettings.Load();
				GameSettings.SetEditorGlobalSpace(value);
			}
		}

		public static float SnapMoveIncrement
		{
			get
			{
				GameSettings.Load();
				return GameSettings.EditorSnapMove;
			}
			set
			{
				GameSettings.Load();
				GameSettings.SetEditorSnapMove(value);
			}
		}

		public static float SnapRotateIncrement
		{
			get
			{
				GameSettings.Load();
				return GameSettings.EditorSnapRotate;
			}
			set
			{
				GameSettings.Load();
				GameSettings.SetEditorSnapRotate(value);
			}
		}

		public static bool UnlitMode
		{
			get
			{
				GameSettings.Load();
				return GameSettings.EditorUnlit;
			}
			set
			{
				GameSettings.Load();
				GameSettings.SetEditorUnlit(value);
			}
		}

		public static bool BodyRulesApply => !StandaloneEditing;

		public static Camera ActiveCamera
		{
			get
			{
				if (!(EditorCamera != null))
				{
					return Camera.main;
				}
				return EditorCamera;
			}
		}

		public static bool ShowDimensions
		{
			get
			{
				GameSettings.Load();
				return GameSettings.EditorShowDimensions;
			}
			set
			{
				GameSettings.Load();
				GameSettings.SetEditorShowDimensions(value);
			}
		}

		public static bool ShowGrid
		{
			get
			{
				GameSettings.Load();
				return GameSettings.EditorShowGrid;
			}
			set
			{
				GameSettings.Load();
				GameSettings.SetEditorShowGrid(value);
			}
		}

		public static bool ShowHistory
		{
			get
			{
				GameSettings.Load();
				return GameSettings.EditorShowHistory;
			}
			set
			{
				GameSettings.Load();
				GameSettings.SetEditorShowHistory(value);
			}
		}

		public static int MinBoundExtent
		{
			get
			{
				if (!(GameModeController.Current != null))
				{
					return 4;
				}
				return GameModeController.Current.MinBodyExtent;
			}
		}

		public static int SlimBoundExtent
		{
			get
			{
				if (!SlimAxisAllowed)
				{
					return MinBoundExtent;
				}
				return Mathf.Min(2, MinBoundExtent);
			}
		}

		public static int MinIslandVoxels
		{
			get
			{
				if (!(GameModeController.Current != null))
				{
					return 16;
				}
				return GameModeController.Current.MinBodyIslandVoxels;
			}
		}

		public static bool IsToolAvailable(EditorState tool)
		{
			if ((!StandaloneEditing && !SinglePieceEditing) || tool != EditorState.LoopCut)
			{
				return !IsLockedByTutorial(tool);
			}
			return false;
		}

		public static bool IsLockedByTutorial(EditorState tool)
		{
			if (TutorialGate != null)
			{
				return !TutorialGate(tool);
			}
			return false;
		}

		public static bool IsExtrudeModeAvailable(ExtrudeMode mode)
		{
			if (StandaloneEditing || SinglePieceEditing)
			{
				return mode != ExtrudeMode.Create;
			}
			return true;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetLessonOverrides()
		{
			PaintMask = null;
			SlimAxisAllowed = true;
			MenuEditing = false;
			StandaloneEditing = false;
			SinglePieceEditing = false;
			TutorialLocksExtras = false;
			TutorialGate = null;
		}
	}
}

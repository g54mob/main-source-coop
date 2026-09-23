using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Tutorial
{
	public static class TutorialCurriculum
	{
		private sealed class FaceColorCheck
		{
			private readonly Color32 wanted;

			private readonly List<(Vector3Int Cell, Vector3Int Normal)> faces = new List<(Vector3Int, Vector3Int)>();

			private int seenVersion = -1;

			private int matched;

			private int total;

			public IReadOnlyList<(Vector3Int Cell, Vector3Int Normal)> Faces => faces;

			public FaceColorCheck(Color32 wanted)
			{
				this.wanted = wanted;
			}

			public void Watch(List<(Vector3Int Cell, Vector3Int Normal)> watched)
			{
				faces.Clear();
				faces.AddRange(watched);
				seenVersion = -1;
			}

			public (int done, int total) Counts(VoxelGrid grid)
			{
				Refresh(grid);
				return (done: matched, total: total);
			}

			public float Ratio(VoxelGrid grid)
			{
				Refresh(grid);
				if (total != 0)
				{
					return (float)matched / (float)total;
				}
				return 1f;
			}

			private void Refresh(VoxelGrid grid)
			{
				if (grid.Version == seenVersion)
				{
					return;
				}
				seenVersion = grid.Version;
				matched = 0;
				total = 0;
				foreach (var face2 in faces)
				{
					Vector3Int item = face2.Cell;
					int face = FaceAxes.IndexOf(face2.Normal);
					if (grid.IsFaceVisible(item, face))
					{
						total++;
						if (!grid.TryGetFaceColor(item, face, out var color))
						{
							grid.TryGet(item, out var data);
							color = data.Color;
						}
						if (Similar(color, wanted))
						{
							matched++;
						}
					}
				}
			}

			private static bool Similar(Color32 a, Color32 b)
			{
				if (Mathf.Abs(a.r - b.r) <= 48 && Mathf.Abs(a.g - b.g) <= 48)
				{
					return Mathf.Abs(a.b - b.b) <= 48;
				}
				return false;
			}
		}

		private static readonly Vector3Int Up = Vector3Int.up;

		private static readonly Color32 ScreenColor = new Color32(22, 26, 40, byte.MaxValue);

		private static readonly Color32 CasingColor = new Color32(196, 186, 164, byte.MaxValue);

		private static readonly Color32 BodyColor = new Color32(150, 150, 155, byte.MaxValue);

		private static readonly Color32 KnobColor = new Color32(70, 130, 220, byte.MaxValue);

		private static readonly Color32 BlemishColor = new Color32(byte.MaxValue, 40, 200, byte.MaxValue);

		private const int ShallowSteps = 6;

		private const int HumpSteps = 4;

		private const int HumpWidth = 12;

		private const int HumpHeight = 12;

		private const float LimitsRestoreAfterSeconds = 2.5f;

		private static readonly Vector3Int TvFront = Vector3Int.forward;

		private static readonly Vector3Int TvBack = Vector3Int.back;

		private const float DefaultArriveTolerance = 0.15f;

		private const int ScreenWidth = 12;

		private const int ScreenHeight = 10;

		private const int ScreenDepth = 2;

		private const float PaintedEnough = 0.9f;

		public static readonly IReadOnlyList<string> LessonIds = new string[7] { "tv", "create", "loopcut", "bevel", "pattern", "dimensions", "templates" };

		public const string TvLesson = "tv";

		public static List<TutorialStep> Build(TutorialContext c)
		{
			HashSet<Vector3Int> knob = new HashSet<Vector3Int>();
			List<Vector3Int> lump = new List<Vector3Int>();
			List<TutorialStep> list = new List<TutorialStep>
			{
				Welcome(),
				LookAround(c),
				Grow(c),
				Undo(c, lump),
				Limits(c, lump),
				Widen(c, knob),
				Inset(c),
				Paint(c, knob),
				Bucket(c, knob),
				Move(c),
				Rotate(c),
				StepInside(c),
				HuntersEye(c),
				Done(c)
			};
			if (!EditorModeOptions.IsOffered(PaintMode.Bucket))
			{
				list.RemoveAll((TutorialStep step) => step.TitleKey == "Bucket.Title");
			}
			return list;
		}

		public static bool IsLessonOffered(string id)
		{
			return id switch
			{
				"create" => EditorModeOptions.IsOffered(ExtrudeMode.Create), 
				"bevel" => EditorModeOptions.IsOffered(ShapeSubMode.Bevel), 
				"pattern" => EditorModeOptions.IsOffered(PaintMode.Pattern), 
				_ => true, 
			};
		}

		public static List<TutorialStep> BuildLesson(string id, TutorialContext c)
		{
			List<TutorialStep> list = new List<TutorialStep>();
			if (!IsLessonOffered(id))
			{
				return list;
			}
			TutorialStep tutorialStep;
			switch (id)
			{
			case "tv":
			{
				HashSet<Vector3Int> glass = new HashSet<Vector3Int>();
				list.Add(TvWelcome());
				list.Add(Shallow(c));
				list.Add(Hump(c));
				list.Add(Screen(c, glass));
				list.Add(ScreenPaint(c, glass));
				list.Add(Casing(c, glass));
				list.Add(TvDone(c));
				return list;
			}
			case "create":
				tutorialStep = CreateLesson(c);
				break;
			case "loopcut":
				tutorialStep = LoopCutLesson(c);
				break;
			case "bevel":
				tutorialStep = BevelLesson(c);
				break;
			case "pattern":
				tutorialStep = PatternLesson(c);
				break;
			case "dimensions":
				tutorialStep = DimensionsLesson(c);
				break;
			case "templates":
				tutorialStep = TemplatesLesson(c);
				break;
			default:
				tutorialStep = null;
				break;
			}
			TutorialStep tutorialStep2 = tutorialStep;
			if (tutorialStep2 != null)
			{
				list.Add(tutorialStep2);
			}
			return list;
		}

		private static TutorialStep CreateLesson(TutorialContext c)
		{
			return new TutorialStep
			{
				TitleKey = "Lesson.create.Title",
				BodyKey = "Lesson.create.Body",
				HintKey = "Lesson.create.Hint",
				Highlight = EditorState.Extrude,
				OnEnter = delegate
				{
					ArmExtrude(ExtrudeSelectionMode.FreeSelect);
					VoxelEditorSettings.ExtrudeModeSetting = ExtrudeMode.Create;
					Vector3Int normal = FrontNormal(c);
					c.Glow.Show(VisibleFaces(c.Model.Grid, normal, CentralPatch(c.Model.Grid, normal, 4)));
				},
				IsComplete = () => c.DidCreate
			};
		}

		private static TutorialStep LoopCutLesson(TutorialContext c)
		{
			return new TutorialStep
			{
				TitleKey = "Lesson.loopcut.Title",
				BodyKey = "Lesson.loopcut.Body",
				HintKey = "Lesson.loopcut.Hint",
				Highlight = EditorState.LoopCut,
				IsComplete = () => c.DidLoopCut
			};
		}

		private static TutorialStep BevelLesson(TutorialContext c)
		{
			return new TutorialStep
			{
				TitleKey = "Lesson.bevel.Title",
				BodyKey = "Lesson.bevel.Body",
				HintKey = "Lesson.bevel.Hint",
				Highlight = EditorState.Extrude,
				OnEnter = delegate
				{
					ArmExtrude(ExtrudeSelectionMode.Face);
					VoxelEditorSettings.ShapeSubModeSetting = ShapeSubMode.Bevel;
				},
				IsComplete = () => c.DidBevel
			};
		}

		private static TutorialStep PatternLesson(TutorialContext c)
		{
			return new TutorialStep
			{
				TitleKey = "Lesson.pattern.Title",
				BodyKey = "Lesson.pattern.Body",
				HintKey = "Lesson.pattern.Hint",
				Highlight = EditorState.Paint,
				OnEnter = delegate
				{
					ArmPaint(PaintMode.Pattern, ScreenColor);
				},
				IsComplete = () => c.DidPattern
			};
		}

		private static TutorialStep DimensionsLesson(TutorialContext c)
		{
			return new TutorialStep
			{
				TitleKey = "Lesson.dimensions.Title",
				BodyKey = "Lesson.dimensions.Body",
				HintKey = "Lesson.dimensions.Hint",
				OnEnter = delegate
				{
					VoxelEditorSettings.ShowDimensions = false;
				},
				IsComplete = () => VoxelEditorSettings.ShowDimensions,
				DoIt = delegate
				{
					VoxelEditorSettings.ShowDimensions = true;
				},
				DemoByUndo = false
			};
		}

		private static TutorialStep TemplatesLesson(TutorialContext c)
		{
			string pathAtEntry = null;
			return new TutorialStep
			{
				TitleKey = "Lesson.templates.Title",
				BodyKey = "Lesson.templates.Body",
				HintKey = "Lesson.templates.Hint",
				OnEnter = delegate
				{
					pathAtEntry = VoxelEditorSettings.CurrentTemplateFilePath;
				},
				IsComplete = () => !string.IsNullOrEmpty(VoxelEditorSettings.CurrentTemplateFilePath) && VoxelEditorSettings.CurrentTemplateFilePath != pathAtEntry,
				DoIt = delegate
				{
					SaveFirstModel(c);
				},
				DemoByUndo = false
			};
		}

		private static void ArmExtrude(ExtrudeSelectionMode selection)
		{
			VoxelEditorSettings.ExtrudeModeSetting = ExtrudeMode.Extrude;
			VoxelEditorSettings.ShapeSubModeSetting = ShapeSubMode.Extrude;
			VoxelEditorSettings.ExtrudeSelectionMode = selection;
		}

		private static void ArmPaint(PaintMode mode, Color32 color)
		{
			VoxelEditorSettings.PaintMode = mode;
			VoxelEditorSettings.PaintColor = color;
			VoxelEditorSettings.PaintThreshold = 0f;
			VoxelEditorSettings.EyedropperArmed = false;
			VoxelEditorSettings.BrushRadius = ((mode == PaintMode.Brush) ? 1.5f : VoxelEditorSettings.BrushRadius);
		}

		private static TutorialStep Welcome()
		{
			return new TutorialStep
			{
				TitleKey = "Welcome.Title",
				BodyKey = "Welcome.Body"
			};
		}

		private static TutorialStep LookAround(TutorialContext c)
		{
			float travelled = 0f;
			float lastYaw = 0f;
			float startDistance = 0f;
			bool zoomed = false;
			return new TutorialStep
			{
				TitleKey = "Camera.Title",
				BodyKey = "Camera.Body",
				ClipName = "Camera",
				HintKey = "Camera.Hint",
				OnEnter = delegate
				{
					travelled = 0f;
					zoomed = false;
					if (c.Orbit != null)
					{
						lastYaw = c.Orbit.Yaw;
						startDistance = c.Orbit.Distance;
					}
					Vector3 vector = c.WorldCenter();
					Vector3 vector2 = ((c.Camera != null) ? (vector - c.Camera.transform.position) : Vector3.forward);
					vector2.y = 0f;
					vector2 = ((vector2.sqrMagnitude > 0.0001f) ? vector2.normalized : Vector3.forward);
					c.Marker.ShowStar(vector + vector2 * (c.WorldRadius() + 0.35f));
				},
				IsComplete = delegate
				{
					if (c.Orbit == null)
					{
						return false;
					}
					travelled += Mathf.Abs(Mathf.DeltaAngle(lastYaw, c.Orbit.Yaw));
					lastYaw = c.Orbit.Yaw;
					if (Mathf.Abs(c.Orbit.Distance - startDistance) > 0.25f)
					{
						zoomed = true;
					}
					return travelled >= 150f && zoomed;
				}
			};
		}

		private static TutorialStep Grow(TutorialContext c)
		{
			int toAdd = 0;
			VoxelGrid built = null;
			TutorialStep tutorialStep = new TutorialStep();
			tutorialStep.TitleKey = "Grow.Title";
			tutorialStep.BodyKey = "Grow.Body";
			tutorialStep.ClipName = "Grow";
			tutorialStep.HintKey = "Grow.Hint";
			tutorialStep.Highlight = EditorState.Extrude;
			tutorialStep.Unlocks = new EditorState[1] { EditorState.Extrude };
			tutorialStep.DoIt = delegate
			{
				ApplyGrid(c, built);
			};
			tutorialStep.OnEnter = delegate
			{
				ArmExtrude(ExtrudeSelectionMode.Face);
				c.Glow.Show(VisibleFaces(c.Model.Grid, Up, null));
			};
			tutorialStep.Target = delegate
			{
				VoxelGrid voxelGrid = (built = Extruded(c.Model.Grid, Up, 2, null));
				toAdd = voxelGrid.Count - c.Model.Grid.Count;
				return voxelGrid;
			};
			tutorialStep.IsComplete = () => c.Ghost.Satisfied;
			tutorialStep.Progress = () => (done: Mathf.Max(0, toAdd - c.Ghost.MissingCount), total: toAdd);
			return tutorialStep;
		}

		private static TutorialStep Widen(TutorialContext c, HashSet<Vector3Int> knob)
		{
			int toAdd = 0;
			HashSet<Vector3Int> patch = null;
			Vector3Int front = Vector3Int.forward;
			VoxelGrid built = null;
			return new TutorialStep
			{
				TitleKey = "Widen.Title",
				BodyKey = "Widen.Body",
				ClipName = "Widen",
				HintKey = "Widen.Hint",
				Highlight = EditorState.Extrude,
				DoIt = delegate
				{
					ApplyGrid(c, built);
				},
				OnEnter = delegate
				{
					ArmExtrude(ExtrudeSelectionMode.FreeSelect);
					front = FrontNormal(c);
					patch = CentralPatch(c.Model.Grid, front, 4);
					c.Glow.Show(VisibleFaces(c.Model.Grid, front, patch));
				},
				Target = delegate
				{
					VoxelGrid voxelGrid = (built = Extruded(c.Model.Grid, front, 2, patch));
					knob.Clear();
					foreach (Vector3Int position in voxelGrid.Positions)
					{
						if (!c.Model.Grid.Contains(position))
						{
							knob.Add(position);
						}
					}
					toAdd = knob.Count;
					return voxelGrid;
				},
				IsComplete = () => c.Ghost.Satisfied,
				Progress = () => (done: Mathf.Max(0, toAdd - c.Ghost.MissingCount), total: toAdd)
			};
		}

		private static TutorialStep Inset(TutorialContext c)
		{
			int toRemove = 0;
			HashSet<Vector3Int> patch = null;
			VoxelGrid built = null;
			return new TutorialStep
			{
				TitleKey = "Inset.Title",
				BodyKey = "Inset.Body",
				ClipName = "Inset",
				HintKey = "Inset.Hint",
				Highlight = EditorState.Extrude,
				DoIt = delegate
				{
					ApplyGrid(c, built);
				},
				OnEnter = delegate
				{
					ArmExtrude(ExtrudeSelectionMode.FreeSelect);
					patch = CentralPatch(c.Model.Grid, Up, 4);
					c.Glow.Show(VisibleFaces(c.Model.Grid, Up, patch));
				},
				Target = delegate
				{
					VoxelGrid voxelGrid = (built = Carved(c.Model.Grid, Up, 2, patch));
					toRemove = c.Model.Grid.Count - voxelGrid.Count;
					return voxelGrid;
				},
				IsComplete = () => c.Ghost.Satisfied,
				Progress = () => (done: Mathf.Max(0, toRemove - c.Ghost.ExtraCount), total: toRemove)
			};
		}

		private static TutorialStep Paint(TutorialContext c, HashSet<Vector3Int> knob)
		{
			FaceColorCheck check = new FaceColorCheck(KnobColor);
			ICollection<Vector3Int> cells = null;
			TutorialStep tutorialStep = new TutorialStep();
			tutorialStep.TitleKey = "Paint.Title";
			tutorialStep.BodyKey = "Paint.Body";
			tutorialStep.ClipName = "Paint";
			tutorialStep.HintKey = "Paint.Hint";
			tutorialStep.Highlight = EditorState.Paint;
			tutorialStep.Unlocks = new EditorState[1] { EditorState.Paint };
			tutorialStep.DoIt = delegate
			{
				ApplyColor(c, cells, KnobColor);
			};
			tutorialStep.OnEnter = delegate
			{
				ArmPaint(PaintMode.Brush, KnobColor);
				cells = ((knob.Count > 0) ? knob : CentralPatch(c.Model.Grid, Up, 4));
				check.Watch(VisibleFacesOf(c.Model.Grid, cells));
				c.Glow.Show(check.Faces);
			};
			tutorialStep.IsComplete = () => check.Ratio(c.Model.Grid) >= 0.9f;
			tutorialStep.Progress = () => check.Counts(c.Model.Grid);
			return tutorialStep;
		}

		private static TutorialStep Bucket(TutorialContext c, HashSet<Vector3Int> knob)
		{
			FaceColorCheck check = new FaceColorCheck(BodyColor);
			List<Vector3Int> rest = null;
			return new TutorialStep
			{
				TitleKey = "Bucket.Title",
				BodyKey = "Bucket.Body",
				ClipName = "Bucket",
				HintKey = "Bucket.Hint",
				Highlight = EditorState.Paint,
				DoIt = delegate
				{
					ApplyColor(c, rest, BodyColor);
				},
				OnEnter = delegate
				{
					ArmPaint(PaintMode.Bucket, BodyColor);
					rest = new List<Vector3Int>();
					foreach (Vector3Int position in c.Model.Grid.Positions)
					{
						if (!knob.Contains(position))
						{
							rest.Add(position);
						}
					}
					check.Watch(VisibleFacesOf(c.Model.Grid, rest));
				},
				IsComplete = () => check.Ratio(c.Model.Grid) >= 0.9f,
				Progress = () => check.Counts(c.Model.Grid)
			};
		}

		private static TutorialStep TvWelcome()
		{
			return new TutorialStep
			{
				TitleKey = "Tv.Title",
				BodyKey = "Tv.Body"
			};
		}

		private static TutorialStep TvDone(TutorialContext c)
		{
			return new TutorialStep
			{
				TitleKey = "TvDone.Title",
				BodyKey = "TvDone.Body",
				OnEnter = delegate
				{
					SaveFirstModel(c, "TvModelName");
				}
			};
		}

		private static TutorialStep Shallow(TutorialContext c)
		{
			int toRemove = 0;
			VoxelGrid built = null;
			TutorialStep tutorialStep = new TutorialStep();
			tutorialStep.TitleKey = "Shallow.Title";
			tutorialStep.BodyKey = "Shallow.Body";
			tutorialStep.ClipName = "Inset";
			tutorialStep.HintKey = "Shallow.Hint";
			tutorialStep.Highlight = EditorState.Extrude;
			tutorialStep.Unlocks = new EditorState[1] { EditorState.Extrude };
			tutorialStep.DoIt = delegate
			{
				ApplyGrid(c, built);
			};
			tutorialStep.OnEnter = delegate
			{
				ArmExtrude(ExtrudeSelectionMode.Face);
				c.Glow.Show(VisibleFaces(c.Model.Grid, TvBack, null));
			};
			tutorialStep.Target = delegate
			{
				VoxelGrid voxelGrid = (built = Carved(c.Model.Grid, TvBack, 6, null));
				toRemove = c.Model.Grid.Count - voxelGrid.Count;
				return voxelGrid;
			};
			tutorialStep.IsComplete = () => c.Ghost.Satisfied;
			tutorialStep.Progress = () => (done: Mathf.Max(0, toRemove - c.Ghost.ExtraCount), total: toRemove);
			return tutorialStep;
		}

		private static TutorialStep Hump(TutorialContext c)
		{
			int toAdd = 0;
			HashSet<Vector3Int> patch = null;
			VoxelGrid built = null;
			return new TutorialStep
			{
				TitleKey = "Hump.Title",
				BodyKey = "Hump.Body",
				ClipName = "Grow",
				HintKey = "Hump.Hint",
				Highlight = EditorState.Extrude,
				DoIt = delegate
				{
					ApplyGrid(c, built);
				},
				OnEnter = delegate
				{
					ArmExtrude(ExtrudeSelectionMode.FreeSelect);
					patch = CentralRect(c.Model.Grid, TvBack, 12, 12);
					c.Glow.Show(VisibleFaces(c.Model.Grid, TvBack, patch));
				},
				Target = delegate
				{
					VoxelGrid voxelGrid = (built = Extruded(c.Model.Grid, TvBack, 4, patch));
					toAdd = voxelGrid.Count - c.Model.Grid.Count;
					return voxelGrid;
				},
				IsComplete = () => c.Ghost.Satisfied,
				Progress = () => (done: Mathf.Max(0, toAdd - c.Ghost.MissingCount), total: toAdd)
			};
		}

		private static TutorialStep Screen(TutorialContext c, HashSet<Vector3Int> glass)
		{
			int toRemove = 0;
			HashSet<Vector3Int> patch = null;
			VoxelGrid built = null;
			return new TutorialStep
			{
				TitleKey = "Screen.Title",
				BodyKey = "Screen.Body",
				ClipName = "Inset",
				HintKey = "Screen.Hint",
				Highlight = EditorState.Extrude,
				DoIt = delegate
				{
					ApplyGrid(c, built);
				},
				OnEnter = delegate
				{
					ArmExtrude(ExtrudeSelectionMode.FreeSelect);
					patch = CentralRect(c.Model.Grid, TvFront, 12, 10);
					glass.Clear();
					foreach (Vector3Int item in patch)
					{
						Vector3Int vector3Int = item - TvFront * 2;
						if (c.Model.Grid.Contains(vector3Int))
						{
							glass.Add(vector3Int);
						}
					}
					c.Glow.Show(VisibleFaces(c.Model.Grid, TvFront, patch));
				},
				Target = delegate
				{
					VoxelGrid voxelGrid = (built = Carved(c.Model.Grid, TvFront, 2, patch));
					toRemove = c.Model.Grid.Count - voxelGrid.Count;
					return voxelGrid;
				},
				IsComplete = () => c.Ghost.Satisfied,
				Progress = () => (done: Mathf.Max(0, toRemove - c.Ghost.ExtraCount), total: toRemove)
			};
		}

		private static TutorialStep ScreenPaint(TutorialContext c, HashSet<Vector3Int> glass)
		{
			FaceColorCheck check = new FaceColorCheck(ScreenColor);
			ICollection<Vector3Int> cells = null;
			TutorialStep tutorialStep = new TutorialStep();
			tutorialStep.TitleKey = "ScreenPaint.Title";
			tutorialStep.BodyKey = "ScreenPaint.Body";
			tutorialStep.ClipName = "Paint";
			tutorialStep.HintKey = "ScreenPaint.Hint";
			tutorialStep.Highlight = EditorState.Paint;
			tutorialStep.Unlocks = new EditorState[1] { EditorState.Paint };
			tutorialStep.DoIt = delegate
			{
				ApplyColor(c, cells, ScreenColor);
			};
			tutorialStep.OnEnter = delegate
			{
				ArmPaint(PaintMode.Brush, ScreenColor);
				cells = ((glass.Count > 0) ? glass : CentralPatch(c.Model.Grid, TvFront, 4));
				check.Watch(VisibleFacesOf(c.Model.Grid, cells));
				c.Glow.Show(check.Faces);
				ICollection<Vector3Int> allowed = cells;
				VoxelEditorSettings.PaintMask = (Vector3Int cell, int _) => allowed.Contains(cell);
			};
			tutorialStep.OnExit = delegate
			{
				VoxelEditorSettings.PaintMask = null;
			};
			tutorialStep.IsComplete = () => check.Ratio(c.Model.Grid) >= 0.9f;
			tutorialStep.Progress = () => check.Counts(c.Model.Grid);
			return tutorialStep;
		}

		private static TutorialStep Casing(TutorialContext c, HashSet<Vector3Int> glass)
		{
			FaceColorCheck check = new FaceColorCheck(CasingColor);
			List<Vector3Int> rest = null;
			return new TutorialStep
			{
				TitleKey = "Casing.Title",
				BodyKey = "Casing.Body",
				ClipName = "Bucket",
				HintKey = "Casing.Hint",
				Highlight = EditorState.Paint,
				DoIt = delegate
				{
					ApplyColor(c, rest, CasingColor);
				},
				OnEnter = delegate
				{
					ArmPaint(PaintMode.Bucket, CasingColor);
					rest = new List<Vector3Int>();
					foreach (Vector3Int position in c.Model.Grid.Positions)
					{
						if (!glass.Contains(position))
						{
							rest.Add(position);
						}
					}
					check.Watch(VisibleFacesOf(c.Model.Grid, rest));
				},
				IsComplete = () => check.Ratio(c.Model.Grid) >= 0.9f,
				Progress = () => check.Counts(c.Model.Grid)
			};
		}

		private static TutorialStep Undo(TutorialContext c, List<Vector3Int> lump)
		{
			VoxelGrid before = null;
			return new TutorialStep
			{
				TitleKey = "Undo.Title",
				BodyKey = "Undo.Body",
				ClipName = "Undo",
				HintKey = "Undo.Hint",
				DoIt = delegate
				{
					RemoveThroughStack(c, lump);
					c.DidUndo = true;
				},
				DemoByUndo = false,
				OnEnter = delegate
				{
					VoxelGrid grid = c.Model.Grid;
					before = Copy(grid);
					lump.Clear();
					lump.AddRange(Lump(grid, 3));
					Dictionary<Vector3Int, VoxelData?> dictionary = new Dictionary<Vector3Int, VoxelData?>();
					Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>();
					VoxelData voxelData = new VoxelData(BlemishColor);
					foreach (Vector3Int item in lump)
					{
						dictionary[item] = null;
						dictionary2[item] = voxelData;
						grid.Set(item, voxelData);
					}
					c.Model.RebuildMesh();
					UndoManager.Push(new VoxelDiffCommand(c.Model, dictionary, dictionary2, "Tutorial"));
				},
				Target = () => before,
				IsComplete = () => c.DidUndo && !AnyPresent(c.Model.Grid, lump)
			};
		}

		private static TutorialStep Move(TutorialContext c)
		{
			Vector3 target = Vector3.zero;
			TutorialStep tutorialStep = new TutorialStep();
			tutorialStep.TitleKey = "Move.Title";
			tutorialStep.BodyKey = "Move.Body";
			tutorialStep.ClipName = "Move";
			tutorialStep.HintKey = "Move.Hint";
			tutorialStep.Highlight = EditorState.Transform;
			tutorialStep.Unlocks = new EditorState[1] { EditorState.Transform };
			tutorialStep.DoIt = delegate
			{
				Transform transform = c.Model.transform;
				Vector3 position = transform.position;
				Quaternion rotation = transform.rotation;
				Vector3 vector = target - c.FloorPoint();
				vector.y = 0f;
				transform.position += vector;
				UndoManager.Push(new TransformCommand(transform, position, rotation, transform.position, rotation, "Tutorial"));
			};
			tutorialStep.OnEnter = delegate
			{
				VoxelEditorSettings.TransformMode = TransformMode.Move;
				Vector3 preferred = ((c.Camera != null) ? c.Camera.transform.right : Vector3.right);
				target = c.FloorPoint() + c.ClearDirection(preferred, 0.5f) * 0.5f;
				c.Marker.ShowRing(target, 0.3f);
			};
			tutorialStep.IsComplete = () => !c.Controller.IsTransformDragging && Flat(c.FloorPoint() - target).magnitude < 0.08f;
			return tutorialStep;
		}

		private static TutorialStep Rotate(TutorialContext c)
		{
			float startYaw = 0f;
			return new TutorialStep
			{
				TitleKey = "Rotate.Title",
				BodyKey = "Rotate.Body",
				ClipName = "Rotate",
				HintKey = "Rotate.Hint",
				Highlight = EditorState.Transform,
				DoIt = delegate
				{
					Transform transform = c.Model.transform;
					Vector3 position = transform.position;
					Quaternion rotation = transform.rotation;
					transform.RotateAround(c.WorldCenter(), Vector3.up, 90f);
					UndoManager.Push(new TransformCommand(transform, position, rotation, transform.position, transform.rotation, "Tutorial"));
				},
				OnEnter = delegate
				{
					VoxelEditorSettings.TransformMode = TransformMode.Rotate;
					startYaw = c.Model.transform.eulerAngles.y;
				},
				IsComplete = () => !c.Controller.IsTransformDragging && Mathf.Abs(Mathf.Abs(Mathf.DeltaAngle(startYaw, c.Model.transform.eulerAngles.y)) - 90f) < 8f
			};
		}

		private static TutorialStep Limits(TutorialContext c, List<Vector3Int> lump)
		{
			int heightAtEntry = 0;
			int stackAtEntry = 0;
			bool touchedFloor = false;
			float releasedAt = -1f;
			return new TutorialStep
			{
				TitleKey = "Limits.Title",
				BodyKey = "Limits.Body",
				ClipName = "Limits",
				HintKey = "Limits.Hint",
				Highlight = EditorState.Extrude,
				OnEnter = delegate
				{
					RemoveThroughStack(c, lump);
					ArmExtrude(ExtrudeSelectionMode.Face);
					heightAtEntry = Height(c.Model.Grid);
					stackAtEntry = UndoManager.UndoStack.Count;
					c.Glow.Show(VisibleFaces(c.Model.Grid, Up, null));
				},
				IsComplete = delegate
				{
					if (Height(c.Model.Grid) <= VoxelEditorSettings.MinBoundExtent)
					{
						touchedFloor = true;
					}
					if (!(c.SawOverLimit || touchedFloor) || c.Controller.IsExtrudeDragging)
					{
						releasedAt = -1f;
						return false;
					}
					if (releasedAt < 0f)
					{
						releasedAt = Time.unscaledTime;
					}
					return Restored() || Time.unscaledTime - releasedAt > 2.5f;
				},
				OnExit = delegate
				{
					while (!Restored() && UndoManager.CanUndo && UndoManager.UndoStack.Count > stackAtEntry)
					{
						UndoManager.Undo();
					}
				}
			};
			bool Restored()
			{
				return Height(c.Model.Grid) >= heightAtEntry - 1;
			}
		}

		private static TutorialStep StepInside(TutorialContext c)
		{
			Vector3 target = Vector3.zero;
			return new TutorialStep
			{
				TitleKey = "StepInside.Title",
				BodyKey = "StepInside.Body",
				ClipName = "StepInside",
				HintKey = "StepInside.Hint",
				OnEnter = delegate
				{
					Vector3 preferred = ((c.Camera != null) ? c.Camera.transform.forward : Vector3.forward);
					target = c.FloorPoint() + c.ClearDirection(preferred, 1.5f) * 1.5f;
					c.Marker.ShowRing(target, 0.45f);
				},
				IsComplete = () => VoxelEditorSettings.IsMovementMode && c.Player != null && Flat(c.Player.position - target).magnitude < 0.5f
			};
		}

		private static TutorialStep HuntersEye(TutorialContext c)
		{
			TutorialCinematic cinematic = null;
			return new TutorialStep
			{
				TitleKey = "HuntersEye.Title",
				BodyKey = "HuntersEye.Body",
				OnEnter = delegate
				{
					cinematic = TutorialCinematic.Play(c);
				},
				IsComplete = () => cinematic == null || cinematic.Finished,
				OnExit = delegate
				{
					if (cinematic != null)
					{
						cinematic.Abort();
					}
				}
			};
		}

		private static TutorialStep Done(TutorialContext c)
		{
			TutorialStep tutorialStep = new TutorialStep();
			tutorialStep.TitleKey = "Done.Title";
			tutorialStep.BodyKey = "Done.Body";
			tutorialStep.Unlocks = new EditorState[4]
			{
				EditorState.Transform,
				EditorState.Extrude,
				EditorState.Paint,
				EditorState.LoopCut
			};
			tutorialStep.OnEnter = delegate
			{
				TutorialState.MarkCompleted();
				SaveFirstModel(c);
			};
			return tutorialStep;
		}

		private static VoxelGrid Copy(VoxelGrid grid)
		{
			VoxelGrid voxelGrid = new VoxelGrid();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in grid.Voxels)
			{
				voxelGrid.Set(voxel.Key, voxel.Value);
			}
			return voxelGrid;
		}

		private static List<(Vector3Int Cell, Vector3Int Normal)> VisibleFaces(VoxelGrid grid, Vector3Int normal, ICollection<Vector3Int> only)
		{
			int face = FaceAxes.IndexOf(normal);
			List<(Vector3Int, Vector3Int)> list = new List<(Vector3Int, Vector3Int)>();
			foreach (Vector3Int position in grid.Positions)
			{
				if ((only == null || only.Contains(position)) && grid.IsFaceVisible(position, face))
				{
					list.Add((position, normal));
				}
			}
			return list;
		}

		private static List<(Vector3Int Cell, Vector3Int Normal)> VisibleFacesOf(VoxelGrid grid, IEnumerable<Vector3Int> cells)
		{
			List<(Vector3Int, Vector3Int)> list = new List<(Vector3Int, Vector3Int)>();
			foreach (Vector3Int cell in cells)
			{
				for (int i = 0; i < 6; i++)
				{
					if (grid.IsFaceVisible(cell, i))
					{
						list.Add((cell, FaceAxes.Normals[i]));
					}
				}
			}
			return list;
		}

		private static VoxelGrid Extruded(VoxelGrid grid, Vector3Int normal, int steps, ICollection<Vector3Int> only)
		{
			VoxelGrid voxelGrid = Copy(grid);
			foreach (var item2 in VisibleFaces(grid, normal, only))
			{
				Vector3Int item = item2.Cell;
				grid.TryGet(item, out var data);
				for (int i = 1; i <= steps; i++)
				{
					voxelGrid.Set(item + normal * i, data);
				}
			}
			return voxelGrid;
		}

		private static VoxelGrid Carved(VoxelGrid grid, Vector3Int normal, int depth, ICollection<Vector3Int> only)
		{
			VoxelGrid voxelGrid = Copy(grid);
			foreach (var item2 in VisibleFaces(grid, normal, only))
			{
				Vector3Int item = item2.Cell;
				for (int i = 0; i < depth; i++)
				{
					voxelGrid.Remove(item - normal * i);
				}
			}
			return voxelGrid;
		}

		private static HashSet<Vector3Int> CentralPatch(VoxelGrid grid, Vector3Int normal, int size)
		{
			return Patch(grid, normal, size, centered: true);
		}

		private static HashSet<Vector3Int> Patch(VoxelGrid grid, Vector3Int normal, int size, bool centered)
		{
			FaceAxes.GetBasis(normal, out var right, out var up);
			List<(Vector3Int, Vector3Int)> list = VisibleFaces(grid, normal, null);
			HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
			if (list.Count == 0)
			{
				return hashSet;
			}
			int num = int.MaxValue;
			int num2 = int.MinValue;
			int num3 = int.MaxValue;
			int num4 = int.MinValue;
			foreach (var item3 in list)
			{
				Vector3Int item = item3.Item1;
				int b = Dot(item, right);
				int b2 = Dot(item, up);
				num = Mathf.Min(num, b);
				num2 = Mathf.Max(num2, b);
				num3 = Mathf.Min(num3, b2);
				num4 = Mathf.Max(num4, b2);
			}
			int num5 = (centered ? ((num + num2 + 1) / 2 - size / 2) : (num + 1));
			int num6 = (centered ? ((num3 + num4 + 1) / 2 - size / 2) : (num3 + 1));
			foreach (var item4 in list)
			{
				Vector3Int item2 = item4.Item1;
				int num7 = Dot(item2, right);
				int num8 = Dot(item2, up);
				if (num7 >= num5 && num7 < num5 + size && num8 >= num6 && num8 < num6 + size)
				{
					hashSet.Add(item2);
				}
			}
			return hashSet;
		}

		private static List<Vector3Int> Lump(VoxelGrid grid, int size)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			foreach (Vector3Int item in Patch(grid, Up, size, centered: false))
			{
				for (int i = 1; i <= size; i++)
				{
					Vector3Int vector3Int = item + Up * i;
					if (!grid.Contains(vector3Int))
					{
						list.Add(vector3Int);
					}
				}
			}
			return list;
		}

		private static void ApplyGrid(TutorialContext c, VoxelGrid target)
		{
			if (target == null || c.Model == null)
			{
				return;
			}
			VoxelGrid grid = c.Model.Grid;
			Dictionary<Vector3Int, VoxelData?> dictionary = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<(Vector3Int, int), Color32?> dictionary3 = new Dictionary<(Vector3Int, int), Color32?>();
			Dictionary<(Vector3Int, int), Color32?> dictionary4 = new Dictionary<(Vector3Int, int), Color32?>();
			foreach (KeyValuePair<Vector3Int, VoxelData> voxel in target.Voxels)
			{
				if (!grid.Contains(voxel.Key))
				{
					dictionary[voxel.Key] = null;
					dictionary2[voxel.Key] = voxel.Value;
				}
			}
			foreach (Vector3Int position in grid.Positions)
			{
				if (!target.Contains(position))
				{
					grid.TryGet(position, out var data);
					dictionary[position] = data;
					dictionary2[position] = null;
					RememberFaces(grid, position, dictionary3, dictionary4);
				}
			}
			if (dictionary.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<Vector3Int, VoxelData?> item in dictionary2)
			{
				if (item.Value.HasValue)
				{
					grid.Set(item.Key, item.Value.Value);
				}
				else
				{
					grid.Remove(item.Key);
				}
			}
			c.Model.RebuildMesh();
			UndoManager.Push(new VoxelDiffCommand(c.Model, dictionary, dictionary2, "Tutorial", (dictionary3.Count > 0) ? dictionary3 : null, (dictionary3.Count > 0) ? dictionary4 : null));
		}

		private static void ApplyColor(TutorialContext c, IEnumerable<Vector3Int> cells, Color32 color)
		{
			if (cells == null || c.Model == null)
			{
				return;
			}
			VoxelGrid grid = c.Model.Grid;
			Dictionary<Vector3Int, VoxelData?> dictionary = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<(Vector3Int, int), Color32?> dictionary3 = new Dictionary<(Vector3Int, int), Color32?>();
			Dictionary<(Vector3Int, int), Color32?> dictionary4 = new Dictionary<(Vector3Int, int), Color32?>();
			VoxelData voxelData = new VoxelData(color);
			foreach (Vector3Int cell in cells)
			{
				if (grid.TryGet(cell, out var data))
				{
					dictionary[cell] = data;
					dictionary2[cell] = voxelData;
					RememberFaces(grid, cell, dictionary3, dictionary4);
				}
			}
			if (dictionary.Count == 0)
			{
				return;
			}
			foreach (Vector3Int key in dictionary.Keys)
			{
				grid.Set(key, voxelData);
				grid.ClearFaceColors(key);
			}
			c.Model.RebuildMesh();
			UndoManager.Push(new VoxelDiffCommand(c.Model, dictionary, dictionary2, "Tutorial", (dictionary3.Count > 0) ? dictionary3 : null, (dictionary3.Count > 0) ? dictionary4 : null));
		}

		private static void RememberFaces(VoxelGrid grid, Vector3Int cell, Dictionary<(Vector3Int Position, int Face), Color32?> were, Dictionary<(Vector3Int Position, int Face), Color32?> become)
		{
			for (int i = 0; i < 6; i++)
			{
				if (grid.TryGetFaceColor(cell, i, out var color))
				{
					were[(cell, i)] = color;
					become[(cell, i)] = null;
				}
			}
		}

		private static void RemoveThroughStack(TutorialContext c, List<Vector3Int> cells)
		{
			VoxelGrid grid = c.Model.Grid;
			Dictionary<Vector3Int, VoxelData?> dictionary = new Dictionary<Vector3Int, VoxelData?>();
			Dictionary<Vector3Int, VoxelData?> dictionary2 = new Dictionary<Vector3Int, VoxelData?>();
			foreach (Vector3Int cell in cells)
			{
				if (grid.TryGet(cell, out var data))
				{
					dictionary[cell] = data;
					dictionary2[cell] = null;
					grid.Remove(cell);
				}
			}
			if (dictionary.Count != 0)
			{
				c.Model.RebuildMesh();
				UndoManager.Push(new VoxelDiffCommand(c.Model, dictionary, dictionary2, "Tutorial"));
			}
		}

		private static bool AnyPresent(VoxelGrid grid, List<Vector3Int> cells)
		{
			if (cells == null)
			{
				return false;
			}
			foreach (Vector3Int cell in cells)
			{
				if (grid.Contains(cell))
				{
					return true;
				}
			}
			return false;
		}

		private static Vector3Int FrontNormal(TutorialContext c)
		{
			Vector3 direction = ((c.Camera != null) ? c.Camera.transform.forward : Vector3.forward);
			Vector3 lhs = c.Model.transform.InverseTransformDirection(direction);
			Vector3Int result = Vector3Int.forward;
			float num = float.MinValue;
			Vector3Int[] array = new Vector3Int[4]
			{
				Vector3Int.left,
				Vector3Int.right,
				Vector3Int.forward,
				Vector3Int.back
			};
			foreach (Vector3Int vector3Int in array)
			{
				float num2 = 0f - Vector3.Dot(lhs, vector3Int);
				if (num2 > num)
				{
					num = num2;
					result = vector3Int;
				}
			}
			return result;
		}

		private static int Height(VoxelGrid grid)
		{
			if (!GridBounds.TryCompute(grid, out var min, out var max))
			{
				return 0;
			}
			return max.y - min.y + 1;
		}

		private static int Dot(Vector3Int a, Vector3Int b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		private static Vector3 Flat(Vector3 v)
		{
			return new Vector3(v.x, 0f, v.z);
		}

		private static void SaveFirstModel(TutorialContext c, string nameKey = "FirstModelName")
		{
			List<TemplatePiece> list = TemplateSession.Gather();
			if (list.Count != 0 && !(c.Model == null))
			{
				string text = TemplateStorage.UnusedName(Loc.Get(nameKey));
				string text2 = (VoxelEditorSettings.CurrentTemplateFilePath = TemplateStorage.SaveTemplate(null, text, list, "", "", c.Model.VoxelSize));
				VoxelEditorSettings.CurrentTemplateName = text;
				VoxelEditorSettings.CurrentTemplateTag = "";
				VoxelEditorSettings.CurrentTemplateCategory = "";
				TemplatePortraitService.RequestFor(text2);
				StartingMimic.SelectedPath = text2;
			}
		}

		private static HashSet<Vector3Int> CentralRect(VoxelGrid grid, Vector3Int normal, int width, int height)
		{
			FaceAxes.GetBasis(normal, out var right, out var up);
			Vector3Int b = ((right.y != 0) ? up : right);
			List<(Vector3Int, Vector3Int)> list = VisibleFaces(grid, normal, null);
			HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
			if (list.Count == 0)
			{
				return hashSet;
			}
			int num = int.MaxValue;
			int num2 = int.MinValue;
			int num3 = int.MaxValue;
			int num4 = int.MinValue;
			foreach (var item3 in list)
			{
				Vector3Int item = item3.Item1;
				int b2 = Dot(item, b);
				num = Mathf.Min(num, b2);
				num2 = Mathf.Max(num2, b2);
				num3 = Mathf.Min(num3, item.y);
				num4 = Mathf.Max(num4, item.y);
			}
			width = Mathf.Min(width, num2 - num - 1);
			height = Mathf.Min(height, num4 - num3 - 1);
			if (width < 1 || height < 1)
			{
				return hashSet;
			}
			int num5 = (num + num2 + 1) / 2 - width / 2;
			int num6 = (num3 + num4 + 1) / 2 - height / 2;
			foreach (var item4 in list)
			{
				Vector3Int item2 = item4.Item1;
				int num7 = Dot(item2, b);
				if (num7 >= num5 && num7 < num5 + width && item2.y >= num6 && item2.y < num6 + height)
				{
					hashSet.Add(item2);
				}
			}
			return hashSet;
		}
	}
}

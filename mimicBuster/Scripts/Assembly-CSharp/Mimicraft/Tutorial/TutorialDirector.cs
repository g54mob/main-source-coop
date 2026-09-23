using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.UI;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.Video;

namespace Mimicraft.Tutorial
{
	public class TutorialDirector : MonoBehaviour
	{
		public const string CoreLesson = "core";

		private const float HintAfterSeconds = 30f;

		private const float DoItAfterSeconds = 60f;

		private const float DemoSeconds = 2.5f;

		private const float SettleSeconds = 0.75f;

		[Tooltip("Canvas the coach card is built under. Left empty, the first Canvas in the scene is used.")]
		[SerializeField]
		private Canvas uiCanvas;

		[SerializeField]
		private TutorialCoachView coach;

		private readonly List<TutorialStep> steps = new List<TutorialStep>();

		private int current = -1;

		private string lesson = "core";

		private TutorialContext context;

		private TutorialContext pending;

		private bool hasSettledSample;

		private Vector3 settledPosition;

		private float settledSince;

		private readonly HashSet<EditorState> allowed = new HashSet<EditorState>();

		private bool gated;

		private ExtrudeSelectionMode selectionModeAtStart;

		private ExtrudeMode extrudeModeAtStart;

		private ShapeSubMode shapeSubModeAtStart;

		private float stepStartedAt;

		private bool hintOffered;

		private bool doItOffered;

		private bool demonstrated;

		private float demoUntil;

		private bool coachBound;

		public static TutorialDirector Instance { get; private set; }

		public Transform CanvasTransform => ResolveCanvas();

		private void Awake()
		{
			Instance = this;
			if (!TutorialLaunch.Consume())
			{
				base.enabled = false;
			}
		}

		private void OnEnable()
		{
			UndoManager.EditStepApplied += OnEditStepApplied;
		}

		private void OnDisable()
		{
			UndoManager.EditStepApplied -= OnEditStepApplied;
		}

		private void Start()
		{
			if (!EnsureCoach())
			{
				base.enabled = false;
			}
		}

		private bool EnsureCoach()
		{
			if (coach == null)
			{
				Transform transform = ResolveCanvas();
				if (transform == null)
				{
					Debug.LogWarning("[TutorialDirector] Sahnede Canvas yok - tutorial gösterilemiyor.");
					return false;
				}
				coach = TutorialCoachView.Create(transform);
				coachBound = false;
			}
			if (!coachBound)
			{
				coach.Bind(delegate
				{
					Advance(completed: true);
				}, delegate
				{
					Advance(completed: false);
				}, AskToQuit, ShowHint, DoIt, delegate
				{
					context?.FrameModel();
				});
				coachBound = true;
			}
			return true;
		}

		private Transform ResolveCanvas()
		{
			if (uiCanvas != null)
			{
				return uiCanvas.transform;
			}
			Canvas canvas = Object.FindFirstObjectByType<Canvas>();
			if (!(canvas != null))
			{
				return null;
			}
			return canvas.transform;
		}

		public void StartLesson(string id)
		{
			if (!string.IsNullOrEmpty(id) && EnsureCoach())
			{
				if (current >= 0)
				{
					Finish("replaced");
				}
				lesson = id;
				base.enabled = true;
				if (context != null)
				{
					BeginSteps();
				}
			}
		}

		private void Update()
		{
			if (context == null)
			{
				if (pending == null)
				{
					TutorialContext tutorialContext = new TutorialContext();
					if (!tutorialContext.TryResolve())
					{
						return;
					}
					pending = tutorialContext;
				}
				Vector3 vector = ((pending.Model != null) ? pending.Model.transform.position : Vector3.zero);
				if (!hasSettledSample || (vector - settledPosition).sqrMagnitude > 0.0001f)
				{
					hasSettledSample = true;
					settledPosition = vector;
					settledSince = Time.unscaledTime;
				}
				else if (!(Time.unscaledTime - settledSince < 0.75f))
				{
					Begin(pending);
					pending = null;
				}
			}
			else
			{
				if (current < 0 || current >= steps.Count)
				{
					return;
				}
				TutorialStep tutorialStep = steps[current];
				if (context.Controller != null && context.Controller.IsExtrudeOverLimit)
				{
					context.SawOverLimit = true;
				}
				if (tutorialStep.Progress != null)
				{
					var (done, total) = tutorialStep.Progress();
					coach.SetProgress(done, total);
				}
				coach.SetFindModelVisible(!VoxelEditorSettings.IsMovementMode);
				if (demoUntil > 0f)
				{
					if (!(Time.unscaledTime < demoUntil))
					{
						demoUntil = 0f;
						UndoManager.Undo();
						coach.ShowHint("Coach.NowYou");
						coach.OfferDoIt(keep: true);
					}
				}
				else
				{
					if (context.Controller != null && context.Controller.IsFocusLocked)
					{
						return;
					}
					if (tutorialStep.IsComplete != null && tutorialStep.IsComplete())
					{
						Bounds bounds = context.WorldBounds();
						ImpactEffects.SpawnTutorialCelebration(bounds.center + Vector3.up * bounds.extents.y);
						Advance(completed: true);
						return;
					}
					float num = Time.unscaledTime - stepStartedAt;
					bool flag = tutorialStep.IsComplete != null;
					if (!hintOffered && flag && tutorialStep.HintKey != null && num > 30f)
					{
						hintOffered = true;
						coach.OfferHint();
					}
					if (!doItOffered && flag && tutorialStep.DoIt != null && num > 60f)
					{
						doItOffered = true;
						coach.OfferDoIt(!tutorialStep.DemoByUndo);
					}
				}
			}
		}

		private void Begin(TutorialContext resolved)
		{
			context = resolved;
			selectionModeAtStart = VoxelEditorSettings.ExtrudeSelectionMode;
			extrudeModeAtStart = VoxelEditorSettings.ExtrudeModeSetting;
			shapeSubModeAtStart = VoxelEditorSettings.ShapeSubModeSetting;
			if (lesson == "core")
			{
				VoxelEditorSettings.SlimAxisAllowed = false;
				PlayerVoxelBody.AlignDistanceUnlimited = true;
			}
			BeginSteps();
		}

		private void BeginSteps()
		{
			if (lesson != "core" && PlayerVoxelBody.CanLocalReset(report: false))
			{
				PlayerVoxelBody.RequestLocalReset();
			}
			steps.Clear();
			VoxelEditorSettings.TutorialLocksExtras = true;
			if (lesson == "core")
			{
				steps.AddRange(TutorialCurriculum.Build(context));
				gated = true;
				allowed.Clear();
				allowed.Add(EditorState.View);
				ApplyGate();
			}
			else
			{
				steps.AddRange(TutorialCurriculum.BuildLesson(lesson, context));
				gated = false;
				ClearGate();
			}
			if (steps.Count == 0)
			{
				Debug.LogWarning("[TutorialDirector] '" + lesson + "' diye bir ders yok.");
				base.enabled = false;
			}
			else
			{
				TutorialTelemetry.BeginSession(lesson);
				Show(0);
			}
		}

		private void Advance(bool completed)
		{
			TutorialTelemetry.StepEnded(completed);
			if (current + 1 >= steps.Count)
			{
				Finish("finished");
			}
			else
			{
				Show(current + 1);
			}
		}

		private void Show(int index)
		{
			Exit(current);
			current = index;
			TutorialStep tutorialStep = steps[index];
			context.ClearLatches();
			context.HideMarkers();
			if (gated && tutorialStep.Unlocks != null)
			{
				EditorState[] unlocks = tutorialStep.Unlocks;
				foreach (EditorState item in unlocks)
				{
					allowed.Add(item);
				}
				ApplyGate();
			}
			if (tutorialStep.UnlocksExtras)
			{
				VoxelEditorSettings.TutorialLocksExtras = false;
			}
			stepStartedAt = Time.unscaledTime;
			hintOffered = (doItOffered = (demonstrated = false));
			demoUntil = 0f;
			if (context.Controller != null)
			{
				context.Controller.AbandonGesture();
			}
			tutorialStep.OnEnter?.Invoke();
			context.Ghost.SetTarget(tutorialStep.Target?.Invoke());
			coach.Show(tutorialStep, index, steps.Count);
			coach.ShowClip(LoadClip(tutorialStep.ClipName));
			TutorialTelemetry.StepShown(tutorialStep.TitleKey);
		}

		private static VideoClip LoadClip(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return Resources.Load<VideoClip>("Tutorial/Clips/" + name);
			}
			return null;
		}

		private void Exit(int index)
		{
			if (index >= 0 && index < steps.Count)
			{
				steps[index].OnExit?.Invoke();
			}
		}

		private void Finish(string reason)
		{
			if (demoUntil > 0f)
			{
				demoUntil = 0f;
				UndoManager.Undo();
			}
			Exit(current);
			bool num = current >= 0;
			current = -1;
			if (context != null)
			{
				context.HideMarkers();
				VoxelEditorSettings.ExtrudeSelectionMode = selectionModeAtStart;
				VoxelEditorSettings.ExtrudeModeSetting = extrudeModeAtStart;
				VoxelEditorSettings.ShapeSubModeSetting = shapeSubModeAtStart;
			}
			ClearGate();
			VoxelEditorSettings.TutorialLocksExtras = false;
			VoxelEditorSettings.PaintMask = null;
			VoxelEditorSettings.SlimAxisAllowed = true;
			PlayerVoxelBody.AlignDistanceUnlimited = false;
			if (coach != null)
			{
				coach.ShowClip(null);
				coach.Hide();
			}
			base.enabled = false;
			if (num)
			{
				TutorialTelemetry.EndSession(reason);
				ToastView.Instance?.Show(Loc.Get((lesson == "core") ? "Finished" : "Lesson.Finished"));
				if (lesson == "core" && reason == "finished")
				{
					AskWhereNext();
				}
			}
		}

		private void AskWhereNext()
		{
			bool quickPlay = QuickPlay.IsAvailable;
			DialogView.Show(new DialogRequest(Loc.Get("WhereNext"), Loc.Get("WhereNext.Shape"), Loc.Get("WhereNext.Stay"), Loc.Get(quickPlay ? "WhereNext.QuickPlay" : "WhereNext.Menu")), delegate(DialogAnswer answer, string _)
			{
				switch (answer)
				{
				case DialogAnswer.Confirm:
					StartLesson("tv");
					break;
				case DialogAnswer.Alternate:
					if (quickPlay)
					{
						QuickPlay.RequestOnMenu();
						PauseMenuView.LeaveToMenu();
					}
					else
					{
						PauseMenuView.LeaveToMenu();
					}
					break;
				}
			});
		}

		private void AskToQuit()
		{
			if (current < 0)
			{
				return;
			}
			if (DialogView.Instance == null)
			{
				Finish("quit");
				return;
			}
			string asked = lesson;
			DialogView.Confirm(Loc.Get((asked == "core") ? "Coach.ConfirmQuitTutorial" : "Coach.ConfirmQuitLesson"), delegate
			{
				if (current >= 0 && lesson == asked)
				{
					Finish("quit");
				}
			});
		}

		private void ShowHint()
		{
			if (current >= 0 && current < steps.Count)
			{
				TutorialStep tutorialStep = steps[current];
				if (tutorialStep.HintKey != null)
				{
					TutorialTelemetry.HintUsed();
					coach.ShowHint(tutorialStep.HintKey);
				}
			}
		}

		private void DoIt()
		{
			if (current < 0 || current >= steps.Count || context == null)
			{
				return;
			}
			TutorialStep tutorialStep = steps[current];
			if (tutorialStep.DoIt != null && !(demoUntil > 0f) && (!(context.Controller != null) || !context.Controller.IsFocusLocked))
			{
				TutorialTelemetry.DoItUsed();
				context.Controller?.AbandonGesture();
				if (tutorialStep.DemoByUndo && !demonstrated)
				{
					demonstrated = true;
					tutorialStep.DoIt();
					demoUntil = Time.unscaledTime + 2.5f;
					coach.HideDoIt();
				}
				else
				{
					tutorialStep.DoIt();
				}
			}
		}

		private void ApplyGate()
		{
			VoxelEditorSettings.TutorialGate = (EditorState tool) => allowed.Contains(tool);
		}

		private static void ClearGate()
		{
			VoxelEditorSettings.TutorialGate = null;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			ClearGate();
			VoxelEditorSettings.TutorialLocksExtras = false;
			if (current >= 0)
			{
				TutorialTelemetry.EndSession("quit");
			}
			context?.Destroy();
		}

		private void OnEditStepApplied(IUndoableCommand command, UndoManager.EditStepKind kind)
		{
			if (context == null)
			{
				return;
			}
			if (kind == UndoManager.EditStepKind.Undo)
			{
				context.DidUndo = true;
			}
			else if (!(command is ExtrudeCreateCommand))
			{
				if (!(command is LoopCutCommand))
				{
					if (!(command is TransformCommand))
					{
						if (!(command is VoxelDiffCommand))
						{
							return;
						}
						if (VoxelEditorSettings.CurrentTool == EditorState.Extrude)
						{
							if (VoxelEditorSettings.ShapeSubModeSetting == ShapeSubMode.Bevel)
							{
								context.DidBevel = true;
							}
							else
							{
								context.DidExtrude = true;
							}
						}
						else if (VoxelEditorSettings.CurrentTool == EditorState.Paint)
						{
							context.DidPaint = true;
							if (VoxelEditorSettings.PaintMode == PaintMode.Pattern)
							{
								context.DidPattern = true;
							}
						}
					}
					else
					{
						context.DidTransform = true;
					}
				}
				else
				{
					context.DidLoopCut = true;
				}
			}
			else
			{
				context.DidCreate = true;
				context.DidExtrude = true;
			}
		}
	}
}

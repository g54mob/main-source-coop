using System;
using System.Diagnostics;
using System.Text;
using DunGen.Graph;
using UnityEngine;

namespace DunGen.Analysis
{
	[AddComponentMenu("DunGen/Analysis/Runtime Analyzer")]
	public sealed class RuntimeAnalyzer : MonoBehaviour
	{
		public enum SeedMode
		{
			Random = 0,
			Incremental = 1,
			Fixed = 2
		}

		public DungeonFlow DungeonFlow;

		public int Iterations = 100;

		public int MaxFailedAttempts = 20;

		public bool RunOnStart = true;

		public float MaximumAnalysisTime;

		public SeedMode SeedGenerationMode;

		public int Seed;

		public bool ClearDungeonOnCompletion = true;

		public bool AllowTilePooling;

		private DungeonGenerator generator = new DungeonGenerator();

		private GenerationAnalysis analysis;

		private readonly StringBuilder infoText = new StringBuilder();

		private bool finishedEarly;

		private bool prevShouldRandomizeSeed;

		private int targetIterations;

		private int remainingIterations;

		private Stopwatch analysisTime;

		private bool generateNextFrame;

		private int currentSeed;

		private RandomStream randomStream;

		public int CurrentIterations => targetIterations - remainingIterations;

		public static event RuntimeAnalyzerDelegate AnalysisStarted;

		public static event RuntimeAnalyzerDelegate AnalysisComplete;

		public static event AnalysisUpdatedDelegate AnalysisUpdated;

		private void Start()
		{
			if (RunOnStart)
			{
				RunAnalysis();
			}
		}

		[Obsolete("Use RunAnalysis() instead")]
		public void Analyze()
		{
			RunAnalysis();
		}

		public void RunAnalysis()
		{
			bool flag = false;
			if (DungeonFlow == null)
			{
				UnityEngine.Debug.LogError("No DungeonFlow assigned to analyser");
			}
			else if (Iterations <= 0)
			{
				UnityEngine.Debug.LogError("Iteration count must be greater than 0");
			}
			else if (MaxFailedAttempts <= 0)
			{
				UnityEngine.Debug.LogError("Max failed attempt count must be greater than 0");
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				RuntimeAnalyzer.AnalysisStarted?.Invoke(this);
				prevShouldRandomizeSeed = generator.ShouldRandomizeSeed;
				generator.IsAnalysis = true;
				generator.DungeonFlow = DungeonFlow;
				generator.MaxAttemptCount = MaxFailedAttempts;
				generator.ShouldRandomizeSeed = false;
				generator.AllowTilePooling = AllowTilePooling;
				analysis = new GenerationAnalysis(Iterations);
				analysisTime = Stopwatch.StartNew();
				remainingIterations = (targetIterations = Iterations);
				randomStream = new RandomStream(Seed);
				generator.OnGenerationStatusChanged += OnGenerationStatusChanged;
				GenerateNext();
			}
		}

		private void GenerateNext()
		{
			switch (SeedGenerationMode)
			{
			case SeedMode.Random:
				currentSeed = randomStream.Next();
				break;
			case SeedMode.Incremental:
				currentSeed++;
				break;
			case SeedMode.Fixed:
				currentSeed = Seed;
				break;
			}
			generator.Seed = currentSeed;
			generator.Generate();
		}

		private void Update()
		{
			if (MaximumAnalysisTime > 0f && analysisTime.Elapsed.TotalSeconds >= (double)MaximumAnalysisTime)
			{
				remainingIterations = 0;
				finishedEarly = true;
			}
			if (generateNextFrame)
			{
				generateNextFrame = false;
				GenerateNext();
			}
		}

		private void CompleteAnalysis()
		{
			analysisTime.Stop();
			analysis.CalculateMetrics();
			if (ClearDungeonOnCompletion)
			{
				UnityUtil.Destroy(generator.Root);
			}
			OnAnalysisComplete();
			RuntimeAnalyzer.AnalysisComplete?.Invoke(this);
		}

		private void OnGenerationStatusChanged(DungeonGenerator generator, GenerationStatus status)
		{
			if (status == GenerationStatus.Complete || status == GenerationStatus.Failed)
			{
				if (status == GenerationStatus.Complete)
				{
					analysis.IncrementSuccessCount();
					analysis.Add(generator.GenerationStats);
				}
				RuntimeAnalyzer.AnalysisUpdated?.Invoke(this, analysis, generator.GenerationStats, CurrentIterations, targetIterations);
				remainingIterations--;
				if (remainingIterations <= 0)
				{
					generator.OnGenerationStatusChanged -= OnGenerationStatusChanged;
					CompleteAnalysis();
				}
				else
				{
					generateNextFrame = true;
				}
			}
		}

		private void OnAnalysisComplete()
		{
			generator.ShouldRandomizeSeed = prevShouldRandomizeSeed;
			infoText.Length = 0;
			if (finishedEarly)
			{
				infoText.AppendLine("[ Reached maximum analysis time before the target number of iterations was reached ]");
			}
			infoText.AppendFormat("Iterations: {0}, Max Failed Attempts: {1}", finishedEarly ? analysis.IterationCount : analysis.TargetIterationCount, MaxFailedAttempts);
			infoText.AppendFormat("\nTotal Analysis Time: {0:0.00} seconds", analysisTime.Elapsed.TotalSeconds);
			infoText.AppendFormat("\nDungeons successfully generated: {0}% ({1} failed)", Mathf.RoundToInt(analysis.SuccessPercentage), analysis.TargetIterationCount - analysis.SuccessCount);
			infoText.AppendLine();
			infoText.AppendLine();
			infoText.Append("## TIME TAKEN (in milliseconds) ##");
			GenerationStatus[] measurableSteps = GenerationAnalysis.MeasurableSteps;
			for (int i = 0; i < measurableSteps.Length; i++)
			{
				GenerationStatus step = measurableSteps[i];
				AddInfoEntry(infoText, step.ToString(), analysis.GetGenerationStepData(step));
			}
			infoText.Append("\n\t-------------------------------------------------------");
			AddInfoEntry(infoText, "Total", analysis.TotalTime);
			infoText.AppendLine();
			infoText.AppendLine();
			infoText.AppendLine("## ROOM DATA ##");
			AddInfoEntry(infoText, "Main Path Rooms", analysis.MainPathRoomCount);
			AddInfoEntry(infoText, "Branch Path Rooms", analysis.BranchPathRoomCount);
			infoText.Append("\n\t-------------------");
			AddInfoEntry(infoText, "Total", analysis.TotalRoomCount);
			infoText.AppendLine();
			infoText.AppendLine();
			infoText.AppendFormat("Retry Count: {0}", analysis.TotalRetries);
			static void AddInfoEntry(StringBuilder stringBuilder, string title, NumberSetData data)
			{
				string arg = new string(' ', 20 - title.Length);
				stringBuilder.Append($"\n\t{title}:{arg}\t{data}");
			}
		}

		private void OnGUI()
		{
			if (analysis == null || infoText == null || infoText.Length == 0)
			{
				string text = ((analysis.SuccessCount < analysis.IterationCount) ? ("\nFailed Dungeons: " + (analysis.IterationCount - analysis.SuccessCount)) : "");
				GUILayout.Label($"Analysing... {CurrentIterations} / {targetIterations} ({(float)CurrentIterations / (float)targetIterations * 100f:0.0}%){text}");
			}
			else
			{
				GUILayout.Label(infoText.ToString());
			}
		}
	}
}

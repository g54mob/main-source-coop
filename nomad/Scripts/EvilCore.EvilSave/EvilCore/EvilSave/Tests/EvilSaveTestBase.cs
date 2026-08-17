using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace EvilCore.EvilSave.Tests
{
	public abstract class EvilSaveTestBase : MonoBehaviour
	{
		private readonly List<EvilSaveTestResult> _results = new List<EvilSaveTestResult>();

		public abstract string CategoryName { get; }

		public IReadOnlyList<EvilSaveTestResult> Results => _results;

		public bool HasRun { get; private set; }

		public bool IsRunning { get; private set; }

		public int PassedCount { get; private set; }

		public int FailedCount { get; private set; }

		protected void ClearResults()
		{
			_results.Clear();
			PassedCount = 0;
			FailedCount = 0;
			HasRun = false;
			IsRunning = true;
		}

		protected void MarkComplete()
		{
			HasRun = true;
			IsRunning = false;
		}

		protected void Assert<T>(string testName, T expected, T actual)
		{
			if (EqualityComparer<T>.Default.Equals(expected, actual))
			{
				PassedCount++;
				_results.Add(new EvilSaveTestResult
				{
					TestName = testName,
					Passed = true
				});
			}
			else
			{
				FailedCount++;
				string text = $"Expected: {expected} | Got: {actual}";
				_results.Add(new EvilSaveTestResult
				{
					TestName = testName,
					Passed = false,
					Detail = text
				});
				EvilLogger.LogError("<color=red>[FAIL]</color> " + testName + " | " + text, "Assert", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Demo\\Scripts\\EvilSaveTestResult.cs", 53);
			}
		}

		protected void AssertTrue(string testName, bool condition, string detail = "")
		{
			if (condition)
			{
				PassedCount++;
				_results.Add(new EvilSaveTestResult
				{
					TestName = testName,
					Passed = true
				});
			}
			else
			{
				FailedCount++;
				_results.Add(new EvilSaveTestResult
				{
					TestName = testName,
					Passed = false,
					Detail = detail
				});
				EvilLogger.LogError("<color=red>[FAIL]</color> " + testName + (string.IsNullOrEmpty(detail) ? "" : (" | " + detail)), "AssertTrue", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Demo\\Scripts\\EvilSaveTestResult.cs", 69);
			}
		}

		protected void AssertFloatEqual(string testName, float expected, float actual, float tolerance = 0.0001f)
		{
			if (Mathf.Abs(expected - actual) <= tolerance)
			{
				PassedCount++;
				_results.Add(new EvilSaveTestResult
				{
					TestName = testName,
					Passed = true
				});
			}
			else
			{
				FailedCount++;
				string text = $"Expected: {expected} | Got: {actual}";
				_results.Add(new EvilSaveTestResult
				{
					TestName = testName,
					Passed = false,
					Detail = text
				});
				EvilLogger.LogError("<color=red>[FAIL]</color> " + testName + " | " + text, "AssertFloatEqual", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Demo\\Scripts\\EvilSaveTestResult.cs", 86);
			}
		}

		protected void LogSummary(string category)
		{
			_ = PassedCount;
			_ = FailedCount;
			_ = FailedCount;
			MarkComplete();
		}
	}
}

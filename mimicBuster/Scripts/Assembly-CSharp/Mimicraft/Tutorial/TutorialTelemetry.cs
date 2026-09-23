using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mimicraft.Analytics;
using UnityEngine;

namespace Mimicraft.Tutorial
{
	public static class TutorialTelemetry
	{
		[Serializable]
		public class StepRecord
		{
			public string key;

			public float seconds;

			public bool hint;

			public bool doIt;

			public string outcome;
		}

		[Serializable]
		public class SessionRecord
		{
			public string lesson;

			public string startedAt;

			public string endedAt;

			public string reason;

			public List<StepRecord> steps = new List<StepRecord>();
		}

		[Serializable]
		private class Store
		{
			public List<SessionRecord> sessions = new List<SessionRecord>();
		}

		private const int KeepSessions = 30;

		private static SessionRecord current;

		private static int currentStep = -1;

		private static float stepStartedAt;

		private static string FilePath => Path.Combine(Application.persistentDataPath, "tutorial-telemetry.json");

		public static void BeginSession(string lesson)
		{
			if (current != null)
			{
				EndSession("replaced");
			}
			current = new SessionRecord
			{
				lesson = lesson,
				startedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
			};
			currentStep = -1;
			Telemetry.Send("tutorial_start", ("lesson", lesson));
		}

		public static void StepShown(string key)
		{
			if (current != null)
			{
				CloseStep("left");
				current.steps.Add(new StepRecord
				{
					key = key
				});
				currentStep = current.steps.Count - 1;
				stepStartedAt = Time.unscaledTime;
			}
		}

		public static void HintUsed()
		{
			StepRecord stepRecord = Current();
			if (stepRecord != null)
			{
				stepRecord.hint = true;
			}
		}

		public static void DoItUsed()
		{
			StepRecord stepRecord = Current();
			if (stepRecord != null)
			{
				stepRecord.doIt = true;
			}
		}

		public static void StepEnded(bool completed)
		{
			CloseStep(completed ? "done" : "skipped");
		}

		public static void EndSession(string reason)
		{
			if (current == null)
			{
				return;
			}
			CloseStep("left");
			current.endedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			current.reason = reason;
			int num = 0;
			float num2 = 0f;
			foreach (StepRecord step in current.steps)
			{
				num2 += step.seconds;
				if (step.outcome == "done")
				{
					num++;
				}
			}
			Telemetry.Send("tutorial_end", ("lesson", current.lesson), ("reason", reason), ("elapsed_seconds", Mathf.RoundToInt(num2)), ("steps_done", num), ("steps_total", current.steps.Count));
			Store store = Load();
			store.sessions.Add(current);
			while (store.sessions.Count > 30)
			{
				store.sessions.RemoveAt(0);
			}
			Save(store);
			current = null;
			currentStep = -1;
		}

		private static StepRecord Current()
		{
			if (current == null || currentStep < 0 || currentStep >= current.steps.Count)
			{
				return null;
			}
			return current.steps[currentStep];
		}

		private static void CloseStep(string outcome)
		{
			StepRecord stepRecord = Current();
			if (stepRecord != null && stepRecord.outcome == null)
			{
				stepRecord.seconds = Time.unscaledTime - stepStartedAt;
				stepRecord.outcome = outcome;
				Telemetry.Send("tutorial_step", ("lesson", (current != null) ? current.lesson : ""), ("step", stepRecord.key), ("index", currentStep), ("elapsed_seconds", Mathf.RoundToInt(stepRecord.seconds)), ("hint", stepRecord.hint), ("do_it", stepRecord.doIt), ("outcome", outcome));
			}
		}

		private static Store Load()
		{
			try
			{
				if (File.Exists(FilePath))
				{
					return JsonUtility.FromJson<Store>(File.ReadAllText(FilePath)) ?? new Store();
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TutorialTelemetry] Kayit okunamadi (" + ex.GetType().Name + "); sifirdan baslaniyor.");
			}
			return new Store();
		}

		private static void Save(Store store)
		{
			try
			{
				File.WriteAllText(FilePath, JsonUtility.ToJson(store, prettyPrint: true));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[TutorialTelemetry] Kayit yazilamadi (" + ex.GetType().Name + ").");
			}
		}

		public static void Clear()
		{
			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);
			}
		}

		public static string Report()
		{
			Store store = Load();
			StringBuilder stringBuilder = new StringBuilder();
			if (store.sessions.Count == 0 && current == null)
			{
				return "Henuz kayit yok - tutorial hic oynanmamis.";
			}
			SessionRecord sessionRecord = current ?? store.sessions[store.sessions.Count - 1];
			stringBuilder.AppendLine("Son oturum: " + sessionRecord.lesson + "  " + sessionRecord.startedAt + "  (" + (sessionRecord.reason ?? "suruyor") + ")");
			foreach (StepRecord step in sessionRecord.steps)
			{
				stringBuilder.AppendLine($"  {step.key,-18} {step.seconds,6:0.0} sn  {step.outcome,-8}" + (step.hint ? "  ipucu" : "") + (step.doIt ? "  benim-icin-yap" : ""));
			}
			if (store.sessions.Count == 0)
			{
				return stringBuilder.ToString();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine($"Toplam {store.sessions.Count} oturum, kart basina:");
			stringBuilder.AppendLine(string.Format("  {0,-18} {1,3}  {2,7}  {3,5}  {4,7}  {5,7}", "kart", "n", "ortanca", "ipucu", "yapildi", "atlandi"));
			foreach (IGrouping<string, StepRecord> item in from s in store.sessions.SelectMany((SessionRecord s) => s.steps)
				where s.key != null
				group s by s.key)
			{
				List<float> list = (from s in item
					select s.seconds into s
					orderby s
					select s).ToList();
				float num = list[list.Count / 2];
				int count = list.Count;
				int part = item.Count((StepRecord s) => s.hint);
				int part2 = item.Count((StepRecord s) => s.doIt);
				int part3 = item.Count((StepRecord s) => s.outcome == "skipped");
				stringBuilder.AppendLine($"  {item.Key,-18} {count,3}  {num,6:0.0}s  {Percent(part, count),5}  {Percent(part2, count),7}  {Percent(part3, count),7}");
			}
			return stringBuilder.ToString();
		}

		private static string Percent(int part, int whole)
		{
			if (whole != 0)
			{
				return $"%{Mathf.RoundToInt(100f * (float)part / (float)whole)}";
			}
			return "-";
		}
	}
}

using System.Collections.Generic;
using Features.LevelModule.Scripts;
using UnityEngine;

namespace Features.KrakenModule.Scripts.Data
{
	public class KrakenAppearTriggerWindowModel : ILevelCleanup
	{
		private readonly List<float> _itemTriggerTimes = new List<float>();

		private readonly HashSet<float> _quotaThresholdsTriggered = new HashSet<float>();

		public void RecordItemTriggered(float timestamp, float windowSeconds)
		{
			Prune(timestamp, windowSeconds);
			_itemTriggerTimes.Add(timestamp);
		}

		public int GetItemTriggerCount(float timestamp, float windowSeconds)
		{
			Prune(timestamp, windowSeconds);
			return _itemTriggerTimes.Count;
		}

		public bool TryMarkQuotaThresholdTriggered(float threshold)
		{
			float item = Mathf.Clamp01(threshold);
			return _quotaThresholdsTriggered.Add(item);
		}

		public void Cleanup()
		{
			_itemTriggerTimes.Clear();
			_quotaThresholdsTriggered.Clear();
		}

		private void Prune(float timestamp, float windowSeconds)
		{
			float num = timestamp - Mathf.Max(0.01f, windowSeconds);
			for (int num2 = _itemTriggerTimes.Count - 1; num2 >= 0; num2--)
			{
				if (_itemTriggerTimes[num2] < num)
				{
					_itemTriggerTimes.RemoveAt(num2);
				}
			}
		}
	}
}

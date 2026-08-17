using System;
using System.Collections.Generic;

namespace NomadDrive.Features.Objectives
{
	public static class ObjectivesEventBus
	{
		private static readonly Dictionary<string, float> _lastRatioByKey = new Dictionary<string, float>();

		private static readonly Dictionary<string, float> _lastAmountByKey = new Dictionary<string, float>();

		public static event Action<ObjectiveSignal, object> SignalRaised;

		public static event Action<string, float> RatioRaised;

		public static event Action<string, float> AmountRaised;

		public static void Raise(ObjectiveSignal signal, object payload = null)
		{
			ObjectivesEventBus.SignalRaised?.Invoke(signal, payload);
		}

		public static void RaiseRatio(string key, float ratio)
		{
			if (!string.IsNullOrEmpty(key))
			{
				_lastRatioByKey[key] = ratio;
			}
			ObjectivesEventBus.RatioRaised?.Invoke(key, ratio);
		}

		public static void RaiseAmount(string key, float amount)
		{
			if (!string.IsNullOrEmpty(key))
			{
				_lastAmountByKey[key] = amount;
			}
			ObjectivesEventBus.AmountRaised?.Invoke(key, amount);
		}

		public static bool TryGetLastRatio(string key, out float ratio)
		{
			if (!string.IsNullOrEmpty(key) && _lastRatioByKey.TryGetValue(key, out ratio))
			{
				return true;
			}
			ratio = 0f;
			return false;
		}

		public static bool TryGetLastAmount(string key, out float amount)
		{
			if (!string.IsNullOrEmpty(key) && _lastAmountByKey.TryGetValue(key, out amount))
			{
				return true;
			}
			amount = 0f;
			return false;
		}
	}
}

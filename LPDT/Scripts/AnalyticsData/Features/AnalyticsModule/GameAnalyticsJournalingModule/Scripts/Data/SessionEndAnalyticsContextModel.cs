using System.Collections.Generic;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data.Configurations;
using Features.GameCycle.Scripts.SessionCleanup;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	public class SessionEndAnalyticsContextModel : ISessionCleanup
	{
		private readonly SessionEndAnalyticsPlacePriorityConfiguration _placePriorityConfiguration;

		private readonly SessionEndAnalyticsReasonPriorityConfiguration _reasonPriorityConfiguration;

		private readonly Dictionary<SessionEndAnalyticsPlace, int> _activePlaces = new Dictionary<SessionEndAnalyticsPlace, int>();

		private readonly List<SessionEndAnalyticsPlace> _placesToRemove = new List<SessionEndAnalyticsPlace>();

		private SessionEndAnalyticsReason _reason;

		private int _reasonPriority;

		public SessionEndAnalyticsContextModel(SessionEndAnalyticsPlacePriorityConfiguration placePriorityConfiguration, SessionEndAnalyticsReasonPriorityConfiguration reasonPriorityConfiguration)
		{
			_placePriorityConfiguration = placePriorityConfiguration;
			_reasonPriorityConfiguration = reasonPriorityConfiguration;
			_reasonPriority = _reasonPriorityConfiguration.GetPriority(SessionEndAnalyticsReason.Unknown);
		}

		public bool TrySetReason(SessionEndAnalyticsReason reason)
		{
			int priority = _reasonPriorityConfiguration.GetPriority(reason);
			if (priority < _reasonPriority)
			{
				return false;
			}
			_reason = reason;
			_reasonPriority = priority;
			return true;
		}

		public void RegisterPlace(SessionEndAnalyticsPlaceKind kind, int levelNumber = 0)
		{
			if (kind != SessionEndAnalyticsPlaceKind.Unknown)
			{
				RemovePlaces(kind);
				SessionEndAnalyticsPlace key = new SessionEndAnalyticsPlace(kind, levelNumber);
				int priority = _placePriorityConfiguration.GetPriority(kind);
				_activePlaces[key] = priority;
			}
		}

		public void UnregisterPlace(SessionEndAnalyticsPlaceKind kind, int levelNumber = 0)
		{
			if (kind != SessionEndAnalyticsPlaceKind.Unknown)
			{
				RemovePlaces(kind);
			}
		}

		public bool HasPlace(SessionEndAnalyticsPlaceKind kind, int levelNumber = 0)
		{
			return _activePlaces.ContainsKey(new SessionEndAnalyticsPlace(kind, levelNumber));
		}

		public void ClearPlaces()
		{
			_activePlaces.Clear();
		}

		public SessionEndAnalyticsPlace GetCurrentPlace()
		{
			if (_activePlaces.Count == 0)
			{
				return SessionEndAnalyticsPlace.Unknown;
			}
			SessionEndAnalyticsPlace result = SessionEndAnalyticsPlace.Unknown;
			int num = int.MinValue;
			foreach (KeyValuePair<SessionEndAnalyticsPlace, int> activePlace in _activePlaces)
			{
				if (activePlace.Value > num)
				{
					num = activePlace.Value;
					result = activePlace.Key;
				}
			}
			return result;
		}

		public SessionEndAnalyticsReason GetCurrentReason()
		{
			return _reason;
		}

		public void Reset()
		{
			ClearPlaces();
			_reason = SessionEndAnalyticsReason.Unknown;
			_reasonPriority = _reasonPriorityConfiguration.GetPriority(SessionEndAnalyticsReason.Unknown);
		}

		public void Cleanup()
		{
			Reset();
		}

		private void RemovePlaces(SessionEndAnalyticsPlaceKind kind)
		{
			_placesToRemove.Clear();
			foreach (SessionEndAnalyticsPlace key in _activePlaces.Keys)
			{
				if (key.Kind == kind)
				{
					_placesToRemove.Add(key);
				}
			}
			foreach (SessionEndAnalyticsPlace item in _placesToRemove)
			{
				_activePlaces.Remove(item);
			}
		}
	}
}

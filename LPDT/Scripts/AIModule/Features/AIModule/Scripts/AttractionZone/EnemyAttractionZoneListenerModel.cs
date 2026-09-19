using System.Collections.Generic;

namespace Features.AIModule.Scripts.AttractionZone
{
	public class EnemyAttractionZoneListenerModel
	{
		private readonly Dictionary<EnemyType, List<IEnemyAttractionZoneCallbackListener>> _listenersByType = new Dictionary<EnemyType, List<IEnemyAttractionZoneCallbackListener>>();

		private readonly Dictionary<int, IEnemyAttractionZoneCallbackListener> _listenerByInstance = new Dictionary<int, IEnemyAttractionZoneCallbackListener>();

		private readonly Dictionary<int, AttractionZoneData> _activeZones = new Dictionary<int, AttractionZoneData>();

		public IReadOnlyDictionary<int, AttractionZoneData> ActiveZones => _activeZones;

		public void RegisterListener(EnemyType enemyType, int enemyInstance, IEnemyAttractionZoneCallbackListener listener)
		{
			if (!_listenersByType.TryGetValue(enemyType, out var value))
			{
				value = new List<IEnemyAttractionZoneCallbackListener>();
				_listenersByType.Add(enemyType, value);
			}
			value.Add(listener);
			_listenerByInstance[enemyInstance] = listener;
		}

		public void UnregisterListener(EnemyType enemyType, int enemyInstance)
		{
			if (!_listenerByInstance.TryGetValue(enemyInstance, out var value))
			{
				return;
			}
			if (_listenersByType.TryGetValue(enemyType, out var value2))
			{
				value2.Remove(value);
				if (value2.Count == 0)
				{
					_listenersByType.Remove(enemyType);
				}
			}
			_listenerByInstance.Remove(enemyInstance);
		}

		public List<IEnemyAttractionZoneCallbackListener> GetAllListeners()
		{
			List<IEnemyAttractionZoneCallbackListener> list = new List<IEnemyAttractionZoneCallbackListener>();
			foreach (List<IEnemyAttractionZoneCallbackListener> value in _listenersByType.Values)
			{
				list.AddRange(value);
			}
			return list;
		}

		public void AddZone(AttractionZoneData zone)
		{
			_activeZones[zone.ZoneId] = zone;
		}

		public bool RemoveZone(int zoneId)
		{
			return _activeZones.Remove(zoneId);
		}

		public bool TryGetZone(int zoneId, out AttractionZoneData zone)
		{
			return _activeZones.TryGetValue(zoneId, out zone);
		}
	}
}

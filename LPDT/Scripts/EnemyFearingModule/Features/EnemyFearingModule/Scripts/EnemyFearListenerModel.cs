using System.Collections.Generic;
using Features.AIModule.Scripts;

namespace Features.EnemyFearingModule.Scripts
{
	public class EnemyFearListenerModel
	{
		private readonly Dictionary<EnemyType, List<IEnemyFearCallbackListener>> _enemiesFearCallbackListener = new Dictionary<EnemyType, List<IEnemyFearCallbackListener>>();

		private readonly Dictionary<int, IEnemyFearCallbackListener> _enemyFearCallbackListener = new Dictionary<int, IEnemyFearCallbackListener>();

		public void RegisterFearListener(EnemyType enemyType, int enemyInstance, IEnemyFearCallbackListener fearCallbackListener)
		{
			if (!_enemiesFearCallbackListener.TryGetValue(enemyType, out var value))
			{
				value = new List<IEnemyFearCallbackListener>();
				_enemiesFearCallbackListener.Add(enemyType, value);
			}
			value.Add(fearCallbackListener);
			_enemyFearCallbackListener[enemyInstance] = fearCallbackListener;
		}

		public void UnregisterFearListener(EnemyType enemyType, int enemyInstance)
		{
			if (!_enemyFearCallbackListener.TryGetValue(enemyInstance, out var value))
			{
				return;
			}
			if (_enemiesFearCallbackListener.TryGetValue(enemyType, out var value2))
			{
				value2.Remove(value);
				if (value2.Count == 0)
				{
					_enemiesFearCallbackListener.Remove(enemyType);
				}
			}
			_enemyFearCallbackListener.Remove(enemyInstance);
		}

		public List<IEnemyFearCallbackListener> GetAllEnemyFearCallbackListeners()
		{
			List<IEnemyFearCallbackListener> list = new List<IEnemyFearCallbackListener>();
			foreach (List<IEnemyFearCallbackListener> value in _enemiesFearCallbackListener.Values)
			{
				list.AddRange(value);
			}
			return list;
		}

		public List<IEnemyFearCallbackListener> GetEnemyFearCallbackListeners(EnemyType enemyType)
		{
			if (_enemiesFearCallbackListener.TryGetValue(enemyType, out var value))
			{
				return new List<IEnemyFearCallbackListener>(value);
			}
			return new List<IEnemyFearCallbackListener>();
		}

		public List<IEnemyFearCallbackListener> GetEnemyFearCallbackListeners(List<EnemyType> enemyTypes)
		{
			List<IEnemyFearCallbackListener> list = new List<IEnemyFearCallbackListener>();
			foreach (EnemyType enemyType in enemyTypes)
			{
				if (_enemiesFearCallbackListener.TryGetValue(enemyType, out var value))
				{
					list.AddRange(value);
				}
			}
			return list;
		}

		public IEnemyFearCallbackListener GetEnemyFearCallbackListener(int enemyInstance)
		{
			_enemyFearCallbackListener.TryGetValue(enemyInstance, out var value);
			return value;
		}
	}
}

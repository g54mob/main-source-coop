using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;

namespace Features.EnemyFearingModule.Scripts
{
	public class EnemyFearService : IEnemyFearService, IDisposable
	{
		private readonly EnemyFearListenerModel _enemiesFearListenerModel;

		private CancellationTokenSource _cancellationToken;

		public EnemyFearService(EnemyFearListenerModel enemiesFearListenerModel)
		{
			_enemiesFearListenerModel = enemiesFearListenerModel;
		}

		public void Dispose()
		{
			CancelFear();
		}

		public async UniTask FearEnemy(EnemyType enemy)
		{
			List<IEnemyFearCallbackListener> enemyFearCallbackListeners = _enemiesFearListenerModel.GetEnemyFearCallbackListeners(enemy);
			await TriggerFear(enemyFearCallbackListeners);
		}

		public async UniTask FearEnemy(int enemyInstance)
		{
			IEnemyFearCallbackListener enemyFearCallbackListener = _enemiesFearListenerModel.GetEnemyFearCallbackListener(enemyInstance);
			if (enemyFearCallbackListener != null)
			{
				await TriggerFear(new List<IEnemyFearCallbackListener> { enemyFearCallbackListener });
			}
		}

		public async UniTask FearEnemies(List<EnemyType> enemies)
		{
			List<IEnemyFearCallbackListener> enemyFearCallbackListeners = _enemiesFearListenerModel.GetEnemyFearCallbackListeners(enemies);
			await TriggerFear(enemyFearCallbackListeners);
		}

		public async UniTask<List<EnemyTypeWithTransform>> FearAllEnemy(List<EnemyType> except)
		{
			List<IEnemyFearCallbackListener> allEnemyFearCallbackListeners = _enemiesFearListenerModel.GetAllEnemyFearCallbackListeners();
			if (except != null)
			{
				List<EnemyType> list = new List<EnemyType>(except);
				for (int i = 0; i < allEnemyFearCallbackListeners.Count; i++)
				{
					IEnemyFearCallbackListener enemyFearCallbackListener = allEnemyFearCallbackListeners[i];
					enemyFearCallbackListener.IsDespawnAfterFear = true;
					if (list.Contains(enemyFearCallbackListener.EnemyType))
					{
						enemyFearCallbackListener.IsDespawnAfterFear = false;
						list.Remove(enemyFearCallbackListener.EnemyType);
					}
				}
			}
			return await TriggerFear(allEnemyFearCallbackListeners);
		}

		public void CancelFear()
		{
			if (_cancellationToken != null)
			{
				_cancellationToken?.Cancel();
				_cancellationToken?.Dispose();
			}
		}

		private async UniTask<List<EnemyTypeWithTransform>> TriggerFear(List<IEnemyFearCallbackListener> listeners)
		{
			List<IEnemyFearCallbackListener> valid = GetValid(listeners);
			if (valid.Count == 0)
			{
				return new List<EnemyTypeWithTransform>();
			}
			foreach (IEnemyFearCallbackListener item in valid)
			{
				item.FearCompleted = false;
				item.Fear();
			}
			List<EnemyTypeWithTransform> result = await WaitForFearCompleted(valid);
			_cancellationToken = null;
			return result;
		}

		private async UniTask<List<EnemyTypeWithTransform>> WaitForFearCompleted(List<IEnemyFearCallbackListener> listeners)
		{
			CancelFear();
			_cancellationToken = new CancellationTokenSource();
			while (!_cancellationToken.Token.IsCancellationRequested)
			{
				if (listeners.Where((IEnemyFearCallbackListener callbackListener) => callbackListener != null).All((IEnemyFearCallbackListener l) => l.FearCompleted))
				{
					List<EnemyTypeWithTransform> list = new List<EnemyTypeWithTransform>();
					foreach (IEnemyFearCallbackListener listener in listeners)
					{
						if (listener.PositionOnFearEnd.HasValue)
						{
							list.Add(new EnemyTypeWithTransform(listener.EnemyType, listener.PositionOnFearEnd.Value));
						}
					}
					return list;
				}
				await UniTask.Yield(_cancellationToken.Token, cancelImmediately: true);
			}
			return new List<EnemyTypeWithTransform>();
		}

		private List<IEnemyFearCallbackListener> GetValid(List<IEnemyFearCallbackListener> listeners)
		{
			if (listeners == null || listeners.Count == 0)
			{
				return new List<IEnemyFearCallbackListener>();
			}
			return listeners.Where((IEnemyFearCallbackListener callbackListener) => callbackListener != null).ToList();
		}
	}
}

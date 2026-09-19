using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class TipPoolService : ITipPoolService, IInitializable
	{
		private readonly ITipFactory _factory;

		private readonly TipConfiguration _configuration;

		private readonly Dictionary<TipType, Stack<ITipEntity>> _available = new Dictionary<TipType, Stack<ITipEntity>>();

		private readonly Dictionary<TipType, int> _totalCreated = new Dictionary<TipType, int>();

		private readonly Dictionary<ITipEntity, TipType> _entityTipType = new Dictionary<ITipEntity, TipType>();

		private readonly HashSet<ITipEntity> _returning = new HashSet<ITipEntity>();

		public TipPoolService(ITipFactory factory, TipConfiguration configuration)
		{
			_factory = factory;
			_configuration = configuration;
		}

		public void Initialize()
		{
			foreach (KeyValuePair<TipType, TipEntity> prefab in _configuration.Prefabs)
			{
				prefab.Deconstruct(out var key, out var _);
				TipType tipType = key;
				for (int i = 0; i < _configuration.InitialPoolSize; i++)
				{
					ITipEntity tipEntity = CreateNew(tipType);
					tipEntity.Disable();
					GetStack(tipType).Push(tipEntity);
				}
			}
		}

		public ITipEntity Pop(TipType tipType, bool animated = true)
		{
			Stack<ITipEntity> stack = GetStack(tipType);
			ITipEntity tipEntity = null;
			while (stack.Count > 0)
			{
				ITipEntity tipEntity2 = stack.Pop();
				if (IsAlive(tipEntity2))
				{
					tipEntity = tipEntity2;
					break;
				}
				Forget(tipEntity2, tipType);
			}
			if (tipEntity == null)
			{
				if (GetTotalCreated(tipType) >= _configuration.MaxCapacity)
				{
					Debug.LogWarning(string.Format("[{0}] reached max capacity ({1}) for tip type {2}; cannot provide another tip.", "TipPoolService", _configuration.MaxCapacity, tipType));
					return null;
				}
				tipEntity = CreateNew(tipType);
			}
			tipEntity.StopDissolveAnimation();
			tipEntity.Enable();
			if (animated)
			{
				tipEntity.SetDissolve(1f);
				tipEntity.AnimateDissolve(1f, 0f, _configuration.DissolveAnimationDuration, null);
			}
			else
			{
				tipEntity.SetDissolve(0f);
			}
			return tipEntity;
		}

		public void Return(ITipEntity entity, bool animated = true)
		{
			if (!IsAlive(entity) || _returning.Contains(entity))
			{
				return;
			}
			if (animated)
			{
				_returning.Add(entity);
				entity.AnimateDissolve(0f, 1f, _configuration.DissolveAnimationDuration, delegate
				{
					CompleteReturn(entity);
				});
			}
			else
			{
				entity.StopDissolveAnimation();
				CompleteReturn(entity);
			}
		}

		private void CompleteReturn(ITipEntity entity)
		{
			_returning.Remove(entity);
			if (IsAlive(entity))
			{
				entity.Disable();
				if (_entityTipType.TryGetValue(entity, out var value))
				{
					GetStack(value).Push(entity);
				}
			}
		}

		private void Forget(ITipEntity entity, TipType tipType)
		{
			_entityTipType.Remove(entity);
			_totalCreated[tipType] = Mathf.Max(0, GetTotalCreated(tipType) - 1);
		}

		private static bool IsAlive(ITipEntity entity)
		{
			if (entity is Object obj)
			{
				return obj != null;
			}
			return false;
		}

		private ITipEntity CreateNew(TipType tipType)
		{
			ITipEntity tipEntity = _factory.Create(tipType);
			_totalCreated[tipType] = GetTotalCreated(tipType) + 1;
			_entityTipType[tipEntity] = tipType;
			return tipEntity;
		}

		private Stack<ITipEntity> GetStack(TipType tipType)
		{
			if (!_available.TryGetValue(tipType, out var value))
			{
				value = new Stack<ITipEntity>();
				_available[tipType] = value;
			}
			return value;
		}

		private int GetTotalCreated(TipType tipType)
		{
			return _totalCreated.GetValueOrDefault(tipType, 0);
		}
	}
}

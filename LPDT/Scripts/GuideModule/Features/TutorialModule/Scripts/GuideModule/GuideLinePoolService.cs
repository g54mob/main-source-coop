using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class GuideLinePoolService : IGuideLinePoolService, IInitializable
	{
		private readonly IGuideLineFactory _factory;

		private readonly GuideLineConfiguration _configuration;

		private readonly Dictionary<GuideLineBuildType, Stack<IGuideLineEntity>> _available = new Dictionary<GuideLineBuildType, Stack<IGuideLineEntity>>();

		private readonly Dictionary<GuideLineBuildType, int> _totalCreated = new Dictionary<GuideLineBuildType, int>();

		private readonly Dictionary<IGuideLineEntity, GuideLineBuildType> _entityBuildType = new Dictionary<IGuideLineEntity, GuideLineBuildType>();

		private readonly HashSet<IGuideLineEntity> _returning = new HashSet<IGuideLineEntity>();

		public GuideLinePoolService(IGuideLineFactory factory, GuideLineConfiguration configuration)
		{
			_factory = factory;
			_configuration = configuration;
		}

		public void Initialize()
		{
			foreach (KeyValuePair<GuideLineBuildType, PlaneGuideLineEntity> prefab in _configuration.Prefabs)
			{
				prefab.Deconstruct(out var key, out var _);
				GuideLineBuildType buildType = key;
				for (int i = 0; i < _configuration.InitialPoolSize; i++)
				{
					IGuideLineEntity guideLineEntity = CreateNew(buildType);
					guideLineEntity.Disable();
					GetStack(buildType).Push(guideLineEntity);
				}
			}
		}

		public IGuideLineEntity Pop(GuideLineBuildType buildType, bool animated = true)
		{
			Stack<IGuideLineEntity> stack = GetStack(buildType);
			IGuideLineEntity guideLineEntity;
			if (stack.Count > 0)
			{
				guideLineEntity = stack.Pop();
			}
			else
			{
				if (GetTotalCreated(buildType) >= _configuration.MaxCapacity)
				{
					Debug.LogWarning(string.Format("[{0}] reached max capacity ({1}) for build type {2}; cannot provide another guide line.", "GuideLinePoolService", _configuration.MaxCapacity, buildType));
					return null;
				}
				guideLineEntity = CreateNew(buildType);
			}
			guideLineEntity.StopDissolveAnimation();
			guideLineEntity.Enable();
			if (animated)
			{
				guideLineEntity.SetDissolve(1f);
				guideLineEntity.AnimateDissolve(1f, 0f, _configuration.DissolveAnimationDuration, null);
			}
			else
			{
				guideLineEntity.SetDissolve(0f);
			}
			return guideLineEntity;
		}

		public void Return(IGuideLineEntity entity, bool animated = true)
		{
			if (entity == null || _returning.Contains(entity))
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

		private void CompleteReturn(IGuideLineEntity entity)
		{
			_returning.Remove(entity);
			entity.Disable();
			if (_entityBuildType.TryGetValue(entity, out var value))
			{
				GetStack(value).Push(entity);
			}
		}

		private IGuideLineEntity CreateNew(GuideLineBuildType buildType)
		{
			IGuideLineEntity guideLineEntity = _factory.Create(buildType);
			_totalCreated[buildType] = GetTotalCreated(buildType) + 1;
			_entityBuildType[guideLineEntity] = buildType;
			return guideLineEntity;
		}

		private Stack<IGuideLineEntity> GetStack(GuideLineBuildType buildType)
		{
			if (!_available.TryGetValue(buildType, out var value))
			{
				value = new Stack<IGuideLineEntity>();
				_available[buildType] = value;
			}
			return value;
		}

		private int GetTotalCreated(GuideLineBuildType buildType)
		{
			return _totalCreated.GetValueOrDefault(buildType, 0);
		}
	}
}

using System;
using System.Collections.Generic;
using Features.BoosterModule.BoosterModule.Scripts.Entities;
using Fusion;

namespace Features.BoosterModule.BoosterModule.Scripts
{
	public class BoosterModel
	{
		private readonly Dictionary<PlayerRef, Dictionary<string, IBoosterEntity>> _activeBoosters = new Dictionary<PlayerRef, Dictionary<string, IBoosterEntity>>();

		public IReadOnlyDictionary<PlayerRef, Dictionary<string, IBoosterEntity>> ActiveBoosters => _activeBoosters;

		public event Action<string, PlayerRef> OnActiveBoosterAdded;

		public event Action<string, PlayerRef> OnActiveBoosterRemoved;

		public ActiveBoosterAddingResult TryAddActiveBooster<TBoosterSettings>(BoosterEntityBase<TBoosterSettings> boosterEntityBase, PlayerRef playerRef) where TBoosterSettings : BoosterSettingsBase
		{
			if (_activeBoosters.TryGetValue(playerRef, out var value))
			{
				if (!value.TryAdd(boosterEntityBase.BoosterSettings.GetIdentifier(), boosterEntityBase))
				{
					return new ActiveBoosterAddingResult(isSuccess: false, ActiveBoosterAddingResultType.AlreadyExists);
				}
			}
			else
			{
				_activeBoosters.Add(playerRef, new Dictionary<string, IBoosterEntity> { 
				{
					boosterEntityBase.BoosterSettings.GetIdentifier(),
					boosterEntityBase
				} });
			}
			this.OnActiveBoosterAdded?.Invoke(boosterEntityBase.BoosterSettings.GetIdentifier(), playerRef);
			return new ActiveBoosterAddingResult(isSuccess: true, ActiveBoosterAddingResultType.Added);
		}

		public void RemoveActiveBooster(string boosterType, PlayerRef playerRef)
		{
			_activeBoosters[playerRef].Remove(boosterType);
			this.OnActiveBoosterRemoved?.Invoke(boosterType, playerRef);
		}

		public void ClearActiveBoosters()
		{
			_activeBoosters.Clear();
		}
	}
}

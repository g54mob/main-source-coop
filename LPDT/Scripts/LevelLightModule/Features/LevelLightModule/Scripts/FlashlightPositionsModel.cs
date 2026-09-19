using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.LevelLightModule.Scripts
{
	public class FlashlightPositionsModel : ISessionCleanup
	{
		private readonly Dictionary<int, Dictionary<FlashlightPositionType, Transform>> _flashlightHolders = new Dictionary<int, Dictionary<FlashlightPositionType, Transform>>();

		public IReadOnlyDictionary<int, Dictionary<FlashlightPositionType, Transform>> FlashlightHolders => _flashlightHolders;

		public event Action<int, FlashlightPositionType, Transform> OnFlashlightPositionAdded;

		public void AddFlashlightHolder(int playerId, FlashlightPositionType positionType, Transform flashlightHolder)
		{
			if (!_flashlightHolders.TryAdd(playerId, new Dictionary<FlashlightPositionType, Transform> { { positionType, flashlightHolder } }))
			{
				_flashlightHolders[playerId][positionType] = flashlightHolder;
			}
			this.OnFlashlightPositionAdded?.Invoke(playerId, positionType, flashlightHolder);
		}

		public void RemoveFlashlightHolder(int playerId, FlashlightPositionType positionType, Transform flashlightHolder)
		{
			if (_flashlightHolders.TryGetValue(playerId, out var value) && value.TryGetValue(positionType, out var value2) && value2 == flashlightHolder)
			{
				value.Remove(positionType);
			}
		}

		public void Cleanup()
		{
			_flashlightHolders.Clear();
		}
	}
}

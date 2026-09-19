using System;
using System.Collections.Generic;
using Features.BoosterModule.BoosterModule.Scripts.Entities;

namespace Features.BoosterModule.BoosterModule.Scripts
{
	public class BoosterSystem : IDisposable
	{
		private readonly BoosterModel _boosterModel;

		public BoosterSystem(BoosterModel boosterModel)
		{
			_boosterModel = boosterModel;
		}

		public void Dispose()
		{
			foreach (Dictionary<string, IBoosterEntity> value in _boosterModel.ActiveBoosters.Values)
			{
				foreach (IBoosterEntity value2 in value.Values)
				{
					value2.Deactivate();
				}
			}
			_boosterModel.ClearActiveBoosters();
		}
	}
}

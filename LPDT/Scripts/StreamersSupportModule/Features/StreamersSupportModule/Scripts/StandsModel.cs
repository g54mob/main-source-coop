using System;
using System.Collections.Generic;

namespace Features.StreamersSupportModule.Scripts
{
	public class StandsModel
	{
		private readonly List<StreamerStand> _stands = new List<StreamerStand>();

		public IReadOnlyList<StreamerStand> Stands => _stands;

		public event Action<StreamerStand> OnStandRegistered;

		public void RegisterStands(List<StreamerStand> stands)
		{
			foreach (StreamerStand stand in stands)
			{
				RegisterStand(stand);
			}
		}

		public void RegisterStand(StreamerStand stand)
		{
			_stands.Add(stand);
			this.OnStandRegistered?.Invoke(stand);
		}

		public void UnregisterStands(List<StreamerStand> stands)
		{
			foreach (StreamerStand stand in stands)
			{
				UnregisterStand(stand);
			}
		}

		public void UnregisterStand(StreamerStand stand)
		{
			_stands.Remove(stand);
		}
	}
}

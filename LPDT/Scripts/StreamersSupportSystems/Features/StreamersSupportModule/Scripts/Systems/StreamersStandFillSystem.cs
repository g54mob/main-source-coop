using System;
using Zenject;

namespace Features.StreamersSupportModule.Scripts.Systems
{
	public class StreamersStandFillSystem : IInitializable, IDisposable
	{
		private readonly StandsModel _model;

		private readonly StreamersStandsConfiguration _configuration;

		public StreamersStandFillSystem(StandsModel model, StreamersStandsConfiguration configuration)
		{
			_model = model;
			_configuration = configuration;
		}

		public void Initialize()
		{
			_model.OnStandRegistered += FillRegisteredStand;
			foreach (StreamerStand stand in _model.Stands)
			{
				FillRegisteredStand(stand);
			}
		}

		public void Dispose()
		{
			_model.OnStandRegistered -= FillRegisteredStand;
		}

		private void FillRegisteredStand(StreamerStand stand)
		{
			if (stand.StandIndex >= 0 && stand.StandIndex < _configuration.StreamersStandData.Count)
			{
				stand.ApplyStandData(_configuration.StreamersStandData[stand.StandIndex]);
			}
		}
	}
}

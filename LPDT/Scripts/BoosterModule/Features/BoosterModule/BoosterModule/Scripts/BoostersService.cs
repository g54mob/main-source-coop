using System;
using System.Linq;
using Features.BoosterModule.BoosterModule.Scripts.Entities;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;

namespace Features.BoosterModule.BoosterModule.Scripts
{
	public class BoostersService : IBoostersService
	{
		private readonly IBoosterEntitiesFactory _boosterEntitiesFactory;

		private readonly BoosterModel _boosterModel;

		private readonly MultiplayerModel _multiplayerModel;

		public BoostersService(IBoosterEntitiesFactory boosterEntitiesFactory, BoosterModel boosterModel, MultiplayerModel multiplayerModel)
		{
			_boosterEntitiesFactory = boosterEntitiesFactory;
			_boosterModel = boosterModel;
			_multiplayerModel = multiplayerModel;
		}

		public BoosterEntityBase<TBoosterSettings> ActivateBooster<TBoosterSettings>(TBoosterSettings boosterSettings, PlayerRef playerRef) where TBoosterSettings : BoosterSettingsBase
		{
			BoosterEntityBase<TBoosterSettings> boosterEntityBase = _boosterEntitiesFactory.CreateBoosterEntity(boosterSettings);
			ReActivateBooster(boosterEntityBase, boosterSettings, playerRef);
			return boosterEntityBase;
		}

		public void ReActivateBooster<TBoosterSettings>(BoosterEntityBase<TBoosterSettings> boosterEntityBase, TBoosterSettings boosterSettings, PlayerRef playerRef) where TBoosterSettings : BoosterSettingsBase
		{
			if (boosterEntityBase == null)
			{
				return;
			}
			ActiveBoosterAddingResult activeBoosterAddingResult = _boosterModel.TryAddActiveBooster(boosterEntityBase, playerRef);
			if (activeBoosterAddingResult.IsSuccess)
			{
				boosterEntityBase.UpdateBoosterSettings(boosterSettings);
				if (playerRef == _multiplayerModel.NetworkRunner.LocalPlayer)
				{
					boosterEntityBase.Activate();
				}
				return;
			}
			switch (activeBoosterAddingResult.ResultType)
			{
			case ActiveBoosterAddingResultType.None:
			case ActiveBoosterAddingResultType.Added:
				break;
			case ActiveBoosterAddingResultType.AlreadyExists:
				boosterEntityBase.UpdateBoosterSettings(boosterSettings);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void DeactivateBooster<TBoosterSettings>(BoosterEntityBase<TBoosterSettings> boosterEntityBase, PlayerRef playerRef) where TBoosterSettings : BoosterSettingsBase
		{
			string identifier = boosterEntityBase.GetIdentifier();
			IBoosterEntity boosterEntity = _boosterModel.ActiveBoosters[playerRef].Values.First((IBoosterEntity b) => b.IsIdentifierEqual(identifier));
			_boosterModel.RemoveActiveBooster(identifier, playerRef);
			boosterEntity.Deactivate();
		}

		public void DeactivateBooster(string identifier, PlayerRef playerRef)
		{
			IBoosterEntity boosterEntity = _boosterModel.ActiveBoosters[playerRef].Values.FirstOrDefault((IBoosterEntity b) => b.IsIdentifierEqual(identifier));
			if (boosterEntity != null)
			{
				_boosterModel.RemoveActiveBooster(identifier, playerRef);
				boosterEntity.Deactivate();
			}
		}
	}
}

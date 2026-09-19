using System;
using Features.BoosterModule.BoosterModule.Scripts.Entities;
using RSG.Muffin.ApplicationFilesModule.Core.Scripts;
using Zenject;

namespace Features.BoosterModule.BoosterModule.Scripts
{
	public class BoosterEntitiesFactory : IBoosterEntitiesFactory
	{
		private readonly DiContainer _container;

		private readonly IApplicationFilesHolder _applicationFilesHolder;

		public BoosterEntitiesFactory(DiContainer container, IApplicationFilesHolder applicationFilesHolder)
		{
			_container = container;
			_applicationFilesHolder = applicationFilesHolder;
		}

		public BoosterEntityBase<TBoosterSettings> CreateBoosterEntity<TBoosterSettings>(TBoosterSettings boosterSettings) where TBoosterSettings : BoosterSettingsBase
		{
			Type type = FindConcreteBoosterEntity<TBoosterSettings>();
			if (type == null)
			{
				return null;
			}
			BoosterEntityBase<TBoosterSettings> boosterEntityBase = _container.Instantiate(type) as BoosterEntityBase<TBoosterSettings>;
			boosterEntityBase.BoosterSettings = boosterSettings;
			_container.Inject(boosterEntityBase);
			return boosterEntityBase;
		}

		private Type FindConcreteBoosterEntity<TBoosterSettings>() where TBoosterSettings : BoosterSettingsBase
		{
			Type baseType = typeof(BoosterEntityBase<TBoosterSettings>);
			return _applicationFilesHolder.GetFirstOrDefaultType((Type type) => !type.IsAbstract && baseType.IsAssignableFrom(type));
		}
	}
}

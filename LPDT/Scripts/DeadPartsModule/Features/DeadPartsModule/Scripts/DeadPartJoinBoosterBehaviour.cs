using System;
using Features.BoosterModule.BoosterModule.Scripts;
using Features.BoosterModule.BoosterModule.Scripts.Entities;
using Fusion;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class DeadPartJoinBoosterBehaviour : NetworkBehaviour
	{
		private IBoostersService _boostersService;

		private IBoosterEntity _currentBoosterEntity;

		public event Action OnBeforeDisableDeadPartEffect;

		[Inject]
		private void InjectDependencies(IBoostersService boostersService)
		{
			_boostersService = boostersService;
		}

		public void ApplyDeadPartEffect<TBoosterSettings>(TBoosterSettings boosterSettings) where TBoosterSettings : BoosterSettingsBase
		{
			if (boosterSettings != null)
			{
				if (_currentBoosterEntity != null)
				{
					DisableDeadPartEffect();
				}
				_currentBoosterEntity = _boostersService.ActivateBooster(boosterSettings, base.Object.InputAuthority);
			}
		}

		public void DisableDeadPartEffect()
		{
			if (_currentBoosterEntity != null)
			{
				this.OnBeforeDisableDeadPartEffect?.Invoke();
				string identifier = _currentBoosterEntity.GetIdentifier();
				_currentBoosterEntity = null;
				_boostersService.DeactivateBooster(identifier, base.Object.InputAuthority);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}

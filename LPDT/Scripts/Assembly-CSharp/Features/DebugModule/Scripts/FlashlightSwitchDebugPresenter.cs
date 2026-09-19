using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.LevelLightModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public class FlashlightSwitchDebugPresenter : PresenterBehaviour<FlashlightSwitchDebugViewBase>
	{
		private readonly IFlashlightSpawnService _flashlightSpawnService;

		private readonly MultiplayerModel _multiplayerModel;

		public FlashlightSwitchDebugPresenter(IFlashlightSpawnService flashlightSpawnService, MultiplayerModel multiplayerModel)
		{
			_flashlightSpawnService = flashlightSpawnService;
			_multiplayerModel = multiplayerModel;
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			base.View.SetFlashlightTypes((from FlashlightType flashlightType in Enum.GetValues(typeof(FlashlightType))
				where flashlightType != FlashlightType.None
				select flashlightType).ToArray());
			base.View.OnFlashlightTypeApplyRequested += SpawnFlashlight;
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			base.View.OnFlashlightTypeApplyRequested -= SpawnFlashlight;
		}

		private void SpawnFlashlight(FlashlightType flashlightType)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && _multiplayerModel.NetworkRunner.IsRunning)
			{
				_flashlightSpawnService.SpawnFlashlight(flashlightType, _multiplayerModel.NetworkRunner.LocalPlayer).Forget();
			}
		}
	}
}

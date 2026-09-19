using Features.DeviceModule.Scripts;
using Features.DeviceModule.Scripts.DeviceData;
using Features.ExtendedLogger.Scripts;
using RSG.Muffin.PlatformStatusSubmodule.Scripts.API;

namespace Features.PlatformStatusRealizationModule.Scripts
{
	public class GameStatusService : IGameStatusService
	{
		private readonly GameStatusesConfiguration _gameStatusesConfiguration;

		private readonly IPlatformStatusService _platformStatusService;

		private readonly IDeviceService _deviceService;

		public GameStatusService(GameStatusesConfiguration gameStatusesConfiguration, IPlatformStatusService platformStatusService, IDeviceService deviceService)
		{
			_gameStatusesConfiguration = gameStatusesConfiguration;
			_platformStatusService = platformStatusService;
			_deviceService = deviceService;
		}

		public void SetGameStatus(GameStatusId gameStatusId)
		{
			DeviceType currentDevice = _deviceService.GetCurrentDevice();
			if (!_gameStatusesConfiguration.GameStatusesData.TryGetValue(gameStatusId, out var value))
			{
				ExtendedDebug.LogFiltered(DebugFilterType.PlatformStatus, $"SetGameStatus({gameStatusId}): no configuration entry, skipped");
				return;
			}
			if (!value.PlatformsData.TryGetValue(currentDevice, out var value2))
			{
				ExtendedDebug.LogFiltered(DebugFilterType.PlatformStatus, $"SetGameStatus({gameStatusId}): no entry for device {currentDevice}, skipped");
				return;
			}
			ExtendedDebug.LogFiltered(DebugFilterType.PlatformStatus, $"SetGameStatus({gameStatusId}): {value2.Property} = '{value2.Value}' (device: {currentDevice})");
			_platformStatusService.UpdateGenericStatus(value2.Property, value2.Value);
		}

		public void SetGameStatusParameter(GameStatusParameterId gameStatusParameterId, string parameterValue)
		{
			DeviceType currentDevice = _deviceService.GetCurrentDevice();
			if (!_gameStatusesConfiguration.GameStatusParametersData.TryGetValue(gameStatusParameterId, out var value))
			{
				ExtendedDebug.LogFiltered(DebugFilterType.PlatformStatus, $"SetGameStatusParameter({gameStatusParameterId}): no configuration entry, skipped");
				return;
			}
			if (!value.PlatformsData.TryGetValue(currentDevice, out var value2))
			{
				ExtendedDebug.LogFiltered(DebugFilterType.PlatformStatus, $"SetGameStatusParameter({gameStatusParameterId}): no entry for device {currentDevice}, skipped");
				return;
			}
			ExtendedDebug.LogFiltered(DebugFilterType.PlatformStatus, $"SetGameStatusParameter({gameStatusParameterId}): {value2.Property} = '{parameterValue}' (device: {currentDevice})");
			_platformStatusService.UpdateGenericStatus(value2.Property, parameterValue);
		}
	}
}

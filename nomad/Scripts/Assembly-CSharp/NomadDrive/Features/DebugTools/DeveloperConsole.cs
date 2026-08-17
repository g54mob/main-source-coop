using System;
using System.Text;
using Enviro;
using EvilCore.Audio;
using EvilCore.GraphicsQuality;
using EvilCore.Managers;
using EvilCore.Networking;
using EvilCore.Particles;
using EvilCore.Settings;
using EvilCore.UI.Scripts;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.GameTime;
using NomadDrive.Features.Player;
using NomadDrive.Features.WorldGeneration;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using NomadDrive.Features.WorldGeneration.Thumbleweed;
using NomadDrive.Managers.GameTime;
using QFSW.QC;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.DebugTools
{
	public class DeveloperConsole : MonoBehaviour
	{
		private QuantumConsole _quantumConsole;

		private CrosshairPanel _crosshairPanel;

		private PlayerStatsManager _playerStatsManager;

		private IGameUIManager _guiManager;

		private IPlayerService _playerService;

		private IParticlesManager _particlesManager;

		private ThumbleweedManager _thumbleweedManager;

		private ITimeManager _timeManager;

		private LootRegistry _lootRegistry;

		private IGraphicsQualityManager _graphicsQualityManager;

		private IAudioManager _audioManager;

		private IVoiceChatManager _voiceChatManager;

		private ISettingsManager _settingsManager;

		public bool IsInitialized { get; set; }

		[Inject]
		private void Construct(CrosshairPanel crosshairPanel, IPlayerService playerReferenceService, IGameUIManager guiManager, IParticlesManager particlesManager, ThumbleweedManager thumbleweedManager, ITimeManager timeManager, LootRegistry lootRegistry, IGraphicsQualityManager graphicsQualityManager, IAudioManager audioManager, IVoiceChatManager voiceChatManager, ISettingsManager settingsManager)
		{
			_crosshairPanel = crosshairPanel;
			_playerService = playerReferenceService;
			_guiManager = guiManager;
			_particlesManager = particlesManager;
			_thumbleweedManager = thumbleweedManager;
			_timeManager = timeManager;
			_lootRegistry = lootRegistry;
			_graphicsQualityManager = graphicsQualityManager;
			_audioManager = audioManager;
			_voiceChatManager = voiceChatManager;
			_settingsManager = settingsManager;
			_playerService.OnPlayerRegistered += Init;
		}

		private void OnDestroy()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= Init;
			}
			if (_quantumConsole != null)
			{
				_quantumConsole.OnActivate -= Activate;
				_quantumConsole.OnDeactivate -= Deactivate;
			}
		}

		private void Init()
		{
			if (!IsInitialized && !(this == null))
			{
				if (_playerService.TryGetStatsManager(out var manager))
				{
					_playerStatsManager = manager;
				}
				_quantumConsole = GetComponent<QuantumConsole>();
				if (_quantumConsole == null)
				{
					IsInitialized = true;
					return;
				}
				_quantumConsole.OnActivate += Activate;
				_quantumConsole.OnDeactivate += Deactivate;
				IsInitialized = true;
			}
		}

		private void Activate()
		{
			_crosshairPanel.Deactivate();
			GameCursor.Unlock();
			_guiManager.ShowCanvasGroup(GameCanvasGroupName.DeveloperConsole, interactable: true, blockRaycast: true);
			_quantumConsole.FocusConsoleInput();
		}

		private void Deactivate()
		{
			_crosshairPanel?.Activate();
			GameCursor.Lock();
			_guiManager.HideCanvasGroup(GameCanvasGroupName.DeveloperConsole);
		}

		[Command("player.setNutrition", "Set nutrition value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetNutrition(float value)
		{
			_playerStatsManager.Nutrition.CurrentValue = value;
		}

		[Command("player.setHydration", "Set hydration value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetHydration(float value)
		{
			_playerStatsManager.Hydration.CurrentValue = value;
		}

		[Command("player.setEnergy", "Set energy value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetEnergy(float value)
		{
			_playerStatsManager.Energy.CurrentValue = value;
		}

		[Command("player.setDamage", "Set health stat value (controls damage zone)", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetDamage(float value)
		{
			_playerStatsManager.Health.CurrentValue = value;
		}

		[Command("player.applyDamage", "Apply direct damage to player", Platform.AllPlatforms, MonoTargetType.Single)]
		public void ApplyDamage(float value)
		{
			_playerStatsManager.ApplyDirectDamage(value);
		}

		[Command("player.healDamage", "Heal direct damage", Platform.AllPlatforms, MonoTargetType.Single)]
		public void HealDamage(float value)
		{
			_playerStatsManager.HealDamage(value);
		}

		[Command("player.effectiveHealth", "Show current effective health", Platform.AllPlatforms, MonoTargetType.Single)]
		public void ShowEffectiveHealth()
		{
		}

		[Command("player.setWalkSpeed", "Set walk speed value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetSpeed(float value)
		{
			if (_playerService.TryGetFirstPersonController(out var controller) && controller.FirstPersonControllerSettings != null)
			{
				controller.FirstPersonControllerSettings.walkSpeed = value;
			}
		}

		[Command("player.setSprintSpeed", "Set sprint speed value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetSprintSpeed(float value)
		{
			if (_playerService.TryGetFirstPersonController(out var controller) && controller.FirstPersonControllerSettings != null)
			{
				controller.FirstPersonControllerSettings.sprintSpeed = value;
			}
		}

		[Command("player.setCrouchSpeed", "Set crouched walk speed value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetCrouchSpeed(float value)
		{
			if (_playerService.TryGetFirstPersonController(out var controller) && controller.FirstPersonControllerSettings != null)
			{
				controller.FirstPersonControllerSettings.crouchedWalkSpeed = value;
			}
		}

		[Command("player.setCrouchSprintSpeed", "Set crouched sprint speed value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetCrouchSprintSpeed(float value)
		{
			if (_playerService.TryGetFirstPersonController(out var controller) && controller.FirstPersonControllerSettings != null)
			{
				controller.FirstPersonControllerSettings.crouchedSprintSpeed = value;
			}
		}

		[Command("player.setStrong", "Toggle mass debuff: true = ignore heavy item slowdown", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetPlayerStrong(bool value)
		{
			if (_playerService.TryGetFirstPersonController(out var controller))
			{
				controller.SetMassDebuffEnabled(!value);
			}
		}

		[Command("audio.setMaster", "Master volume 0-100", Platform.AllPlatforms, MonoTargetType.Single)]
		public void AudioSetMaster(int value)
		{
			if (_audioManager == null)
			{
				WarnAudioMissing();
			}
			else
			{
				_audioManager.SetMasterVolume((float)Mathf.Clamp(value, 0, 100) / 100f);
			}
		}

		[Command("audio.setMusic", "Music bus volume 0-100", Platform.AllPlatforms, MonoTargetType.Single)]
		public void AudioSetMusic(int value)
		{
			if (_audioManager == null)
			{
				WarnAudioMissing();
			}
			else
			{
				_audioManager.SetMusicVolume((float)Mathf.Clamp(value, 0, 100) / 100f);
			}
		}

		[Command("audio.setSfx", "Sfx bus volume 0-100", Platform.AllPlatforms, MonoTargetType.Single)]
		public void AudioSetSfx(int value)
		{
			if (_audioManager == null)
			{
				WarnAudioMissing();
			}
			else
			{
				_audioManager.SetSfxVolume((float)Mathf.Clamp(value, 0, 100) / 100f);
			}
		}

		[Command("audio.setAmbience", "Ambience bus volume 0-100", Platform.AllPlatforms, MonoTargetType.Single)]
		public void AudioSetAmbience(int value)
		{
			if (_audioManager == null)
			{
				WarnAudioMissing();
			}
			else
			{
				_audioManager.SetAmbienceVolume((float)Mathf.Clamp(value, 0, 100) / 100f);
			}
		}

		[Command("audio.setVoiceMute", "Mute/unmute local mic", Platform.AllPlatforms, MonoTargetType.Single)]
		public void AudioSetVoiceMute(bool muted)
		{
			if (_voiceChatManager != null)
			{
				_voiceChatManager.SetMuted(muted);
			}
		}

		[Command("audio.getLevels", "Dump current voice state", Platform.AllPlatforms, MonoTargetType.Single)]
		public string AudioGetLevels()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Voice state:");
			if (_voiceChatManager != null)
			{
				stringBuilder.AppendLine($"  voice.connected = {_voiceChatManager.IsConnected}");
				stringBuilder.AppendLine($"  voice.mute = {_voiceChatManager.IsMuted}");
			}
			else
			{
				stringBuilder.AppendLine("  voice = unavailable");
			}
			return stringBuilder.ToString();
		}

		private void WarnAudioMissing()
		{
		}

		[Command("time.setTimeOfDay", "value of day time 12.5 -> 12:30 PM", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetDayTime(float value)
		{
			EnviroManager.instance.Time.SetTimeOfDay(value);
		}

		[Command("time.forwardSmoothly", "Advance time by <hours> at <speed>x visual flow", Platform.AllPlatforms, MonoTargetType.Single)]
		public void ForwardTimeSmoothlyCommand(float hours, float speed)
		{
			if (_timeManager != null)
			{
				int num = Mathf.FloorToInt(hours);
				int minutes = Mathf.FloorToInt((hours - (float)num) * 60f);
				DayTimePart dayTimePart = new DayTimePart
				{
					Hours = num,
					Minutes = minutes,
					Seconds = 0
				};
				_timeManager.ForwardTimeSmoothly(dayTimePart, speed);
			}
		}

		[Command("weather.set", "Smoothly transition to weather preset by name", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherCommand(string weatherName)
		{
			ChangeWeather(weatherName, instant: false);
		}

		[Command("weather.setInstant", "Instantly switch to weather preset by name", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherInstantCommand(string weatherName)
		{
			ChangeWeather(weatherName, instant: true);
		}

		[Command("weather.clearSky", "Transition to Clear Sky", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherClearSky()
		{
			ChangeWeather("Clear Sky", instant: false);
		}

		[Command("weather.cloudy1", "Transition to Cloudy 1", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherCloudy1()
		{
			ChangeWeather("Cloudy 1", instant: false);
		}

		[Command("weather.cloudy2", "Transition to Cloudy 2", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherCloudy2()
		{
			ChangeWeather("Cloudy 2", instant: false);
		}

		[Command("weather.cloudy3", "Transition to Cloudy 3", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherCloudy3()
		{
			ChangeWeather("Cloudy 3", instant: false);
		}

		[Command("weather.cloudy4", "Transition to Cloudy 4", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherCloudy4()
		{
			ChangeWeather("Cloudy 4", instant: false);
		}

		[Command("weather.foggy", "Transition to Foggy", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherFoggy()
		{
			ChangeWeather("Foggy", instant: false);
		}

		[Command("weather.rain", "Transition to Rain", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherRain()
		{
			ChangeWeather("Rain", instant: false);
		}

		[Command("weather.storm", "Transition to Storm", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherStorm()
		{
			ChangeWeather("Storm", instant: false);
		}

		[Command("weather.snow", "Transition to Snow", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetWeatherSnow()
		{
			ChangeWeather("Snow", instant: false);
		}

		[Command("weather.next", "Host: force the dynamic weather director to pick the next segment now", Platform.AllPlatforms, MonoTargetType.Single)]
		public void WeatherNextCommand()
		{
			DynamicWeatherDirector dynamicWeatherDirector = UnityEngine.Object.FindFirstObjectByType<DynamicWeatherDirector>();
			if (!(dynamicWeatherDirector == null))
			{
				dynamicWeatherDirector.DebugForceAdvanceSegment();
			}
		}

		[Command("weather.segment", "Log the dynamic weather director's current scheduler state", Platform.AllPlatforms, MonoTargetType.Single)]
		public void WeatherSegmentCommand()
		{
			_ = UnityEngine.Object.FindFirstObjectByType<DynamicWeatherDirector>() == null;
		}

		private void ChangeWeather(string weatherName, bool instant)
		{
			if (!(EnviroManager.instance == null) && !(EnviroManager.instance.Weather == null) && !string.IsNullOrWhiteSpace(weatherName))
			{
				if (instant)
				{
					EnviroManager.instance.Weather.ChangeWeatherInstant(weatherName);
				}
				else
				{
					EnviroManager.instance.Weather.ChangeWeather(weatherName);
				}
			}
		}

		[Command("world.spawnThumbleweed", "Spawn one tumbleweed at camera look-at point, rolling in a random direction", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SpawnThumbleweedCommand()
		{
			if (!(_thumbleweedManager == null) && TryGetActiveCamera(out var cameraTransform) && Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo, 200f, -1, QueryTriggerInteraction.Ignore))
			{
				float y = UnityEngine.Random.Range(0f, 360f);
				Vector3 horizontalWindDirection = Quaternion.Euler(0f, y, 0f) * Vector3.forward;
				_thumbleweedManager.SpawnAtPosition(hitInfo.point, horizontalWindDirection);
			}
		}

		[Command("world.createSandstorm", "Spawn SandStorm particle 50m in front of camera at <scale>", Platform.AllPlatforms, MonoTargetType.Single)]
		public void CreateSandstormCommand(float scale)
		{
			if (_particlesManager != null && TryGetActiveCamera(out var cameraTransform))
			{
				Vector3 position = cameraTransform.position + cameraTransform.forward * 50f;
				_particlesManager.PlayOneShot("SandStorm", position, Quaternion.identity, new ParticleOverrides
				{
					ScaleMultiplier = scale
				});
			}
		}

		[Command("world.spawnLoot", "Spawn networked loot by display name or address (full condition, full liquid)", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SpawnLootCommand(string key)
		{
			if (_lootRegistry == null || string.IsNullOrWhiteSpace(key) || NetworkSingleton<WorldGenerator>.Instance == null || !TryGetActiveCamera(out var cameraTransform))
			{
				return;
			}
			LootRegistryEntry lootRegistryEntry = _lootRegistry.GetByAddress(key);
			if (lootRegistryEntry == null)
			{
				foreach (LootRegistryEntry entry in _lootRegistry.Entries)
				{
					if (string.Equals(entry.displayName, key, StringComparison.OrdinalIgnoreCase))
					{
						lootRegistryEntry = entry;
						break;
					}
				}
			}
			if (lootRegistryEntry != null && !string.IsNullOrEmpty(lootRegistryEntry.assetGuid))
			{
				RaycastHit hitInfo;
				Vector3 position = ((!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, 50f, -1, QueryTriggerInteraction.Ignore)) ? (cameraTransform.position + cameraTransform.forward * 3f) : (hitInfo.point + Vector3.up * 0.25f));
				NetworkSingleton<WorldGenerator>.Instance.CmdDebugSpawnLootByGuid(lootRegistryEntry.assetGuid, position);
			}
		}

		[Command("world.listLoot", "List all loot keys available to world.spawnLoot", Platform.AllPlatforms, MonoTargetType.Single)]
		public string ListLootCommand()
		{
			if (_lootRegistry == null || _lootRegistry.Count == 0)
			{
				return "No loot registered.";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Registered loot ({_lootRegistry.Count}):");
			foreach (LootRegistryEntry entry in _lootRegistry.Entries)
			{
				stringBuilder.AppendLine($"  [{entry.category}] {entry.displayName}  ->  {entry.address}");
			}
			return stringBuilder.ToString();
		}

		[Command("world.originInfo", "Show floating-origin state (TotalShift, focus render/true pos, tile coord)", Platform.AllPlatforms, MonoTargetType.Single)]
		public string FloatingOriginInfoCommand()
		{
			FloatingOriginManager instance = FloatingOriginManager.Instance;
			if (!(instance != null))
			{
				return "FloatingOriginManager unavailable (FloatingOriginInstaller not wired into GameLifetimeScope).";
			}
			return instance.DebugInfo();
		}

		[Command("world.teleportFar", "Teleport the local player +<km> in +Z to exercise floating-origin rebases (on-foot test)", Platform.AllPlatforms, MonoTargetType.Single)]
		public void TeleportFarCommand(float km)
		{
			if (_playerService != null && _playerService.IsPlayerSpawned && !(_playerService.LocalPlayer == null))
			{
				NomadDrive.Features.Player.Player localPlayer = _playerService.LocalPlayer;
				Vector3 pos = localPlayer.transform.position + new Vector3(0f, 0f, km * 1000f);
				localPlayer.SetPositionAndRotation(pos, localPlayer.transform.eulerAngles);
			}
		}

		[Command("world.forceShift", "Force one floating-origin rebase now (ignores the distance threshold)", Platform.AllPlatforms, MonoTargetType.Single)]
		public void ForceShiftCommand()
		{
			FloatingOriginManager instance = FloatingOriginManager.Instance;
			if (!(instance == null))
			{
				instance.ForceShift();
			}
		}

		private bool TryGetActiveCamera(out Transform cameraTransform)
		{
			if (_playerService != null && _playerService.TryGetCameraTransform(out cameraTransform) && cameraTransform != null)
			{
				return true;
			}
			if (Camera.main != null)
			{
				cameraTransform = Camera.main.transform;
				return true;
			}
			cameraTransform = null;
			return false;
		}

		[Command("game.setSensitivity", "Set mouse look sensitivity on the 1-10 user scale (applies to X and Y)", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetMouseSensitivity(float value)
		{
			if (_settingsManager != null)
			{
				value = Mathf.Clamp(value, 1f, 10f);
				_settingsManager.SetSensitivity(value, value);
			}
		}

		[Command("graphics.setQuality", "Set graphics quality level (0=Low, 1=Medium, 2=High)", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetQualityCommand(int level)
		{
			if (_graphicsQualityManager != null)
			{
				level = Mathf.Clamp(level, 0, 2);
				_graphicsQualityManager.SetQuality((GraphicsQualityLevel)level);
			}
		}

		[Command("graphics.getQuality", "Get current graphics quality level", Platform.AllPlatforms, MonoTargetType.Single)]
		public string GetQualityCommand()
		{
			if (_graphicsQualityManager == null)
			{
				return "GraphicsQualityManager unavailable.";
			}
			return $"Current: {_graphicsQualityManager.CurrentLevel} ({(int)_graphicsQualityManager.CurrentLevel})";
		}

		[Command("graphics.forceApply", "Re-apply current graphics quality (debug)", Platform.AllPlatforms, MonoTargetType.Single)]
		public void ForceQualityCommand()
		{
			_graphicsQualityManager?.ForceApply();
		}

		[Command("display.setFullscreen", "Set fullscreen value", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetFullscreen(bool value)
		{
			Screen.fullScreen = value;
		}

		[Command("display.setResolution1080p", "Set resolution to 1080p", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetResolution1080P()
		{
			Screen.SetResolution(1920, 1080, Screen.fullScreen);
		}

		[Command("display.setResolution1440p", "Set resolution to 1440p", Platform.AllPlatforms, MonoTargetType.Single)]
		public void SetResolution1440P()
		{
			Screen.SetResolution(2560, 1440, Screen.fullScreen);
		}

		[Command("prefs.clearAll", "Wipe ALL PlayerPrefs-style data (settings, input remaps, EvilSave.Prefs). Does NOT touch EvilSave save slots.", Platform.AllPlatforms, MonoTargetType.Single)]
		public void ClearAllPrefs()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}
	}
}

using System;
using EvilCore.CustomPass;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using VContainer;

namespace EvilCore.GraphicsQuality
{
	public class GraphicsQualityManager : MonoBehaviour, IGraphicsQualityManager
	{
		[SerializeField]
		private GraphicsQualityConfig config;

		[SerializeField]
		private Volume globalVolume;

		[Inject]
		private ICustomPassManager _customPassManager;

		[Inject]
		private IGameLoadingManager _gameLoadingManager;

		private GraphicsQualityLevel _currentLevel;

		private bool _hasAppliedOnce;

		private GraphicsQualityConfig.LevelSettings _lastAppliedSettings;

		private bool _hasWarnedMissingVolume;

		private bool _hasWarnedMissingOverrides;

		public GraphicsQualityLevel CurrentLevel => _currentLevel;

		public event Action<GraphicsQualityLevel> OnQualityChanged;

		private void Awake()
		{
			_currentLevel = GraphicsQualityLevel.High;
		}

		private void Start()
		{
			ApplyQuality();
			this.OnQualityChanged?.Invoke(_currentLevel);
			if (_gameLoadingManager != null)
			{
				_gameLoadingManager.OnLoadingComplete.AddListener(OnGameReady);
			}
		}

		private void OnDestroy()
		{
			if (_gameLoadingManager != null)
			{
				_gameLoadingManager.OnLoadingComplete.RemoveListener(OnGameReady);
			}
		}

		private void OnGameReady()
		{
			ForceApply();
		}

		public void SetQuality(GraphicsQualityLevel level)
		{
			if (!_hasAppliedOnce || _currentLevel != level)
			{
				_currentLevel = level;
				ApplyQuality();
				this.OnQualityChanged?.Invoke(level);
			}
		}

		public void ForceApply()
		{
			ApplyQuality();
			this.OnQualityChanged?.Invoke(_currentLevel);
		}

		public void SetMotionBlurEnabled(bool enabled)
		{
			MotionBlur component;
			if (globalVolume == null || globalVolume.profile == null)
			{
				if (!_hasWarnedMissingVolume)
				{
					_hasWarnedMissingVolume = true;
				}
			}
			else if (globalVolume.profile.TryGet<MotionBlur>(out component))
			{
				component.active = enabled;
			}
		}

		private void ApplyQuality()
		{
			GraphicsQualityConfig.LevelSettings settings = config.GetSettings(_currentLevel);
			ApplyQualityLevel();
			ApplyUnitySettings(settings);
			ApplyVolumeOverrides(settings);
			ApplyCustomPasses(settings);
			_lastAppliedSettings = settings;
			_hasAppliedOnce = true;
		}

		private void ApplyQualityLevel()
		{
			QualitySettings.SetQualityLevel(_currentLevel switch
			{
				GraphicsQualityLevel.High => 0, 
				GraphicsQualityLevel.Medium => 1, 
				GraphicsQualityLevel.Low => 2, 
				_ => 0, 
			}, applyExpensiveChanges: false);
		}

		private void ApplyUnitySettings(GraphicsQualityConfig.LevelSettings settings)
		{
			if (!_hasAppliedOnce || _lastAppliedSettings.textureMipmapLimit != settings.textureMipmapLimit)
			{
				QualitySettings.globalTextureMipmapLimit = settings.textureMipmapLimit;
			}
			if (!_hasAppliedOnce || _lastAppliedSettings.anisotropicFiltering != settings.anisotropicFiltering)
			{
				QualitySettings.anisotropicFiltering = settings.anisotropicFiltering;
			}
			QualitySettings.lodBias = settings.lodBias;
			QualitySettings.shadowDistance = settings.shadowDistance;
			QualitySettings.streamingMipmapsActive = settings.textureStreamingEnabled;
			if (settings.textureStreamingEnabled)
			{
				QualitySettings.streamingMipmapsMemoryBudget = settings.textureStreamingBudgetMB;
			}
		}

		private void ApplyVolumeOverrides(GraphicsQualityConfig.LevelSettings settings)
		{
			if (globalVolume == null || globalVolume.profile == null)
			{
				if (!_hasWarnedMissingVolume)
				{
					_hasWarnedMissingVolume = true;
				}
				return;
			}
			VolumeProfile profile = globalVolume.profile;
			bool flag = false;
			if (profile.TryGet<ScreenSpaceAmbientOcclusion>(out var component))
			{
				component.active = settings.ssaoEnabled;
				component.quality.levelAndOverride = (level: settings.ssaoQuality, useOverride: false);
			}
			else
			{
				flag = true;
			}
			if (profile.TryGet<GlobalIllumination>(out var component2))
			{
				component2.active = settings.ssgiEnabled;
				component2.maxRaySteps = settings.ssgiRaySteps;
			}
			else
			{
				flag = true;
			}
			if (profile.TryGet<ScreenSpaceReflection>(out var component3))
			{
				component3.active = settings.ssrEnabled;
			}
			else
			{
				flag = true;
			}
			if (profile.TryGet<Bloom>(out var component4))
			{
				component4.active = settings.bloomEnabled;
				component4.quality.levelAndOverride = (level: settings.bloomQuality, useOverride: false);
				component4.highQualityFiltering = settings.bloomHighQualityFiltering;
			}
			else
			{
				flag = true;
			}
			if (profile.TryGet<Fog>(out var component5))
			{
				component5.enableVolumetricFog.value = settings.volumetricFogEnabled;
				component5.volumetricFogBudget = settings.volumetricFogBudget;
			}
			else
			{
				flag = true;
			}
			if (profile.TryGet<ContactShadows>(out var component6))
			{
				component6.active = settings.contactShadowsEnabled;
			}
			else
			{
				flag = true;
			}
			if (flag && !_hasWarnedMissingOverrides)
			{
				_hasWarnedMissingOverrides = true;
			}
		}

		private void ApplyCustomPasses(GraphicsQualityConfig.LevelSettings settings)
		{
			if (_customPassManager != null && _customPassManager.IsInitialized)
			{
				if (settings.customPassesEnabled)
				{
					_customPassManager.EnableAll();
				}
				else
				{
					_customPassManager.DisableAll();
				}
			}
		}
	}
}

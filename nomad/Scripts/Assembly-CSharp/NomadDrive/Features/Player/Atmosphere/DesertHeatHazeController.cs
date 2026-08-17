using EvilCore.CustomPass;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.GraphicsQuality;
using NomadDrive.Features.Player.Core;
using NomadDrive.Managers.GameTime;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.Atmosphere
{
	public class DesertHeatHazeController : MonoBehaviour, IPlayerComponent
	{
		private const float Epsilon = 0.001f;

		private const string StrengthProp = "_Strength";

		private const string DistortionScaleProp = "_DistortionScale";

		private const string NoiseScaleProp = "_NoiseScale";

		private const string ScrollSpeedProp = "_ScrollSpeed";

		private const string DistanceStartProp = "_DistanceStart";

		private const string DistanceFullProp = "_DistanceFull";

		private const string VerticalBiasProp = "_VerticalBias";

		private const string HorizonTopProp = "_HorizonTop";

		[SerializeField]
		private DesertHeatHazeConfig config;

		[Inject]
		private ICustomPassManager _customPassManager;

		[Inject]
		private ITimeManager _timeManager;

		[Inject]
		private IGraphicsQualityManager _graphicsQuality;

		private bool _isInitialized;

		private bool _passVerified;

		private float _currentStrength;

		private float _strengthVelocity;

		private bool _passEnabled;

		public int SetupPriority => 25;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (!isLocalPlayer)
			{
				base.enabled = false;
			}
			else if (config == null)
			{
				EvilLogger.LogError("[DesertHeatHazeController] DesertHeatHazeConfig not assigned. Heat haze disabled.", "SetupForPlayer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Atmosphere\\DesertHeatHazeController.cs", 59);
				base.enabled = false;
			}
			else if (_customPassManager == null || _timeManager == null)
			{
				EvilLogger.LogError("[DesertHeatHazeController] Required services missing (ICustomPassManager / ITimeManager). Heat haze disabled.", "SetupForPlayer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Atmosphere\\DesertHeatHazeController.cs", 66);
				base.enabled = false;
			}
			else
			{
				_isInitialized = true;
			}
		}

		private void Update()
		{
			if (!_isInitialized || !_customPassManager.IsInitialized)
			{
				return;
			}
			if (!_passVerified)
			{
				if (!_customPassManager.HasPass(config.passId))
				{
					base.enabled = false;
					return;
				}
				_passVerified = true;
			}
			ApplyStaticProperties();
			bool num = _graphicsQuality == null || _graphicsQuality.CurrentLevel != GraphicsQualityLevel.Low;
			bool flag = false;
			bool flag2 = false;
			float num2 = ((!num) ? 0f : ((!flag) ? ComputeTargetStrength(flag2) : Mathf.Clamp01(config.debugStrengthOverride)));
			_currentStrength = ((flag || flag2) ? num2 : Mathf.SmoothDamp(_currentStrength, num2, ref _strengthVelocity, Mathf.Max(0.01f, config.strengthSmoothTime)));
			bool flag3 = num && _currentStrength > 0.001f;
			if (flag3 != _passEnabled)
			{
				_customPassManager.SetPassEnabled(config.passId, flag3);
				_passEnabled = flag3;
			}
			if (flag3)
			{
				_customPassManager.GetProperties(config.passId).SetFloat("_Strength", _currentStrength);
			}
		}

		private float ComputeTargetStrength(bool scrubTime)
		{
			float time = _timeManager.CurrentHour;
			float num = ((config.temperatureCurve != null) ? Mathf.Clamp01(config.temperatureCurve.Evaluate(_timeManager.Temperature)) : 1f);
			float num2 = ((config.timeOfDayCurve != null) ? Mathf.Clamp01(config.timeOfDayCurve.Evaluate(time)) : 1f);
			return num * num2 * config.maxStrength;
		}

		private void ApplyStaticProperties()
		{
			_customPassManager.GetProperties(config.passId).SetFloat("_DistortionScale", config.distortionScale).SetFloat("_NoiseScale", config.noiseScale)
				.SetVector("_ScrollSpeed", new Vector4(config.scrollSpeed.x, config.scrollSpeed.y, 0f, 0f))
				.SetFloat("_DistanceStart", config.distanceStart)
				.SetFloat("_DistanceFull", config.distanceFull)
				.SetFloat("_VerticalBias", config.verticalBias)
				.SetFloat("_HorizonTop", config.horizonTop);
		}

		private void OnDestroy()
		{
			if (_customPassManager != null && !(config == null))
			{
				_customPassManager.GetProperties(config.passId).SetFloat("_Strength", 0f);
				if (_passEnabled)
				{
					_customPassManager.SetPassEnabled(config.passId, enabled: false);
				}
			}
		}
	}
}

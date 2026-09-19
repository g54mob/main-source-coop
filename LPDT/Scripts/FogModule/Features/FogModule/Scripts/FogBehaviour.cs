using System;
using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core;
using Features.GameUpdaterModule;
using Global.SerializableDictionary;
using UnityEngine;
using Zenject;

namespace Features.FogModule.Scripts
{
	[Serializable]
	public abstract class FogBehaviour : BeachBehaviour
	{
		protected readonly struct FogSnapshot
		{
			public readonly bool RestoreFogEnabled;

			public readonly bool RestoreFogMode;

			public readonly bool RestoreFogColor;

			public readonly bool RestoreFogStartDistance;

			public readonly bool RestoreFogEndDistance;

			public readonly bool FogEnabled;

			public readonly FogMode FogMode;

			public readonly Color FogColor;

			public readonly float FogStartDistance;

			public readonly float FogEndDistance;

			public FogSnapshot(bool restoreFogEnabled, bool restoreFogMode, bool restoreFogColor, bool restoreFogStartDistance, bool restoreFogEndDistance, bool fogEnabled, FogMode fogMode, Color fogColor, float fogStartDistance, float fogEndDistance)
			{
				RestoreFogEnabled = restoreFogEnabled;
				RestoreFogMode = restoreFogMode;
				RestoreFogColor = restoreFogColor;
				RestoreFogStartDistance = restoreFogStartDistance;
				RestoreFogEndDistance = restoreFogEndDistance;
				FogEnabled = fogEnabled;
				FogMode = fogMode;
				FogColor = fogColor;
				FogStartDistance = fogStartDistance;
				FogEndDistance = fogEndDistance;
			}
		}

		[Tooltip("Enable or disable fog while this beach preset is active.")]
		[SerializeField]
		private OptionalValue<bool> _fogEnabled;

		[Tooltip("Unity fog mode applied while this beach preset is active.")]
		[SerializeField]
		private OptionalValue<FogMode> _fogMode = new OptionalValue<FogMode>(FogMode.Linear);

		[Tooltip("Fog preset location applied once when fog transitions are disabled.")]
		[SerializeField]
		private FogLocationType _defaultFogLocation;

		[Tooltip("Fog presets referenced by the fog locations authored on the level's fog regions.")]
		[SerializeField]
		private SerializableDictionary<FogLocationType, FogPreset> _fogPresets;

		[Tooltip("When enabled, fog color/start/end distance are updated every frame from the fog region the local player is inside.")]
		[SerializeField]
		private bool _fogTransitionEnabled;

		private IGameUpdater _gameUpdater;

		private FogRegionsModel _fogRegionsModel;

		private bool _hasSnapshot;

		private FogSnapshot _snapshot;

		public override int Order => 25;

		[Inject]
		public void InjectDependencies(IGameUpdater gameUpdater, FogRegionsModel fogRegionsModel)
		{
			_gameUpdater = gameUpdater;
			_fogRegionsModel = fogRegionsModel;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			if (!_hasSnapshot)
			{
				_snapshot = CaptureFogSnapshot();
				_hasSnapshot = true;
			}
			SetOptionalValue(_fogEnabled, SetFogEnabled);
			SetOptionalValue(_fogMode, SetFogMode);
			if (!_fogTransitionEnabled && TryGetFogPreset(_defaultFogLocation, out var fogPreset))
			{
				ApplyFogPreset(fogPreset);
			}
			_gameUpdater.OnUpdate += OnUpdate;
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
			_gameUpdater.OnUpdate -= OnUpdate;
			if (_hasSnapshot)
			{
				RestoreFogSnapshot(_snapshot);
				_hasSnapshot = false;
			}
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult result = new BeachValidationResult();
			ValidateFogPresets(result);
			return result;
		}

		protected abstract Color GetFogColor();

		protected abstract float GetFogStartDistance();

		protected abstract float GetFogEndDistance();

		protected abstract FogMode GetFogMode();

		protected abstract bool IsFogEnabled();

		protected abstract void SetFogColor(Color fogColor);

		protected abstract void SetFogStartDistance(float startDistance);

		protected abstract void SetFogEndDistance(float endDistance);

		protected abstract void SetFogMode(FogMode mode);

		protected abstract void SetFogEnabled(bool enabled);

		private void OnUpdate()
		{
			if (_fogTransitionEnabled && !_fogRegionsModel.DebugFogTransitionEnabled)
			{
				FogBlend currentBlend = _fogRegionsModel.CurrentBlend;
				if (currentBlend.IsValid && TryGetFogPreset(currentBlend.StartLocation, out var fogPreset) && TryGetFogPreset(currentBlend.EndLocation, out var fogPreset2))
				{
					SetOptionalValue(new OptionalValue<Color>(Color.Lerp(fogPreset.FogColor.Value, fogPreset2.FogColor.Value, currentBlend.Ratio), GetCommonEnabled(fogPreset.FogColor, fogPreset2.FogColor)), SetFogColor);
					SetOptionalValue(new OptionalValue<float>(Mathf.Lerp(fogPreset.StartDistance.Value, fogPreset2.StartDistance.Value, currentBlend.Ratio), GetCommonEnabled(fogPreset.StartDistance, fogPreset2.StartDistance)), SetFogStartDistance);
					SetOptionalValue(new OptionalValue<float>(Mathf.Lerp(fogPreset.EndDistance.Value, fogPreset2.EndDistance.Value, currentBlend.Ratio), GetCommonEnabled(fogPreset.EndDistance, fogPreset2.EndDistance)), SetFogEndDistance);
				}
			}
		}

		private bool GetCommonEnabled<T>(OptionalValue<T> lhs, OptionalValue<T> rhs)
		{
			if (lhs.Enabled)
			{
				return rhs.Enabled;
			}
			return false;
		}

		private FogSnapshot CaptureFogSnapshot()
		{
			bool flag = ShouldSnapshotFogColor();
			bool flag2 = ShouldSnapshotFogStartDistance();
			bool flag3 = ShouldSnapshotFogEndDistance();
			return new FogSnapshot(_fogEnabled.Enabled, _fogMode.Enabled, flag, flag2, flag3, IsFogEnabled(), GetFogMode(), flag ? GetFogColor() : default(Color), flag2 ? GetFogStartDistance() : 0f, flag3 ? GetFogEndDistance() : 0f);
		}

		private void RestoreFogSnapshot(FogSnapshot snapshot)
		{
			if (snapshot.RestoreFogEnabled)
			{
				SetFogEnabled(snapshot.FogEnabled);
			}
			if (snapshot.RestoreFogMode)
			{
				SetFogMode(snapshot.FogMode);
			}
			if (snapshot.RestoreFogColor)
			{
				SetFogColor(snapshot.FogColor);
			}
			if (snapshot.RestoreFogStartDistance)
			{
				SetFogStartDistance(snapshot.FogStartDistance);
			}
			if (snapshot.RestoreFogEndDistance)
			{
				SetFogEndDistance(snapshot.FogEndDistance);
			}
		}

		private bool ShouldSnapshotFogColor()
		{
			if (!_fogTransitionEnabled)
			{
				if (TryGetFogPreset(_defaultFogLocation, out var fogPreset))
				{
					return fogPreset.FogColor.Enabled;
				}
				return false;
			}
			return true;
		}

		private bool ShouldSnapshotFogStartDistance()
		{
			if (!_fogTransitionEnabled)
			{
				if (TryGetFogPreset(_defaultFogLocation, out var fogPreset))
				{
					return fogPreset.StartDistance.Enabled;
				}
				return false;
			}
			return true;
		}

		private bool ShouldSnapshotFogEndDistance()
		{
			if (!_fogTransitionEnabled)
			{
				if (TryGetFogPreset(_defaultFogLocation, out var fogPreset))
				{
					return fogPreset.EndDistance.Enabled;
				}
				return false;
			}
			return true;
		}

		private void ApplyFogPreset(FogPreset fogPreset)
		{
			if (!(fogPreset == null))
			{
				SetOptionalValue(fogPreset.FogColor, SetFogColor);
				SetOptionalValue(fogPreset.StartDistance, SetFogStartDistance);
				SetOptionalValue(fogPreset.EndDistance, SetFogEndDistance);
			}
		}

		private void ValidateFogPresets(BeachValidationResult result)
		{
			if (!_fogTransitionEnabled)
			{
				ValidateDefaultFogPreset(result);
				return;
			}
			if (_fogPresets == null || _fogPresets.Count == 0)
			{
				result.AddError(base.DisplayName + ": fog transitions are enabled but no fog presets are assigned.");
				return;
			}
			foreach (KeyValuePair<FogLocationType, FogPreset> fogPreset in _fogPresets)
			{
				if (fogPreset.Key == FogLocationType.None)
				{
					result.AddError(base.DisplayName + ": fog presets cannot use FogLocationType.None.");
				}
				if (fogPreset.Value == null)
				{
					result.AddError($"{base.DisplayName}: fog preset for {fogPreset.Key} is missing.");
				}
				else
				{
					ValidateFogPreset(result, fogPreset.Key.ToString(), fogPreset.Value);
				}
			}
			ValidateFogLocationCoverage(result);
		}

		private void ValidateFogLocationCoverage(BeachValidationResult result)
		{
			foreach (FogLocationType value in Enum.GetValues(typeof(FogLocationType)))
			{
				if (value != FogLocationType.None && !TryGetFogPreset(value, out var _))
				{
					result.AddError($"{base.DisplayName}: fog location {value} has no matching fog preset — a fog region authored with it cannot be resolved.");
				}
			}
		}

		private void ValidateDefaultFogPreset(BeachValidationResult result)
		{
			FogPreset fogPreset;
			if (_defaultFogLocation == FogLocationType.None)
			{
				result.AddError(base.DisplayName + ": fog transitions are disabled but default fog location is None.");
			}
			else if (!TryGetFogPreset(_defaultFogLocation, out fogPreset))
			{
				result.AddError($"{base.DisplayName}: fog transitions are disabled but default fog location {_defaultFogLocation} has no matching fog preset.");
			}
			else
			{
				ValidateFogPreset(result, _defaultFogLocation.ToString(), fogPreset);
			}
		}

		private void ValidateFogPreset(BeachValidationResult result, string presetName, FogPreset fogPreset)
		{
			if (fogPreset.StartDistance.Enabled && fogPreset.StartDistance.Value < 0f)
			{
				result.AddError(base.DisplayName + ": fog preset " + presetName + " start distance cannot be negative.");
			}
			if (fogPreset.EndDistance.Enabled && fogPreset.EndDistance.Value < 0f)
			{
				result.AddError(base.DisplayName + ": fog preset " + presetName + " end distance cannot be negative.");
			}
			if (GetCommonEnabled(fogPreset.StartDistance, fogPreset.EndDistance) && fogPreset.StartDistance.Value > fogPreset.EndDistance.Value)
			{
				result.AddError(base.DisplayName + ": fog preset " + presetName + " start distance must not exceed end distance.");
			}
		}

		private bool TryGetFogPreset(FogLocationType fogLocationType, out FogPreset fogPreset)
		{
			fogPreset = null;
			if (_fogPresets == null)
			{
				return false;
			}
			if (_fogPresets.TryGetValue(fogLocationType, out fogPreset))
			{
				return fogPreset != null;
			}
			return false;
		}

		private void SetOptionalValue<T>(OptionalValue<T> value, Action<T> setter)
		{
			if (value.Enabled)
			{
				setter(value.Value);
			}
		}
	}
}

using System;
using Features.BeachPresetModule.Scripts.Core;
using Features.PostProcessingModule.Scripts;
using INab.BetterFog.URP;
using UnityEngine;
using Zenject;

namespace Features.FogModule.Scripts
{
	[Serializable]
	public class VolumetricFogBehaviour : FogBehaviour
	{
		[Tooltip("Volume slot from PostProcessingModel / VolumeAutoRegister (scene must register this type).")]
		[SerializeField]
		private PostProcessingType _postProcessingVolumeKey = PostProcessingType.Fog;

		private PostProcessingModel _postProcessingModel;

		[Inject]
		public void InjectDependencies(PostProcessingModel postProcessingModel)
		{
			_postProcessingModel = postProcessingModel;
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = base.Validate(preset);
			if (_postProcessingVolumeKey == PostProcessingType.None)
			{
				beachValidationResult.AddError(base.DisplayName + ": assign a Post Processing volume key (typically Fog).");
			}
			return beachValidationResult;
		}

		protected override Color GetFogColor()
		{
			if (TryGetBetterFog(out var betterFog))
			{
				return betterFog._FogColor.value;
			}
			return Color.black;
		}

		protected override float GetFogStartDistance()
		{
			if (TryGetBetterFog(out var betterFog))
			{
				return betterFog._SceneStart.value;
			}
			return 0f;
		}

		protected override float GetFogEndDistance()
		{
			if (TryGetBetterFog(out var betterFog))
			{
				return betterFog._SceneEnd.value;
			}
			return 0f;
		}

		protected override FogMode GetFogMode()
		{
			if (TryGetBetterFog(out var betterFog))
			{
				return betterFog._FogType.value;
			}
			return FogMode.Linear;
		}

		protected override bool IsFogEnabled()
		{
			if (TryGetBetterFog(out var betterFog))
			{
				return betterFog.active;
			}
			return false;
		}

		protected override void SetFogColor(Color fogColor)
		{
			if (TryGetBetterFog(out var betterFog))
			{
				betterFog._FogColor.value = fogColor;
				betterFog._FogColor.overrideState = true;
			}
		}

		protected override void SetFogStartDistance(float startDistance)
		{
			if (TryGetBetterFog(out var betterFog))
			{
				betterFog._SceneStart.value = startDistance;
				betterFog._SceneStart.overrideState = true;
			}
		}

		protected override void SetFogEndDistance(float endDistance)
		{
			if (TryGetBetterFog(out var betterFog))
			{
				betterFog._SceneEnd.value = endDistance;
				betterFog._SceneEnd.overrideState = true;
			}
		}

		protected override void SetFogMode(FogMode mode)
		{
			if (TryGetBetterFog(out var betterFog))
			{
				betterFog._FogType.value = mode;
				betterFog._FogType.overrideState = true;
			}
		}

		protected override void SetFogEnabled(bool enabled)
		{
			if (TryGetBetterFog(out var betterFog))
			{
				betterFog.active = enabled;
			}
		}

		private bool TryGetBetterFog(out BetterFogVolumeComponent betterFog)
		{
			betterFog = null;
			if (_postProcessingVolumeKey == PostProcessingType.None)
			{
				return false;
			}
			if (!_postProcessingModel.ActiveVolumes.TryGetValue(_postProcessingVolumeKey, out var value) || value == null || value.profile == null)
			{
				return false;
			}
			return value.profile.TryGet<BetterFogVolumeComponent>(out betterFog);
		}
	}
}
